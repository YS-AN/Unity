using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
	/// <summary>
	/// 움직이는 방향 저장
	/// </summary>
	private Vector3 moveDir;
	private Vector3 rotateDir;

	private Rigidbody rb;

	/// <summary>
	/// 움직이는 속도
	/// </summary>
	[SerializeField]
	private float movePower;

	/// <summary>
	/// 회전 속도
	/// </summary>
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
		transform.Translate(moveDir * movePower * Time.deltaTime, Space.Self);
	}

	private void OnMove(InputValue value)
	{
		moveDir.z = value.Get<Vector2>().y;
	}

	private void Rotate() //float x
	{
		this.gameObject.transform.Rotate(Vector3.up, rotateDir.x * rotateSpeed * Time.deltaTime, Space.Self);
	}

	private void OnRotate(InputValue value)
	{
		rotateDir.x = value.Get<Vector2>().x;
	}
}
