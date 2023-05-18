using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
	/// <summary>
	/// �����̴� ���� ����
	/// </summary>
	private Vector3 moveDir;
	private Rigidbody rb;

	[SerializeField]
	private float movePower;

	[SerializeField]
	private float rotateSpeed;


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		Move();
		Rotate();
	}

	private void Move()
	{

		rb.AddForce(moveDir * movePower, ForceMode.Force); //�����̰� ���� ���⿡ ���� �Ǿ���
														   //AddForce : ���������� ���ϴ� ��
														   //Force : ���׽� �б� (�ð��� ������ ���������� ���� ��)

		//this.gameObject.transform.localScale += Vector3.one * 0.0001f; 

		Camera.main.transform.LookAt(this.gameObject.transform);
	}

	private void OnMove(InputValue value)
	{
		Debug.Log("MOVE");

		//Ű���� ����, ������ / �� �Ʒ�
		//y���� �� �Ʒ���.
		// �ٵ� �츮�� ������ �� �� �Ʒ� �����δٰ� �ؼ� ���ڱ� ���� ���� �߸� �̻���.
		// �׷��ϱ� �������� �������� ������ �� �ֶǷ� y�� z�� �־��� z�� �յڰ�, x�� �¿�ϱ�


		moveDir.z = value.Get<Vector2>().y; //���� Z�� �յ���
		moveDir.x = value.Get<Vector2>().x;
	}

	private void Rotate()
	{
		transform.Rotate(Vector3.up, moveDir.x * rotateSpeed * Time.deltaTime, Space.Self);
	}

	private void OnRotate(InputValue value)
	{
		moveDir.x = value.Get<Vector2>().x;
	}
}