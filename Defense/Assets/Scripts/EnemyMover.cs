using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour
{
	//네비게이션 시스템은 연산이 많이 들어가는 작업임. 
	//너무 많은 연산이 일어나지 않도록 연산량에 대한 주의가 필요함. 

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
		//현재 위치가 endPoint의 위치가 근접한다면? (그 차이가 0.1보다 작다면?)
		if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
		{
			Destroy(gameObject); //현재 obj 삭제
		}
	}
}
