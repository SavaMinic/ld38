using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
	private GoTween scalingTransition;

	private Rigidbody rigidBody;

	public ParticleSystem explosion;

	private bool isFound;

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision collision)
	{
		if (!isFound && collision.collider.gameObject.layer == LayerMask.NameToLayer("Spider"))
		{
			isFound = true;
			rigidBody.AddForce(-collision.collider.transform.forward * 7f);
			StartCoroutine(ExplosionAnimation());
		}
	}

	private IEnumerator ExplosionAnimation()
	{
		explosion.Play();
		scalingTransition = Go.to(transform, .3f, new GoTweenConfig()
			.scale(0.005f)
			.setIterations(3, GoLoopType.PingPong)
		);

		yield return new WaitForSeconds(1.5f);

		GameManager.Instance.GranadeFound();
		Go.to(transform, .5f, new GoTweenConfig()
			.scale(0f)
		);

		yield return new WaitForSeconds(.5f);
		GameObject.Destroy(gameObject);
	}

	void OnDestroy()
	{
		if (scalingTransition != null)
		{
			scalingTransition.destroy();
		}
	}
}
