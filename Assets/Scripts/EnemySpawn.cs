using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

	public GameObject EnemyPrefab;
	public float spawnTimer;

	private float nextSpawnTime;

	void Awake()
	{
		nextSpawnTime = Time.time + spawnTimer;
	}

	void Update()
	{
		if (GameManager.Instance.IsPlaying && Time.time > nextSpawnTime)
		{
			nextSpawnTime = Time.time + spawnTimer;
			var enemy = Instantiate(EnemyPrefab);
			enemy.transform.parent = transform.parent;
			enemy.transform.position = transform.position;
		}
	}
}
