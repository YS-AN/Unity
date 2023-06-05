using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private float ZoomSpeed;

	[SerializeField]
	private float MoveSpeed;

	[SerializeField]
	private float Padding; //�󸶳� �׵θ� �����̿� �������� ����

	
	private Vector3 moveDir;
	private float zoomScroll;

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Confined; //���콺�� ����â�� ����� ���ϰ� ����
	}

	private void nDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	private void LateUpdate()
	{
		Move();
		Zoom();
	}

	private void OnPointer(InputValue value)
	{
		Vector2 mousePos = value.Get<Vector2>();

		moveDir.x = MovePosition(mousePos.x, Screen.width); //Screen.width : ��ũ���� ���� ũ��
		moveDir.y = MovePosition(mousePos.y, Screen.height); //Screen.height : ��ũ���� ���� ũ��
	}

	private float MovePosition(float mousePoint, int screenSize)
	{
		//x���̸� ��, ��
		//y���̸� ��, �Ʒ�

		//���콺 ��ġ�� �׵θ��� ������ ���� �� �����̴� ��� -> Padding������ �޾Ƽ� padding�� ���� ���� �����̵��� ��

		if (mousePoint <= Padding) //���콺�� �����̳� ���� ���� ���� ���� ��
		{
			return -1; //�����̳� �������� �̵�
		}
		else if (mousePoint >= screenSize - Padding) //���콺�� ��ũ�� ���� (�������̳� �Ʒ���)�� ���� ��
		{
			return 1; //�������̳� �Ʒ������� �̵�
		}
		else //padding���� ������ �������� ����
		{
			return 0;
		}
	}

	private void Move()
	{
		//World�� �������� �������� �Ʒ��� ī�޶� �߶��ϴ� ���� ������ �� ���� 
		transform.Translate(Vector3.forward * moveDir.y * MoveSpeed * Time.deltaTime, Space.World); 
		transform.Translate(Vector3.right * moveDir.x * MoveSpeed * Time.deltaTime, Space.World);
	}

	private void OnZoom(InputValue value)
	{
		zoomScroll = value.Get<Vector2>().y;
	}

	private void Zoom()
	{
		transform.Translate(Vector3.forward * zoomScroll * ZoomSpeed * Time.deltaTime, Space.Self); //forward = Ȯ��
	}
}
