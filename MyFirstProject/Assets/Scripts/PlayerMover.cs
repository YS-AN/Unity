using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
	/// <summary>
	/// 움직이는 방향 저장
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

		rb.AddForce(moveDir * movePower, ForceMode.Force); //움직이고 싶은 방향에 힘을 실어줌
														   //AddForce : 지속적으로 가하는 힘
														   //Force : 지그시 밀기 (시간을 나눠서 지속적으로 힘을 줌)

		//this.gameObject.transform.localScale += Vector3.one * 0.0001f; 

		Camera.main.transform.LookAt(this.gameObject.transform);
	}

	private void OnMove(InputValue value)
	{
		Debug.Log("MOVE");

		//키보드 왼쪽, 오른쪽 / 위 아래
		//y축은 위 아래야.
		// 근데 우리가 움직일 떄 위 아래 움직인다고 해서 갑자기 공이 위로 뜨면 이상해.
		// 그러니까 동서남북 느낌으로 움직일 수 있또록 y를 z에 넣어줌 z가 앞뒤고, x가 좌우니까


		moveDir.z = value.Get<Vector2>().y; //보통 Z가 앞뒤임
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