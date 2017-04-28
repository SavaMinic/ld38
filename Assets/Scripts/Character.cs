using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
	[SerializeField]
	private Animator animator;
	
	// Update is called once per frame
	void Update()
	{
		var dx = Input.GetAxis("Horizontal");
		var dy = Input.GetAxis("Vertical");

		animator.SetFloat("speedX", dx);
		animator.SetFloat("speedY", dy);

		if (dx != 0f || dy != 0f)
		{
			animator.SetBool("walking", true);
		}
		else
		{
			animator.SetBool("walking", false);
		}
	}
}
