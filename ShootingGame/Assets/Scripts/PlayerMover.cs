using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

//3D�� ���� ���� �ݵ�� ���� ��ġ�� ĳ������ �ٴڿ� �α�!

public class PlayerMover : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private float jumpSpeed;

	private CharacterController characterController; //���� �����̳� Translate�� �ƴ� �̵��� ���̴� ��Ʈ�ѷ�
	private Vector3 moveDir;

	private float ySpeed; //y���� �������� ������ �ִ� �ӷ�


	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		Move();
		Jump();
	}

	

	private void OnMove(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();
		moveDir = new Vector3(input.x, 0, input.y);
	}

	private void Move()
	{
		//Translate�� �ֺ��� ���ӻ�Ȳ�� ������� �ʰ�, ���� �浹��ü�� ������ �浹���� �ʰ� �Ѿ
		//CharacterController�� �ֺ���Ȳ�� ���� �� �� �ִ� ������ �� �� ���� ������ üũ�ϸ鼭 �ٴϴ� �̵� (�뷫�� ������ ��ǥ�� Ȱ����)  -> ex. ����̳� ��� ������ ���� �� �ֵ��� ��.

		//but. CharacterController�� �߷��� ������ ���� �ʾ�. �߷¿� ���� ó���� �ʿ��� -> y��ġ�� ĳ���� ������ ���� ������ �����.

		//characterController.Move(moveDir * moveSpeed * Time.deltaTime); //�ӷ� = ���ǵ� * Time.deltaTime(�����ð�)
		//�̷��� �����ϸ�, world�������� �����̱� ������ ȸ���� �� �̵��� �����.
		// ex.���� �¸� �����µ� ��� �����̴� ���� �߻���.

		//�ٷκ��� ������ �������� �����̵��� ������
		characterController.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime); //���� �ٷκ��� ������ �������� z�� �������� �̵�, 
		characterController.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime); //���� �ٷκ��� ������ �������� x�� �������� �̵���. (�����̸� -1�����ְ�, �������� 1�� �����ִϱ� �ڿ������� ���ϴ� �����̵��� ��������)
	}

	//y��ġ�� ������ �߷��� �ִ� ��ó�� ����� �ִ� �Լ�
	private void Jump()
	{
		//���� ������ ���������� �������� �׸��鼭 ������. 
		ySpeed += Physics.gravity.y * Time.deltaTime; //Physics.gravity.y : �߷��� y���� ����
													  //  -> project setting�� Physics ���� ���� ������
													  //�߷��� ��� ���� �ִ� ��� ��� �ϵ��� ��.


		/* //isGrounded�� �������� �� �۵��� �ȵ� (�״� �������� ����) -> ��� ���� 
		 * //�扛ü üũ�� raycast Ȱ���ؼ� �ٴ� üũ�ϵ���!!
		if(characterController.isGrounded) //���� ���� ���������? 
		{
			ySpeed = 0;	//y�ӷ��� 0���� �����ؼ� �� ������ ������ �ʵ��� ��.
		}
		//*/

		if(IsGround() && ySpeed < 0) //ySpeed < 0 : �̹� �پ�ö� �߷��� �޾� �������� �ִ� �߿��� �׶��� üũ�� �ϵ��� ��
		{
			ySpeed = 0;
		}

		characterController.Move(Vector3.up * ySpeed * Time.deltaTime); 
	}

	private void OnJump()
	{
		//���� = ���� �ӷ��� ���� �Ѵ�
		ySpeed = jumpSpeed;
	}

	private bool IsGround()
	{
		//raycast�� 2D�� 3D�� ��¦ �ٸ�
		//	3D : raycast ����� ������ true, �� ������ false��.-> RaycastHit�� ���������� ���� ����
		//	2D : ��ȯ���� RaycastHit��. -> �浹 �� �ϸ� null�� ����.
		
		//3D ���� ��� �ܼ��� ������ �ϸ� �ָ��ϰ� �浹�� �� ���� �� ������ �������� Ȯ���ϵ��� ��
		RaycastHit hit;
		return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.7f);
								//��𿡼�, ��� �ѷ���, ��� ��������, hit������, ��� ���̷�, ���� �����ϱ�
	}
}
