using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private float jumpSpeed;

	private CharacterController characterController;
	private Animator animator;

	private Vector3 moveDir;
	private float ySpeed; 


	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		Move();
		Jump();
	}

	private void OnMove(InputValue value)
	{
		var input = value.Get<Vector2>();
		moveDir = new Vector3(input.x, 0, input.y);

		animator.SetFloat("XSpeed", input.x);
		animator.SetFloat("YSpeed", input.y);
	}

	private void Move()
	{
		characterController.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
		characterController.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
	}

	private void OnJump(InputValue value)
	{
		ySpeed = jumpSpeed;
	}

	private void Jump()
	{
		ySpeed += Physics.gravity.y * Time.deltaTime;

		if (IsGround() && ySpeed < 0)
		{
			ySpeed = 0;
		}
		characterController.Move(Vector3.up * ySpeed * Time.deltaTime);
	}

	private bool IsGround()
	{
		RaycastHit hit;
		return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.7f);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}
}
