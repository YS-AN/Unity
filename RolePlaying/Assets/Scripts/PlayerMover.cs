using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
	[SerializeField]
	private float WalkSpeed;

	[SerializeField]
	private float RunSpeed;

	[SerializeField]
	private float JumpSpeed;

	private CharacterController characterController;
	private Animator animator;

	private Vector3 moveDir;
	private float curSpeed;
	private float ySpeed;
	private bool isWalk;

	//FreeLook Camera : 어떤 대상을 기준으로 카메라가 바로바는 방식의 카메라임

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		Move();
		Fall();
	}

	private void Move()
	{
		curSpeed = GetMoveSpeed();

		if (moveDir.magnitude == 0) //백터 크기가 0이면? (안 누른 경우에는)
			return; //move 자체를 안 하도록 함.

		animator.SetFloat("MoveSpeed", curSpeed);
		Move(curSpeed);
	}

	private void Move(float speed)
	{
		//카메라의 방향 + 지평선과 동일한 방향을 가지고 있어야 해 
		//지평선을 고려하지 않으면,
		//	-> 카메라가 위에서 보고 있을 때 forward를 하면 땅으로 꺼지거나, 
		//	-> 카메라가 아래에서 보고 있을 때 forward를 하면 위로 솟아오르는 현상이 발생할 수 있음 
		//y는 0으로 만듦.
		Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);

		//근데, y값만 임의로 0으로 맞추게 되면 기존 백터의 크기가 줄어들게 됨. -> 속도가 줄기 때문임. 
		//방향성을 갖는 백터를 사용할 경우에는 백터를 1로 만들어 줘야 함. -> 백터의 일반화를 이용하면 돼! 
		//normalized는 백터의 크기가 1인 값을 반환해주는 것. (정규화 과정)
		//	-> 백터의 방향에 따라 움직이는 속도가 달라질 수 있음
		//		-> 동일한 속도로 이동하고 싶다면 백터를 1로 만들어주는 일반화(normalized) 과정이 필요함
		forwardVec = forwardVec.normalized;
		//forwardVec.Normalize(); //백터를 1로 만들어주는 메소드

		Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

		characterController.Move(forwardVec * moveDir.z * speed * Time.deltaTime);
		characterController.Move(rightVec * moveDir.x * speed * Time.deltaTime);

		//transform.rotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
		//회전 값을 바로 주면 끊어지는 느낌이 남. -> 자연스러운 회전을 위해 선형보간을 해주도록 함
		Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
		transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.2f); //현재 상황에서 목적지까지 0.2값으로 선형보간을 함

	}

	private float GetMoveSpeed()
	{
		if (moveDir.magnitude == 0)
			return Mathf.Lerp(curSpeed, 0, 0.1f);

		if (isWalk)
			return Mathf.Lerp(curSpeed, WalkSpeed, 0.1f);
		else 
			return Mathf.Lerp(curSpeed, RunSpeed, 0.1f);
	}

	private void OnMove(InputValue value)
	{
		var input = value.Get<Vector2>();
		moveDir.x = input.x;
		moveDir.z = input.y;
	}

	private void Jump()
	{
		ySpeed = JumpSpeed;
	}

	private void OnJump(InputValue value)
	{
		Jump();
	}

	private void Fall()
	{
		ySpeed += Physics.gravity.y * Time.deltaTime; //중력의 영향을 받을 수 있음

		//isGrounded가 정확도가 많이 떨어지지만... 시간 상 그냥 사용하는 것..!
		//실제 게임 제작을 할 때는 isGrounded를 직접 구현해주도록 함!!
		if (characterController.isGrounded && ySpeed < 0)
		{
			ySpeed = 0; 
		}
				
		characterController.Move(Vector3.up * ySpeed * Time.deltaTime);
	}

	private void OnWalk(InputValue value)
	{
		isWalk = value.isPressed;
	}
}
