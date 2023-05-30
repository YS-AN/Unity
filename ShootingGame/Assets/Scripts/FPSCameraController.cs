using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
	[SerializeField]
	private float mouseSensitivity; //���콺 ����

	[SerializeField]
	private Transform cameraRoot; //ī�޶� ��ġ

	[SerializeField]
	private Camera MainCamera;

	[SerializeField]
	private Camera FPSCamera;

	private Vector2 lookDelta;
	private float xRotation; //x�� ������ ȸ�� -> �¿�� ������
	private float yRotation; //y�� ������ ȸ�� -> ���Ʒ��� ������

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked; //lockState : Ŀ���� ��� ������� �����ϴ� ��
												  //CursorLockMode.Locked : ���콺 Ŀ���� ���Ϳ� ��� ��
												  //CursorLockMode.Confined : �׵θ����� �� ����� ��
												  //CursorLockMode.None : Ŀ���� �����Ӱ� 
		MainCamera.enabled = false;
		FPSCamera.enabled = true;
		
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;

		MainCamera.enabled = true;
		FPSCamera.enabled = false;
	}

	private void Update()
	{
		//Look(); 
		//ī�޶�� �÷��̾� �̵��ϰ� ���� ��ġ�� �����ؾ� ī�޶� ��Ȯ�� ��ġ�� ���� �� ����
		//	-> ���� ī�޶� ���� ȸ���ع�����, �÷��̾�� ī�޶� ���� ȸ���ϰ� �ǰ�, �÷��̾ ȸ�������� ī�޶� �ٽ� ȸ���ϰ�, ī�޶� ȸ�������� �÷��̾ ȸ���ϰ�....�� ���� �ݺ��� �Ǹ鼭 �÷��̾ ��𵵴� ������ �߻��� �� ����
	}

	private void LateUpdate()
	{
		Look(); //���� ī�޶�� LateUpdate���� �����ϵ��� ��
	}

	private void OnLook(InputValue value)
	{
		lookDelta = value.Get<Vector2>();
	}

	private void Look()
	{
		//���� ȸ���� x�� ȸ���� ���Ʒ�, y�� ȸ���� �¿���. 

		yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;
		xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;
		//x���� �������� ���� ���� �� ���̳ʽ� ȸ�� ���̱� ������ ���� ����������� ���콺 �����ӿ� ���� ���Ʒ��� �� �� ����. 

		//Clamp �ִ밪�� �Ѿ�� �ִ밪��, �ּҰ��� �Ѿ�� �ּҰ��� �ִ� �Լ�
		xRotation = Mathf.Clamp(xRotation, -80f, 80f); //x������ ~80�� ~ 80�������� �����̵��� ������

		//���Ʒ��� ���ٰ� �ؼ� �÷��̾� ������Ʈ ��ü�� �����̸� �� �� -> ī�޶� ���Ʒ��� �����̵��� ������
		cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);

		//�¿�� �׳� �÷��̾� ��ü�� �¿�� �����̵�� �� -> ���� �¿츦 �� ���� ���� Ʋ�� ���� ��찡 ������.. �׷��� ����
		transform.localRotation = Quaternion.Euler(0, yRotation, 0);
	}

}