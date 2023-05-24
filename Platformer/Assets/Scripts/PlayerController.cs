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

	//raycast�� � ���̾�� ������ Ȯ���ϱ� ���ؼ��� ���̾�� ������ �������
	//�׷��� �ʿ��� �� �ٷ� LayerMask -> LayerMask�� ���� ��ȣ�ۿ��� ���̾ ���� ���� �� ����
	[SerializeField]
	private LayerMask groundLayer; 

	private bool isGround;

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

	private void FixedUpdate()
	{
		GroundCheck(); //�������� üũ�ϱ� FixedUpdate���� Ȯ���� ��.
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
		if(isGround)
		{
			Jump();
		}
	}

	private void Jump()
	{
		//animator.SetTrigger("Jump");
		rigidbody.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
	}

	//������ �̷��� ����� ���� �ذ��ϸ� �Ǵµ� ������ ���ο� �� ���� ���� �ٸ� ����� �ẽ..! -> GroundCheck
	/*  todo. �� �̰� �����ϸ� ������..! ��? �������� �� ������ �� �� ����! Ȯ���غ���!!!*********************************
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//�߸� ���� �浹ü�� ������ ���, �� �浹ü�� ���� �浹 �� ��츸 üũ�ϵ��� ��. 
		// �� �浹ü�� �÷��̾� ������ ���� -> trigger �浹ü�� ���ؼ� ������ trigger�� �ְ�, rigidbody�� ������, ���� ������Ʈ�� rigidbody�� �ڿ������� �޾� ���� �������

		isGround = false;
		animator.SetBool("IsGround", false);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		isGround = true;
		animator.SetBool("IsGround", true);
	}
	//*/

	// raycast�� Ȱ���Ͽ� ���� ����ִ��� Ȯ���ϱ�
	private void GroundCheck()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer); //�������� �Ʒ��� 1.5������ �� ����� �޾ƿ�
																								   //  + groundLayer���� üũ�� �� ���̾� ������Ʈ�� Ȯ���� �ϵ��� ��
																								   //    (���̾��ũ �̸����� �˻��ؼ��� ������ �����ϱ� ��)

		isGround = (hit.collider != null); //�ε��� �� �ִ�? -> true -> �ε��� �� ������ ���� ���� ��! 
		animator.SetBool("IsGround", isGround);

		if(isGround)
			Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, 0) - transform.position, Color.red); //���� �������� �׷����� (Gameȭ�鿡���� �� ������ Scene���� Ȯ�� �ؾ� ��)
		else
			Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.green); //���� ���� ���� ��� hit�� ��� �������� �̻��ϰ� ������ ������ ������ ������ �׳� down���� ������

		//�ε��� �༮�� �̸��� �����
		if (isGround) { Debug.Log(hit.collider.gameObject.name); }

		// �����ɽ�Ʈ�� ������ ���� �����, ������ ����ٰ� �Ǵ��ع��� ���� ���� (�и� ���� ���� �ִµ�...)
		// -> �׷� ���� �������� �ƴ϶� ���� ��ü�� ��� ������� �ذ��� ������. (ex. Physics2D.BoxCast)

		//RaycastAll�� ���� �� �������� �����...(?)

		//cast ����� ������ �ִ� ��ü�� ��ȣ�ۿ��� �ϱ� ���� �����!
	}


}
