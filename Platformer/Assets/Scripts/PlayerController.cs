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
	private float MaxSpeed; //�÷��̾� �ְ� �ӷ�

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
		//translate�� �����Ӹ��� ��ġ�� �̵��ϴ°� �ƴ϶� 
		//���� ���������� �����ִ� ����� AddForce�� ����߱� ������ "�ӷ�"�� �ǹ��ϴ� Time.deltaTime�� �ʿ䰡 ����
		//rigidbody.AddForce(Vector2.right * inputDir * MoveSpeed, ForceMode2D.Force); //������ ������ �÷����ϱ� ������ ������ �������� �����ֵ��� ��

		//Time.deltaTime�� ������ ��带 Impulse�� �ϸ鼭, deltaTime�� �����ָ� ��!
		//Impulse�� �ѹ� Ȯ �������� ���̱� ������, �����Ӹ��� ������ �ӷ��� ���ؼ� ȮȮ �о��ֵ��� ����
		//-> �� AddForce�� �Ʒ� AddForce�� ������ �۵��� ��.
		//rigidbody.AddForce(Vector2.right * inputDir * MoveSpeed * Time.deltaTime, ForceMode2D.Impulse); 

		//���������� ���� ���ϴٺ��� ����ġ ���ϰ� �÷��̾��� �ӷ��� �ʹ� �������� �����̵��ϴ� �� ó�� ���� ���� ����. 
		// ->�ְ� �ӷ��� �Է¹޾� �ְ� �ӷ��̻����δ� �ӵ��� �ø��� ���ϵ��� ������ �R
		if (inputDir.x < 0 && rigidbody.velocity.x > -MaxSpeed) //�̹� �������� ���� �ִ� ��Ȳ���� && �ӷ��� �ְ�ӷ�(MaxSpeed)�� �������� �ʾҴٸ�? 
		{
			rigidbody.AddForce(Vector2.right * inputDir * MoveSpeed, ForceMode2D.Force); //�������� ������
		}
		else if (inputDir.x > 0 && rigidbody.velocity.x < MaxSpeed) //������ �̵� 
		{
			rigidbody.AddForce(Vector2.right * inputDir * MoveSpeed, ForceMode2D.Force);
		}

		//���� ���� ���� �ѹ��� Ȯ ���߰� �Ϸ��� ������ ���� �����ָ� ��! 
		// todo. ��ø��� �����ϱ� (������ �� �����ֱ�)

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
