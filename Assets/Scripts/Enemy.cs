using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Renderer sphereRenderer;
	public float healthDecreaseOnHit;
	public float nextHitDelay;
	public float hitRigidForce;

	public ParticleSystem loopParticles;
	public ParticleSystem deathParticles;
	public ParticleSystem hitParticles;

	private Rigidbody rigidBody;
	private int spiderLayer;
	private Character spider;

	private float healthRatio = 1f;
	public float HealthRatio
	{
		get { return healthRatio; }
		set
		{
			healthRatio = Mathf.Clamp01(value);
			sphereRenderer.material.SetFloat("_Alpha", healthRatio);
		}
	}

	private float nextHitTime;
	private bool isActive = true;

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		spiderLayer = LayerMask.NameToLayer("Spider");
		spider = FindObjectOfType<Character>();
	}

	void OnCollisionEnter(Collision collision)
	{
		if (isActive && nextHitTime < Time.time && spider.IsAttacking && collision.collider.gameObject.layer == spiderLayer)
		{
			// HIT
			hitParticles.transform.position = collision.contacts[0].point;
			hitParticles.Play();
			rigidBody.AddForce(-collision.collider.transform.forward * hitRigidForce);

			// decrase health
			HealthRatio -= healthDecreaseOnHit;
			nextHitTime = Time.time + nextHitDelay;

			if (HealthRatio <= 0f)
			{
				isActive = false;
				StartCoroutine(DeathAnimation());
			}
		}
	}

	private IEnumerator DeathAnimation()
	{
		loopParticles.Stop();
		deathParticles.Play();

		yield return new WaitForSeconds(2f);

		Destroy(gameObject);
	}
}
