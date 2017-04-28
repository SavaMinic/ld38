using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class Character : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	private Rigidbody rigidBody;

	[Header("Movement & Rotation")]
	[SerializeField]
	private float walkingSpeed;

	[SerializeField]
	private float sneakingSpeed;

	[SerializeField]
	private float normalRotationSpeed;

	[SerializeField]
	private float sneakingRotationSpeed;

	[Header("Jumping")]
	[SerializeField]
	private Vector3 jumpIntensity;

	[SerializeField]
	private float jumpAnimationPrepareTime;

	private bool isJumping;

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
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
			var speed = sneaking ? sneakingSpeed : walkingSpeed;
			transform.Translate(Vector3.forward * (-dy) * speed * Time.deltaTime);
		}
		if (dx != 0f)
		{
			var rotationSpeed = sneaking ? sneakingRotationSpeed : normalRotationSpeed;
			transform.Rotate(0f, dx * rotationSpeed * Time.deltaTime, 0f);
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

	void FixedUpdate()
	{
		// Handle jump
		if (!isJumping && Input.GetKeyDown(KeyCode.Space))
		{
			//isJumping = true;
			StartCoroutine(JumpAnimation());
		}
	}

	#region Coroutines

	private IEnumerator JumpAnimation()
	{
		var dx = Input.GetAxis("Horizontal");
		animator.SetTrigger("jump");

		yield return new WaitForSeconds(jumpAnimationPrepareTime);

		var force = Mathf.Sign(dx) * transform.forward * jumpIntensity.x + transform.up * jumpIntensity.y;
		rigidBody.AddForce(force);
	}

	#endregion
}
