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
		//���� Ư. �Է� �޾� �������� �ʰ�, �׳� ��� ������ ��Ʈ�� ������
		// -> �ٷκ��� �������� �����̴ٰ� ���� ������ �ǵ��� ������ ��

		rigidbody.velocity = new Vector2(transform.right.x * moveSpeed * -1, rigidbody.velocity.y); //�ܼ��� ���� �������θ� �����̵��� ����
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
