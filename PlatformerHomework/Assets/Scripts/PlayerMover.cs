using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
	[SerializeField]
	private float MoveSpeed;

	[SerializeField]
	private float JumpPower;

	private Rigidbody2D rigidbody2D;
	private Animator animator;
	private SpriteRenderer spriteRenderer;

	private Vector3 moveDir;

	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		Move();
	}

	private void OnMove(InputValue value)
	{
		animator.SetTrigger("Move");

		moveDir = value.Get<Vector2>().x * Vector2.right;
	}

	private void Move()
	{
		if (moveDir.x != 0)
			spriteRenderer.flipX = (moveDir.x > 0);

		rigidbody2D.AddForce(moveDir * MoveSpeed, ForceMode2D.Force);
	}

	private void OnJump(InputValue value)
	{
		Jump();
	}

	private void Jump()
	{
		rigidbody2D.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
	}
}
