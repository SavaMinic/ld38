using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingCollider : MonoBehaviour
{

	[SerializeField]
	private LayerMask jumpResetLayerMask;

	private Character character;

	void Awake()
	{
		character = GetComponentInParent<Character>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (((1 << other.gameObject.layer) & jumpResetLayerMask) > 0)
		{
			character.FinishFall();
		}
	}
}
