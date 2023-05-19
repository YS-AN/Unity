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
		//AddForce는 클릭할 때마다가 힘이 가산됨. but, 게임 동작 상 클릭할 때마다 일정한 높이로 올라가야해
		//velocity를 쓰면 어떤 속도를 가지고 있었던지 상관하지 않고, 일정한 속도로 올려줌

		//rigidbody.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
		rigidbody.velocity = Vector2.up * JumpPower;

		OnJumped?.Invoke();
	}

	private void Rotate()
	{
		//새가 아래로 떨어지면 밑을 보고, 위로 올라가면 위를 볼 수 있도록 회전 동작을 추가함.
		transform.right = rigidbody.velocity + Vector2.right * MoveSpeed; // 위아래 속력(velocity) + 진행방향(right) * 진행속도(MoveSpeed);
																		  //   -> 위 + 진행 방향 * 속도 = 그 사이 대각선을 바라보게 됨 => 자연스럽게 회전하는 느낌이 나옴
																		  //	-> 위 + 진행방향 = 새가 완전 위를 봄
																		  //	-> 아래 + 진행방향 = 새가 완전 밑을 봄
																		  //	-> 속도 = 속도에 따라 대각산 각도가 정해짐
																		  //	   (숫자가 낮을 수록 더 확확 꺽어짐) 0이면 완전히 위나 아래를 봄. (90도 회전해 벌임)
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
