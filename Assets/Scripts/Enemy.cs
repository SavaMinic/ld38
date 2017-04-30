using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Renderer sphereRenderer;
	public float healthDecreaseOnHit;
	public float nextHitDelay;
	public float disableTimeDuration;
	public float hitRigidForce;

	public float followingSpeed;
	public float distanceToFollow;
	public float timeToChangeTarget;
	public float roamingSpeed;
	public float roamingRange;

	public ParticleSystem loopParticles;
	public ParticleSystem deathParticles;
	public ParticleSystem hitParticles;

	private Rigidbody rigidBody;
	private int spiderLayer;
	private Character spider;

	private Vector3 targetVector;
	private float timeToChangeTargetVector;
	private bool isFollowingSpider;

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
	private float disabledTime;

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

			disabledTime = Time.time + disableTimeDuration;

			if (HealthRatio <= 0f)
			{
				isActive = false;
				StartCoroutine(DeathAnimation());
			}
		}
	}

	void Update()
	{
		if (!isActive)
			return;

		var wasFollowingSpider = isFollowingSpider;
		var distance = Vector3.Distance(transform.position, spider.transform.position);
		isFollowingSpider = distance <= distanceToFollow;

		if (!isFollowingSpider && Time.time > timeToChangeTargetVector)
		{
			ChangeTarget();
		}

		if (isFollowingSpider != wasFollowingSpider)
		{
			Debug.Log(gameObject.name + (isFollowingSpider ? " STARTED" : " STOP"));
		}
	}

	void FixedUpdate()
	{
		if (!isActive || Time.time <= disabledTime)
			return;
		
		var positionToFollow = isFollowingSpider ? spider.transform.position : targetVector;
		var speed = isFollowingSpider ? followingSpeed : roamingSpeed;
		var newVelocity = -(transform.position - positionToFollow).normalized * speed;
		newVelocity.y = 0f;
		rigidBody.MovePosition(transform.position + newVelocity * Time.deltaTime);
	}

	private float GetRand(float min, float max)
	{
		return Random.value * (max - min) + min;
	}

	private void ChangeTarget()
	{
		timeToChangeTargetVector = Time.time + timeToChangeTarget;
		targetVector = new Vector3(
			GetRand(transform.position.x - roamingRange, transform.position.x + roamingRange),
			0f,
			GetRand(transform.position.z - roamingRange, transform.position.z + roamingRange)
		);
	}

	private IEnumerator DeathAnimation()
	{
		loopParticles.Stop();
		deathParticles.Play();

		yield return new WaitForSeconds(2f);

		Destroy(gameObject);
	}
}
