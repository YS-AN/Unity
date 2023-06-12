using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CustomerInfo
{
	private Seat _seat;
	public Seat Seat { get { return _seat; } }

	private Chair _chair;
	public Chair Chair { get { return _chair; } }

	public void Init(Seat seat, string chairNm)
	{
		_seat = seat;
		_chair = _seat.GetComponentsInChildren<Chair>().Where(x => x.name == chairNm).FirstOrDefault();
	}
}

public class CustomerMover : MonoBehaviour
{
	private Coroutine moveRoutine;

	private NavMeshAgent meshAgent;
	private Animator animator;

	public CustomerInfo info;

	public Action OnMove;

	private void Awake()
	{
		info = new CustomerInfo();

		meshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();	

		OnMove += Move;
	}

	private void OnDisable()
	{
		if(moveRoutine != null)
			StopCoroutine(moveRoutine);
	}

	private	void Move()
	{
		moveRoutine = StartCoroutine(MoveRoutine());
	}

	IEnumerator MoveRoutine()
	{
		animator.SetTrigger("Move");

		meshAgent.destination = info.Chair.StopPoint.position;

		while (Vector3.Distance(transform.position, info.Chair.StopPoint.position) > 0.1f)
		{
			animator.SetTrigger("Side");
			yield return null;
		}
	}
}
