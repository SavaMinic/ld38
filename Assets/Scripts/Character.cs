using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class Character : MonoBehaviour
{
	public enum CharState
	{
		Idle,
		Jumping,
		Attacking
	}

	#region Fields

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private CameraController cameraController;

	private Rigidbody rigidBody;

	[Header("Movement & Rotation")]
	[SerializeField]
	private float walkingSpeed;

	[SerializeField]
	private float sneakingSpeed;

	[SerializeField]
	private float moveWhileAttackingSpeed;

	[SerializeField]
	private float normalRotationSpeed;

	[SerializeField]
	private float sneakingRotationSpeed;

	[Header("Jumping")]
	[SerializeField]
	private Vector3 jumpIntensity;

	[SerializeField]
	private float jumpAnimationPrepareTime;

	[Header("Attacking")]
	[SerializeField]
	private float attackAnimationTime;

	[SerializeField]
	private float attackDisplacement;

	[Header("Sounds")]
	[SerializeField]
	private AudioSource fastRuningBassAudio;

	[SerializeField]
	private float fastRunningTransitionDuration;

	[SerializeField]
	private AudioSource jumpingAudio;

	[SerializeField]
	private AudioSource mainAudio;

	[SerializeField]
	private AudioClip jumpSound;

	[SerializeField]
	private AudioClip landSound;

	private GoTween fastRunningTransition;
	private GoTween jumpTransition;

	#endregion

	#region Properties

	public CharState State { get; private set; }

	public bool IsIdle { get { return State == CharState.Idle; } }
	public bool IsJumping { get { return State == CharState.Jumping; } }
	public bool IsAttacking { get { return State == CharState.Attacking; } }

	#endregion

	#region Unity

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		State = CharState.Idle;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!GameManager.Instance.IsPlaying)
			return;
		
		var dx = Input.GetAxis("Horizontal");
		var dy = Input.GetAxis("Vertical");
		var sneaking = Input.GetKey(KeyCode.LeftShift);

		// Handle logic movements
		if (dy != 0f)
		{
			var speed = IsAttacking ? moveWhileAttackingSpeed 
				: (sneaking ? sneakingSpeed : walkingSpeed);
			transform.Translate(Vector3.forward * (-dy) * speed * Time.deltaTime);
		}
		if (dx != 0f)
		{
			var rotationSpeed = sneaking ? sneakingRotationSpeed : normalRotationSpeed;
			transform.Rotate(0f, dx * rotationSpeed * Time.deltaTime, 0f);
		}

		// Handle attack
		if (IsIdle && Input.GetKeyDown(KeyCode.E))
		{
			StartCoroutine(AttackAnimation());
		}
		// Handle jump
		if (IsIdle && Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(JumpAnimation());
		}

		// Handle animations
		animator.SetFloat("speedX", dx);
		animator.SetFloat("speedY", dy);
		if (dx != 0f || dy != 0f)
		{
			animator.SetBool("walking", true);
			animator.SetBool("sneaking", sneaking);
		}
		else
		{
			animator.SetBool("walking", false);
			animator.SetBool("sneaking", false);
		}

		// Handle audio for running
		if (!IsJumping && Input.GetKeyDown(KeyCode.LeftShift))
		{
			if (fastRunningTransition != null)
				fastRunningTransition.destroy();
			fastRunningTransition = Go.to(fastRuningBassAudio, fastRunningTransitionDuration, new GoTweenConfig().floatProp("volume", 1f));
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			if (fastRunningTransition != null)
				fastRunningTransition.destroy();
			fastRunningTransition = Go.to(fastRuningBassAudio, fastRunningTransitionDuration, new GoTweenConfig().floatProp("volume", 0f));
		}
	}

	#endregion

	#region Coroutines

	private IEnumerator JumpAnimation()
	{
		State = CharState.Jumping;
		animator.ResetTrigger("finishFall");
		animator.SetTrigger("jump");

		yield return new WaitForSeconds(jumpAnimationPrepareTime);

		//cameraController.SetDepthOfField(true);
		var force = transform.forward * jumpIntensity.x + transform.up * jumpIntensity.y;
		rigidBody.AddForce(force);

		jumpingAudio.volume = 1f;
		jumpingAudio.PlayOneShot(jumpSound);
		mainAudio.volume = 0.4f;
		jumpTransition = Go.to(jumpingAudio, 6f, new GoTweenConfig().floatProp("volume", 0.2f).setDelay(1.5f));
		jumpTransition.setOnCompleteHandler(t => {
			mainAudio.volume = 0.65f;
			jumpingAudio.Stop();
		});
	}

	private IEnumerator AttackAnimation()
	{
		State = CharState.Attacking;
		animator.SetTrigger("attack");

		rigidBody.AddForce(-transform.forward * attackDisplacement);
		yield return new WaitForSeconds(attackAnimationTime);

		animator.ResetTrigger("attack");
		State = CharState.Idle;
	}

	#endregion

	#region Public API

	public void FinishFall()
	{
		if (IsJumping)
		{
			if (jumpTransition != null)
			{
				jumpTransition.complete();
				jumpTransition.destroy();
			}
			jumpingAudio.volume = 1f;
			jumpingAudio.PlayOneShot(landSound);
			State = CharState.Idle;
			animator.ResetTrigger("jump");
			animator.SetTrigger("finishFall");
			//cameraController.SetDepthOfField(false);
		}
	}

	#endregion
}
