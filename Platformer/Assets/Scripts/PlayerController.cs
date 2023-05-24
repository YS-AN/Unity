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

	//raycast가 어떤 레이어랑 닿을지 확인하기 위해서는 레이어로 구분을 해줘야해
	//그래서 필요한 게 바로 LayerMask -> LayerMask를 통해 상호작용할 레이어를 따로 받을 수 있음
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
		GroundCheck(); //물리적인 체크니까 FixedUpdate에서 확인을 함.
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

	//원래는 이러한 방법을 통해 해결하면 되는데 오늘은 새로운 걸 배우기 위해 다른 방법을 써봄..! -> GroundCheck
	/*  todo. 나 이거 적용하면 오류나..! 왜? 점프했을 때 동작이 잘 안 나와! 확인해보기!!!*********************************
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//발만 따로 충돌체로 영역을 잡고, 발 충돌체가 땅과 충돌 될 경우만 체크하도록 함. 
		// 발 충돌체는 플레이어 하위에 있음 -> trigger 충돌체에 한해서 하위에 trigger가 있고, rigidbody가 없으면, 상위 오브젝트의 rigidbody를 자연스럽게 받아 사용아 기능해짐

		isGround = false;
		animator.SetBool("IsGround", false);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		isGround = true;
		animator.SetBool("IsGround", true);
	}
	//*/

	// raycast를 활용하여 땅에 닿아있는지 확인하기
	private void GroundCheck()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer); //레이저를 아래로 1.5정도를 쏜 결과를 받아옴
																								   //  + groundLayer에서 체크를 한 레이어 오브젝트만 확인을 하도록 함
																								   //    (레이어마스크 이름으로 검색해서도 설정이 가능하긴 함)

		isGround = (hit.collider != null); //부딪힌 게 있니? -> true -> 부딪힌 게 있으면 땅에 닿은 것! 
		animator.SetBool("IsGround", isGround);

		if(isGround)
			Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, 0) - transform.position, Color.red); //실제 레이저를 그려보기 (Game화면에서는 안 나오고 Scene에서 확인 해야 해)
		else
			Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.green); //땅에 닿지 않을 경우 hit가 없어서 레이저가 이상하게 나오기 때문에 레이저 방향을 그냥 down으로 정해줌

		//부딪힌 녀석의 이름을 출력함
		if (isGround) { Debug.Log(hit.collider.gameObject.name); }

		// 레이케스트는 레이저 선만 벗어나면, 땅에서 벗어났다고 판단해버릴 수도 있음 (분명 아직 땅에 있는데...)
		// -> 그럴 때는 레이저가 아니라 영역 자체를 쏘는 방법으로 해결이 가능함. (ex. Physics2D.BoxCast)

		//RaycastAll은 전부 다 가져오는 방식임...(?)

		//cast 방색은 떨어져 있는 물체와 상호작용을 하기 위한 방법임!
	}


}
