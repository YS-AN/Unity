using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

//3D를 만들 때는 반드시 기준 위치를 캐릭터의 바닥에 두기!

public class PlayerMover : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private float jumpSpeed;

	private CharacterController characterController; //물리 엔진이나 Translate가 아닌 이동에 쓰이는 컨트롤러
	private Vector3 moveDir;

	private float ySpeed; //y축을 기준으로 가지고 있는 속력


	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		Move();
		Jump();
	}

	

	private void OnMove(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();
		moveDir = new Vector3(input.x, 0, input.y);
	}

	private void Move()
	{
		//Translate는 주변에 게임상황을 고려히지 않고, 정적 충돌에체는 무조건 충돌하지 않고 넘어감
		//CharacterController는 주변상황에 의해 갈 수 있는 지형과 갈 수 없는 지형을 체크하면서 다니는 이동 (대략적 물리적 지표를 활용함)  -> ex. 계단이나 언덕 정도는 오를 수 있도록 함.

		//but. CharacterController는 중력을 가지고 있지 않아. 중력에 대한 처리가 필요함 -> y위치를 캐릭터 설정에 따라 움직여 줘야해.

		//characterController.Move(moveDir * moveSpeed * Time.deltaTime); //속력 = 스피드 * Time.deltaTime(단위시간)
		//이렇게 구현하면, world기준으로 움직이기 때문에 회전한 후 이동이 어색함.
		// ex.내가 좌를 눌렀는데 우로 움직이는 현상 발생함.

		//바로보는 방향을 기준으로 움직이도록 변경함
		characterController.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime); //앞을 바로보는 방향을 기준으로 z축 방향으로 이동, 
		characterController.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime); //옆을 바로보는 방향을 기준으로 x축 방향으로 이동함. (왼쪽이면 -1곱해주고, 오른쪽은 1을 곱해주니까 자연스럽게 원하는 방향이동이 가능해짐)
	}

	//y위치를 조정해 중력이 있는 것처럼 만들어 주는 함수
	private void Jump()
	{
		//보통 물건이 떨어질때는 포물선을 그리면서 떨어짐. 
		ySpeed += Physics.gravity.y * Time.deltaTime; //Physics.gravity.y : 중력의 y축을 받음
													  //  -> project setting의 Physics 설정 값을 가져옴
													  //중력을 계속 더해 주는 등가속 운동을 하도록 함.


		/* //isGrounded가 생각보다 잘 작동이 안돼 (그닥 정교하지 않음) -> 사용 비추 
		 * //충돍체 체크나 raycast 활용해서 바닥 체크하도록!!
		if(characterController.isGrounded) //만약 땅에 닿아있으면? 
		{
			ySpeed = 0;	//y속력을 0으로 설정해서 땅 밑으로 빠지지 않도록 함.
		}
		//*/

		if(IsGround() && ySpeed < 0) //ySpeed < 0 : 이미 뛰어올라서 중력을 받아 떨어지고 있는 중에만 그라운드 체크를 하도록 함
		{
			ySpeed = 0;
		}

		characterController.Move(Vector3.up * ySpeed * Time.deltaTime); 
	}

	private void OnJump()
	{
		//점프 = 위로 속력을 갖게 한다
		ySpeed = jumpSpeed;
	}

	private bool IsGround()
	{
		//raycast가 2D와 3D가 살짝 다름
		//	3D : raycast 결과가 맞으면 true, 안 맞으면 false임.-> RaycastHit은 참조형으로 따로 나옴
		//	2D : 반환형이 RaycastHit임. -> 충돌 안 하면 null이 나옴.
		
		//3D 같은 경우 단순히 선으로 하면 애매하게 충돌이 안 먹힐 수 있으니 영역으로 확인하도록 함
		RaycastHit hit;
		return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.7f);
								//어디에서, 어느 둘레로, 어느 방향으로, hit참조형, 어느 길이로, 쏠지 세팅하기
	}
}
