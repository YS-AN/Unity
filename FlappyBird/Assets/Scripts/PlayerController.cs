using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rigidbody;

	[SerializeField]
	private float JumpPower;

	[SerializeField]
	private float MoveSpeed;

	public UnityEvent OnJumped;
	public UnityEvent OnScore;

	public UnityEvent OnDied;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Rotate();
	}

	private void OnJump(InputValue value)
	{
		if(value.isPressed)
		{
			Jump();
		}
	}

	private void Jump()
	{
		//AddForce�� Ŭ���� �����ٰ� ���� �����. but, ���� ���� �� Ŭ���� ������ ������ ���̷� �ö󰡾���
		//velocity�� ���� � �ӵ��� ������ �־����� ������� �ʰ�, ������ �ӵ��� �÷���

		//rigidbody.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
		rigidbody.velocity = Vector2.up * JumpPower;

		OnJumped?.Invoke();
	}

	private void Rotate()
	{
		//���� �Ʒ��� �������� ���� ����, ���� �ö󰡸� ���� �� �� �ֵ��� ȸ�� ������ �߰���.
		transform.right = rigidbody.velocity + Vector2.right * MoveSpeed; // ���Ʒ� �ӷ�(velocity) + �������(right) * ����ӵ�(MoveSpeed);
																		  //   -> �� + ���� ���� * �ӵ� = �� ���� �밢���� �ٶ󺸰� �� => �ڿ������� ȸ���ϴ� ������ ����
																		  //	-> �� + ������� = ���� ���� ���� ��
																		  //	-> �Ʒ� + ������� = ���� ���� ���� ��
																		  //	-> �ӵ� = �ӵ��� ���� �밢�� ������ ������
																		  //	   (���ڰ� ���� ���� �� ȮȮ ������) 0�̸� ������ ���� �Ʒ��� ��. (90�� ȸ���� ����)
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		OnDied?.Invoke();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "ScoreArea")
		{
			GameManager.DataInstance.CurrentScore += 1;
			OnScore?.Invoke();
		}
	}

	public void InitPlayerPosition()
	{
		this.gameObject.transform.position = new Vector3(-1, 0, 0);
	}
}
