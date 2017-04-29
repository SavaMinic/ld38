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

	#endregion

	#region Properties

	public CharState State { get; private set; }

	private bool IsIdle { get { return State == CharState.Idle; } }
	private bool IsJumping { get { return State == CharState.Jumping; } }
	private bool IsAttacking { get { return State == CharState.Attacking; } }

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
		if (IsIdle && Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(AttackAnimation());
		}
		// Handle jump
		if (IsIdle && Input.GetKeyDown(KeyCode.E))
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
	}

	#endregion

	#region Coroutines

	private IEnumerator JumpAnimation()
	{
		State = CharState.Jumping;
		animator.ResetTrigger("finishFall");
		animator.SetTrigger("jump");

		yield return new WaitForSeconds(jumpAnimationPrepareTime);

		var force = transform.forward * jumpIntensity.x + transform.up * jumpIntensity.y;
		rigidBody.AddForce(force);
	}

	private IEnumerator AttackAnimation()
	{
		State = CharState.Attacking;
		animator.SetTrigger("attack");

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
			State = CharState.Idle;
			animator.ResetTrigger("jump");
			animator.SetTrigger("finishFall");
		}
	}

	#endregion
}
