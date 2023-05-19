using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeLineSpawner : MonoBehaviour
{
	[SerializeField]
	private float SpawnDelay;

	[SerializeField]
	private PipeRoad PipleLinePrefab;

	[SerializeField]
	private float RandomRange;

	private void OnEnable()
	{
		spawnRoution = StartCoroutine(SpawnRoutine());
	}

	private void OnDisable()
	{
		StopCoroutine(spawnRoution);
	}

	//파이프라인이 주기적으로 만들어져야함 -> 코루틴 사용
	Coroutine spawnRoution;

	IEnumerator SpawnRoutine()
	{
		while(true)
		{
			yield return new WaitForSeconds(SpawnDelay);

			Vector2 spawnPos = transform.position + Vector3.up * Random.Range(-RandomRange, RandomRange); // 위치를 랜덤으로 조정함

			Instantiate(PipleLinePrefab, spawnPos, transform.rotation);
		}
	}
}
