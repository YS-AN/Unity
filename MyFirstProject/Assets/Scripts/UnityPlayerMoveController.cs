using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityPlayerMoveController : MonoBehaviour
{
	/// <summary>
	/// �����̴� ���� ����
	/// </summary>
	private Vector3 moveDir;
	private Rigidbody rb;

	[SerializeField]
	private float jumpPower;

	[SerializeField]
	private Camera mapCam;


	private void Awake()
	{
		//moveDir = this.gameObject.transform.position;
		rb = GetComponent<Rigidbody>();
	}

	private void Jump()
	{
		Debug.Log("jump");

		rb.AddForce(moveDir * jumpPower, ForceMode.Impulse); //Impulse : ���� �ѹ��� �� -> (1�ʿ� �� ���� �ѹ��� ���ϰ� �ִ� ���)
	}

	private void OnJump(InputValue value)
	{
		moveDir.y = value.Get<Vector2>().y;

		Jump(); //������ ���� ���� �۵��ϵ��� ��
				//Move�� ������ ���� �������� �ٲ����� jump�� ���� ���� �����ؾ��ؼ�, ȣ�� ��ġ�� �ٸ� ��
	}
}