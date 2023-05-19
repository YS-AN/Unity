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

	//������������ �ֱ������� ����������� -> �ڷ�ƾ ���
	Coroutine spawnRoution;

	IEnumerator SpawnRoutine()
	{
		while(true)
		{
			yield return new WaitForSeconds(SpawnDelay);

			Vector2 spawnPos = transform.position + Vector3.up * Random.Range(-RandomRange, RandomRange); // ��ġ�� �������� ������

			Instantiate(PipleLinePrefab, spawnPos, transform.rotation);
		}
	}
}
