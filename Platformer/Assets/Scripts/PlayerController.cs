using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float MoveSpeed;

	[SerializeField]
	private float MaxSpeed; //플레이어 최고 속력

	[SerializeField]
	private float JumpPower;

	private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private Vector2 inputDir;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		Move();
	}

	private void OnMove(InputValue value)
	{
		inputDir = value.Get<Vector2>();
	}

	private void Move()
	{
		//translate는 프레임마다 위치를 이동하는게 아니라 
		//힘을 지속적으로 가해주는 방식의 AddForce를 사용했기 때문에 "속력"을 의미하는 Time.deltaTime는 필요가 없음
		//rigidbody.AddForce(Vector2.right * inputDir * MoveSpeed, ForceMode2D.Force); //오른쪽 방향이 플러스니까 오른쪽 방향을 기준으로 곱해주도록 함

		//Time.deltaTime을 쓰려면 모드를 Impulse로 하면서, deltaTime을 곱해주면 돼!
		//Impulse는 한번 확 가해지는 힘이기 때문에, 프레임마다 동일한 속력을 가해서 확확 밀어주도록 만듦
		//-> 위 AddForce랑 아래 AddForce랑 동일한 작동을 함.
		//rigidbody.AddForce(Vector2.right * inputDir * MoveSpeed * Time.deltaTime, ForceMode2D.Impulse); 

		//지속적으로 힘을 가하다보면 예상치 못하게 플레이어의 속력이 너무 빨라져서 순간이동하는 것 처럼 보일 수도 있음. 
		// ->최고 속력을 입력받아 최고 속력이상으로는 속도를 올리지 못하도록 제한을 둚
		if (inputDir.x < 0 && rigidbody.velocity.x > -MaxSpeed) //이미 왼쪽으로 가고 있는 상황에서 && 속력이 최고속력(MaxSpeed)에 도달하지 않았다면? 
		{
			rigidbody.AddForce(Vector2.right * inputDir * MoveSpeed, ForceMode2D.Force); //움직임을 가해줌
		}
		else if (inputDir.x > 0 && rigidbody.velocity.x < MaxSpeed) //오른쪽 이동 
		{
			rigidbody.AddForce(Vector2.right * inputDir * MoveSpeed, ForceMode2D.Force);
		}

		//손을 떼는 순간 한번에 확 멈추게 하려면 역방향 힘을 가해주면 돼! 
		// todo. 즉시멈춤 구현하기 (역방행 힘 가해주기)

		animator.SetFloat("MoveSpeed", inputDir.magnitude);

		if(inputDir.x != 0)
		{
			spriteRenderer.flipX = (inputDir.x < 0);
		}

	}

	private void OnJump(InputValue value)
	{
		Jump();
	}

	private void Jump()
	{
		animator.SetTrigger("Jump");
		rigidbody.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		animator.SetBool("IsGround", true);
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		animator.SetBool("IsGround", false);
	}
}
