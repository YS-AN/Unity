using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityInput : MonoBehaviour
{
	/************************************************************************
	 * ����Ƽ �Է�
	 * 
	 * ����Ƽ���� ������� ����� ������ �� �ִ� ����
	 * ����ڴ� �ܺ� ��ġ�� �̿��Ͽ� ������ ������ �� ����
	 * ����Ƽ�� �پ��� Ÿ���� �Է±��(Ű���� �� ���콺, ���̽�ƽ, ��ġ��ũ�� ��)�� ���������� ��Ƽ �÷��� ���� ������
	 ************************************************************************/

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		//InputByDevice();
		//InputByInputManager();

	}

	// ��ġ ��� �Է� -> Input
	// Ư���� ��ġ�� �������� �Է� ����
	// Ư���� ��ġ�� �Է��� �����ϱ� ������ ���� �÷����� ������ ����� (��Ƽ �÷��� ȯ�濡���� ������)
	void InputByDevice()
	{
		//Ű���� ���� �Է�
		if (Input.GetKeyUp(KeyCode.Space)) //Ű���� ���� ���� ����
		{
			Debug.Log("KEY UP");
		}
		if (Input.GetKeyDown(KeyCode.Space)) //Ű ���� ����
		{
			Debug.Log("KEY DOWN");
		}
		if (Input.GetKey(KeyCode.Space)) //Ű ������ ��
		{
			Debug.Log("KEY PRESSING");
		}

		//���콺 ���� �Է�
		// ���� = 0 / ������ = 1
		if (Input.GetMouseButton(0))
		{
			Debug.Log("KEY PRESSING");
		}
		if (Input.GetMouseButtonUp(0))
		{
			Debug.Log("KEY UP");
		}
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("KEY DOWN");
		}
	}

	// �ൿ�� ���� �Է� -> InputManager
	// ���� ��ġ�� �Է��� �Է¸Ŵ����� "�̸��� �Է�"�� ����
	// �Է¸Ŵ����� �̸����� ������ �Է��� ��������� Ȯ��
	//  (����Ƽ �������� Edit -> Project Settings -> Input Manager ���� ����)
	//
	// ��, ����Ƽ ��â���� ����̱� ������ Ű����, ���콺, ���̽�ƽ�� ���� ��ġ���� �����
	// �߰�) VR Oculus Integraion Kit ���� ��� �Է¸Ŵ����� ������ ����� ���
	private void InputByInputManager()
	{
		// ��ư �Է�
		// Fire1 : Ű����(Left Ctrl), ���콺(Left Button), ���̽�ƽ(button0)���� ���� ��.
		// ��ư : ������ �ܹ߼� ������.
		if (Input.GetButton("Fire1"))
			Debug.Log("Fire1 is pressing");
		if (Input.GetButtonDown("Fire1"))
			Debug.Log("Fire1 is down");
		if (Input.GetButtonUp("Fire1"))
			Debug.Log("Fire1 is up");

		// �� �Է�
		// Horizontal(����) : Ű����(a,d / ��, ��), ���̽�ƽ(���� �Ƴ��α׽�ƽ �¿�)
		float x = Input.GetAxis("Horizontal"); //�󸶸�ŭ ���������� �޾ƿ;� �ϱ� ������ ��ȯ ���� float��
											   // Vertical(����) : Ű����(w,s / ��, ��), ���̽�ƽ(���� �Ƴ��α׽�ƽ ����)
		float y = Input.GetAxis("Vertical");

		Debug.Log(x + " " + y);
	}

	//VR�� �Է� �Ŵ����� ����. �ֳĸ� inputManager���� VR�� �� �ʰ� ���Ա� ��������!
	//���� ������ �Է� �Ŵ����� ����Ͽ� ���� ������ ������ �����̱� ��. 
	// �츮�� InputSystem �����.
	// �ٵ� ��� asset store���� Oculus InputManager �ٿ� �޾Ƽ� ���� ������ �ѵ�.... �� �̻� ������ �� ���شٰ� ��ǥ�ؼ�... �׳� Input system�� ����...

	// <InputSystem>
	// Unity 2019.1 ���� �����ϰ� �� �Է¹�� -> ����Ƽ������ ������ input system�� Ȱ���ϰڴٰ� ��ǥ��. (�ٵ� ���� ȸ����� inputManager�� �ַ� �̿�����)
	// ������Ʈ�� ���� �Է��� ��������� Ȯ��
	// GamePad, JoyStick, Mouse, Keyboard, Pointer, Pen, TouchScreen, XR ��� �� ���� ������ �����ϰ� ����
	private void InputByInputSystem()
	{
		// InputSystem�� �ڵ�� �������� �ʰ�, "�̺�Ʈ ���"���� ������ (���� input���� ���� ����� �ٸ�!)
		// Update���� �Էº�������� Ȯ���ϴ� ��� ��� ������ ���� ��� �̺�Ʈ�� Ȯ��
		// �޽����� ���� �޴� ��İ� �̺�Ʈ �Լ��� ���� �����ϴ� ��� ������ ����
	}

	// Jump �Է¿� �����ϴ� OnJump �޽��� �Լ� (���� imput system�� jump�� ���� �����߱� ������ ����� ������ ��)
	/*
	private void OnJump(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();
		Debug.Log("ON Jump");
	}
	//*/
}