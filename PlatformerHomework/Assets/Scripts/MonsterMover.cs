using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class MonsterMover : MonoBehaviour
{
	[SerializeField]
	private float MoveSpeed;

	[SerializeField]
	private LayerMask GroundLayerMask;

	private Rigidbody2D rigidbody;

	private bool isMoveRight = false;


	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Move();
	}

	private void FixedUpdate()
	{
		if(IsTouchGround())
		{
			Turn();
		}
	}

	private void Move()
	{
		rigidbody.velocity = new Vector2(transform.right.x * MoveSpeed * -1, rigidbody.velocity.y);
	}

	private void Turn()
	{
		transform.Rotate(Vector3.up, 180);

		isMoveRight = isMoveRight ? false : true;
	}

	private bool IsTouchGround()
	{
		float rayLen = 0.5f;
		float direction = isMoveRight ? 1 : -1;

		Debug.DrawRay(transform.position, Vector2.right * direction * rayLen, Color.red);
		return Physics2D.Raycast(transform.position, Vector2.right * direction, rayLen, GroundLayerMask);
	}
}
