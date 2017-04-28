using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[Header("Movement & Rotation")]
	[SerializeField]
	private float walkingSpeed;

	[SerializeField]
	private float sneakingSpeed;

	[SerializeField]
	private float normalRotationSpeed;

	[SerializeField]
	private float sneakingRotationSpeed;

	private bool isJumping;
	
	// Update is called once per frame
	void Update()
	{
		var dx = Input.GetAxis("Horizontal");
		var dy = Input.GetAxis("Vertical");
		var sneaking = Input.GetKey(KeyCode.LeftShift);

		animator.SetFloat("speedX", dx);
		animator.SetFloat("speedY", dy);

		var speed = sneaking ? sneakingSpeed : walkingSpeed;
		transform.Translate(Vector3.forward * (-dy) * speed * Time.deltaTime);

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

		if (dx != 0f)
		{
			var rotationSpeed = sneaking ? sneakingRotationSpeed : normalRotationSpeed;
			transform.Rotate(0f, dx * rotationSpeed * Time.deltaTime, 0f);
		}

		if (!isJumping && Input.GetKeyDown(KeyCode.Space))
		{
			//isJumping = true;
			animator.SetTrigger("jump");
		}
	}
}
