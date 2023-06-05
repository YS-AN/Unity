using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour
{
	//�׺���̼� �ý����� ������ ���� ���� �۾���. 
	//�ʹ� ���� ������ �Ͼ�� �ʵ��� ���귮�� ���� ���ǰ� �ʿ���. 

	private Transform endPoint;
	private NavMeshAgent meshAgent;

	private void Awake()
	{
		meshAgent = GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		endPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
		meshAgent.destination = endPoint.position;
	}

	private void Update()
	{
		//���� ��ġ�� endPoint�� ��ġ�� �����Ѵٸ�? (�� ���̰� 0.1���� �۴ٸ�?)
		if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
		{
			Destroy(gameObject); //���� obj ����
		}
	}
}
