using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	Transform spawnPoint; //몬스터 만들 위치

	[SerializeField]
	private float spawnTime; //얼마나 자주 만들지?

	[SerializeField]
	private GameObject enemyPrefab; //어떤 몬스터 만들지?

	private Coroutine createEnemyRoutine;

	private void OnEnable()
	{
		createEnemyRoutine = StartCoroutine(SpawnRoutine());
	}

	private void OnDisable()
	{
		StopCoroutine(createEnemyRoutine);
	}

	IEnumerator SpawnRoutine()
	{
		while(true)
		{
			yield return new WaitForSeconds(spawnTime);
			Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
		}
	}
}
