using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
	[SerializeField]
	private float mouseSensitivity;

	[SerializeField]
	private Transform cameraRoot;

	[SerializeField]
	private float lookDistance;

	private Vector2 lookDelta;
	private float xRotation;
	private float yRotation;

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	private void Update()
	{
		Rotate();
	}

	private void LateUpdate()
	{
		Look();
	}

	private void OnLook(InputValue value)
	{
		lookDelta = value.Get<Vector2>();
	}

	private void Look()
	{
		yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;

		xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;
		xRotation = Mathf.Clamp(xRotation, -80f, 80f); //x������ ~80�� ~ 80�������� �����̵��� ������

		//FPS�� ī�޶��� ��ġ�� �ٷκ��� ��ġ�� �����ؼ� ��� ��������, 
		//TPS�� �÷��̾ �ٶ󺸴� �Ͱ� ������� ȸ���� �ϱ� ���ؼ� localRotation�� �ƴ϶� rotation�� �ٲ�
		cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
	}

	private void Rotate()
	{
		//�÷��̾ ī�޶� �ٷκ��� �ִ� ���� ���� �ٶ󺸵��� ����
		//ī�޶� �ٷκ��� ������ lookDistance��ŭ�� �ٶ󺸵��� ������
		Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;

		//lookPoint.y = 0; //ī�޶� ���� �ִ� �������� �÷��̾ ���� ���ָ� �� �Ʒ� �� �� �÷��̾ ������ �� �Ʒ��� Ʋ�����ϱ�
		// (ex. ī�޶� ���� �ٶ󺸸� �÷��̾ ��������)
		//�¿츸 ������ �� �ֵ��� y���� 0���� ������.
		lookPoint.y = transform.position.y; //���� ��ġ�� ���� �� �ֵ��� ��
		transform.LookAt(lookPoint);
	}
}
