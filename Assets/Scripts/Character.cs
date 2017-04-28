using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Vector2 walkingSpeed;

	[SerializeField]
	private Vector2 sneakingSpeed;
	
	// Update is called once per frame
	void Update()
	{
		var dx = Input.GetAxis("Horizontal");
		var dy = Input.GetAxis("Vertical");
		var sneaking = Input.GetKey(KeyCode.LeftShift);

		animator.SetFloat("speedX", dx);
		animator.SetFloat("speedY", dy);

		var speed = sneaking ? sneakingSpeed : walkingSpeed;
		var direction = new Vector3(
			-dx * speed.x,
			0f,
			-dy * speed.y
        );
		transform.Translate(direction * Time.deltaTime);

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
}
