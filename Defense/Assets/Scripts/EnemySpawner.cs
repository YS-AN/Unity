using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	Transform spawnPoint; //���� ���� ��ġ

	[SerializeField]
	private float spawnTime; //�󸶳� ���� ������?

	[SerializeField]
	private GameObject enemyPrefab; //� ���� ������?

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
