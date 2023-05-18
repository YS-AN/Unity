using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityPlayerMoveController : MonoBehaviour
{
	/// <summary>
	/// 움직이는 방향 저장
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

		rb.AddForce(moveDir * jumpPower, ForceMode.Impulse); //Impulse : 힘을 한번에 줌 -> (1초에 줄 힘을 한번에 훅하고 주는 방식)
	}

	private void OnJump(InputValue value)
	{
		moveDir.y = value.Get<Vector2>().y;

		Jump(); //점프를 누를 때만 작동하도록 함
				//Move는 프레이 마다 움직임이 바뀌지만 jump는 누를 때만 반응해야해서, 호출 위치가 다른 것
	}
}