using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MouseController : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private Transform GroundCheckedPoint;

	[SerializeField]
	private LayerMask GroundMask;

	private Rigidbody2D rigidbody;
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Move();
	}

	private void FixedUpdate()
	{
		if(IsGroundExist() == false)
		{
			Turn();
		}
		
	}

	private void Move()
	{
		//몬스터 특. 입력 받아 움직이지 않고, 그냥 계속 정해진 루트를 움직임
		// -> 바로보는 방향으로 움직이다가 끝을 만나면 되돌아 가도록 함

		rigidbody.velocity = new Vector2(transform.right.x * moveSpeed * -1, rigidbody.velocity.y); //단순히 왼쪽 방향으로만 움직이도록 설정
	}

	private void Turn()
	{
		transform.Rotate(Vector3.up, 180);
	}

	private bool IsGroundExist()
	{
		Debug.DrawRay(GroundCheckedPoint.position, Vector2.down, Color.red);

		return Physics2D.Raycast(GroundCheckedPoint.position, Vector2.down, 1f, GroundMask);
	}
}
