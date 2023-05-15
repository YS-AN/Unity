using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove0512 : MonoBehaviour
{
	/// <summary>
	/// �����̴� ���� ����
	/// </summary>
	private Vector3 moveDir;
	private Rigidbody rb;

	public float MoveSpeed;
	public float JumpPower;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>(); //Rigidbody ������Ʈ ã��
	}

	public void Update()
	{
		Move();
	}

	private void Move()
	{
		rb.AddForce(moveDir * MoveSpeed, ForceMode.Force);
		//Camera.main.transform.Translate(this.gameObject.position, this.gameObject);


	}

	private void Jump()
	{
		rb.AddForce(moveDir * JumpPower, ForceMode.Impulse);
	}

	private void OnMove(InputValue value)
	{
		moveDir.x = value.Get<Vector2>().x;
		moveDir.z = value.Get<Vector2>().y;
	}

	private void OnJump(InputValue vale)
	{
		moveDir.y = vale.Get<Vector2>().y;
		Jump();
	}
}
