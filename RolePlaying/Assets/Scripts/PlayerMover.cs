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

	//FreeLook Camera : � ����� �������� ī�޶� �ٷιٴ� ����� ī�޶���

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

		if (moveDir.magnitude == 0) //���� ũ�Ⱑ 0�̸�? (�� ���� ��쿡��)
			return; //move ��ü�� �� �ϵ��� ��.

		animator.SetFloat("MoveSpeed", curSpeed);
		Move(curSpeed);
	}

	private void Move(float speed)
	{
		//ī�޶��� ���� + ���򼱰� ������ ������ ������ �־�� �� 
		//������ ������� ������,
		//	-> ī�޶� ������ ���� ���� �� forward�� �ϸ� ������ �����ų�, 
		//	-> ī�޶� �Ʒ����� ���� ���� �� forward�� �ϸ� ���� �ھƿ����� ������ �߻��� �� ���� 
		//y�� 0���� ����.
		Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);

		//�ٵ�, y���� ���Ƿ� 0���� ���߰� �Ǹ� ���� ������ ũ�Ⱑ �پ��� ��. -> �ӵ��� �ٱ� ������. 
		//���⼺�� ���� ���͸� ����� ��쿡�� ���͸� 1�� ����� ��� ��. -> ������ �Ϲ�ȭ�� �̿��ϸ� ��! 
		//normalized�� ������ ũ�Ⱑ 1�� ���� ��ȯ���ִ� ��. (����ȭ ����)
		//	-> ������ ���⿡ ���� �����̴� �ӵ��� �޶��� �� ����
		//		-> ������ �ӵ��� �̵��ϰ� �ʹٸ� ���͸� 1�� ������ִ� �Ϲ�ȭ(normalized) ������ �ʿ���
		forwardVec = forwardVec.normalized;
		//forwardVec.Normalize(); //���͸� 1�� ������ִ� �޼ҵ�

		Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

		characterController.Move(forwardVec * moveDir.z * speed * Time.deltaTime);
		characterController.Move(rightVec * moveDir.x * speed * Time.deltaTime);

		//transform.rotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
		//ȸ�� ���� �ٷ� �ָ� �������� ������ ��. -> �ڿ������� ȸ���� ���� ���������� ���ֵ��� ��
		Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
		transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.2f); //���� ��Ȳ���� ���������� 0.2������ ���������� ��

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
		ySpeed += Physics.gravity.y * Time.deltaTime; //�߷��� ������ ���� �� ����

		//isGrounded�� ��Ȯ���� ���� ����������... �ð� �� �׳� ����ϴ� ��..!
		//���� ���� ������ �� ���� isGrounded�� ���� �������ֵ��� ��!!
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
