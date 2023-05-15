using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityTransform : MonoBehaviour
{
	/************************************************************************
	  * 트랜스폼 (Transform)
	  * 
	  * 게임오브젝트의 위치, 회전, 크기를 저장하는 컴포넌트
	  * 게임오브젝트의 부모-자식 상태를 저장하는 컴포넌트 
	  * 게임오브젝트는 반드시 하나의 트랜스폼 컴포넌트를 가지고 있으며 추가 & 제거할 수 없음
	  ************************************************************************/

	float moveSpeed = 3;
	float rotateSpeed = 90;

	private Vector3 moveDir;

	private void Start()
	{

	}

	private void Move()
	{
		TranslateMove();
	}

	// <트랜스폼 이동>
	// Translate : 트랜스폼의 이동 함수
	private void TranslateMove()
	{
		//transform.Translate //오브젝트의 위치를 이동 -> 위치 이동이기 때문에 누르는 직시 움직임
		//transform.localScale : 오브젝트 크기를 변경

		transform.position += moveDir * Time.deltaTime;
		//누른 값 만큼이 아니라 속력만큼 이동해야함. deltaTime은 단위 시간임. (한 프레임 당 걸리는 시간)
		// -> 한 플레임 당 걸리는 시간을 곱해야 속력으로 움직이는 게 가능해 짐
		//  -> A컴퓨터는 1초에 100번 업데이트가 되고, B 컴퓨터는 1초에 10번 업데이트가 된다면, 
		//	   A컴퓨터는 1번에 1/100만큼만 이동하고, B 컴퓨터는 1번에 1/10만큼 이동함.=> 공평하게 이동이 가능함.
		//		(거리 = 시간 * 속도) MoveDir = 속도 / deltaTiem = 시간

		//transform 누르자 마자 이동, 때자마자 안 움직여 (바로 바로 반응하는 움직임이 중요)
		//AddForce 누르면 가속, 때면 유지 (가속이 중요)
		//누르자마자 안 옴직이고, 가속받아 움직이면 답답함 -> 탄막슈팅 -> 총알이 나를 때렸다고 해서 가속이 느려질 필요는 없어. 내가 누른만큼 바로바로 영향을 받아 움직여야해
		//힘이 많이 중요한 게임음 AddForece가 중요함. -> 폴 가이즈. -> 물리적인 힘을 받고, 그 힘의 영향이 필요한 경우 씀. 
		//  -> 누가 나를 쳤는데 내가 누르자 마자 바로 다시 움직이면 안 돼 -> 물리적으로 가한 만큼 날라갔다가 다시 그 영향을 받고 움직여야해. 

		// 백터를 이용한 이동 
		// position에대가 냅다 거리 박는 것 보다는 transform.Translate를 사용하는 것을 더 추천함.
		transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); //방향 * 속력 * 단위 시간
																		   //Translate는 내가 바로보는 방향을 기준으로 움직이는 게 가능함.
																		   //AddForce는 세계를 기준으로 움직임. 만약 현재 object가 45도 틀어져 있어도, 세상의 축을 기준으로 움직여 버림

		// x,y,z를 이용한 이동
		transform.Translate(0, 0, moveSpeed * Time.deltaTime);
	}

	// <트랜스폼 이동 기준>
	private void TransformMoveSpace()
	{
		// 월드를 기준으로 이동
		transform.Translate(1, 0, 0, Space.World); //세상의 축을 기준으로 움직이는 것. (AdD Force와 비슷한 결과가 나와)

		// 로컬을 기준으로 이동
		transform.Translate(1, 0, 0, Space.Self); //현재 오브젝트의 축을 기준으로 움직임

		// 다른 대상을 기준으로 이동
		transform.Translate(1, 0, 0, Camera.main.transform);
	}

	// <트랜스폼 회전>
	// Rotate : 트랜스폼의 회전 함수
	private void Rotate()
	{
		//좌회전, 우회전은 y축을 기준으로 해야함. 왜 위를 기준으로 해야해? -> 축을 기준으로 시계방향과 반시계 방향이라고 생각하면 돼.
		// -> 수직을 기준으로 시계, 반시계라 좌우 회전이 y축 기준이 되는 것

		// 축을 이용한 회전 (축을 기준으로 시계방향으로 회전)
		transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime); //Vector3.up = 위쪽 방향을 기준으로 회전할 것임..!

		// 오일러를 이용한 회전
		transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

		// x,y,z를 이용한 회전
		transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
	}

	// <트랜스폼 회전 기준>
	private void RotateSpace()
	{
		// 월드를 기준으로 회전
		transform.Rotate(1, 0, 0, Space.World);
		// 로컬을 기준으로 회전
		transform.Rotate(1, 0, 0, Space.Self); //무조건 현재 오브젝트를 기준으로 좌, 우 회전을 함.
											   // 위치를 기준으로 회전
		transform.RotateAround(Camera.main.transform.position, Vector3.up, 1);
	}

	// <트랜스폼 LookAt 회전>
	// LookAt : 바라보는 방향을 기준으로 회전 -> 목적지를 설정하면, 목적지를 바라봄
	private void LookAt()
	{
		// 위치를 바라보는 회전
		transform.LookAt(new Vector3(0, 0, 0));

		// 머리의 방향을 추가한 바라보는 회전
		transform.LookAt(new Vector3(0, 0, 0), Vector3.right);
	}

	//그렇다면... 목적지 오브젝트가 순간이동을 했을 떄... LookAt은 어떡하냐?
	//rotation은 vector가 아니라 Quarternion을 씀

	// <Quarternion & Euler>
	// Quarternion	: 유니티의 게임오브젝트의 3차원 방향을 저장하고 이를 방향에서 다른 방향으로의 상대 회전으로 정의
	//				  기하학적 회전으로 짐벌락 현상이 발생하지 않음
	// EulerAngle	: 3축을 기준으로 각도법으로 회전시키는 방법
	//				  직관적이지만 짐벌락 현상이 발생하여 회전이 겹치는 축이 생길 수 있음
	// 짐벌락(Gimbal Lock) : 같은 방향으로 오브젝트의 두 회전 축이 겹치는 현상 -> 축이 겹치면 다른 각도 회전이 불가능해 짐
	//					     -> 겹친 2축의 회전 방향이 동일해지기 때문

	// Quarternion을 통해 회전각도를 계산하는 것은 직관적이지 않고 이해하기 어려움
	//   -> 그냥 오일러 각 쓰면 짐벌락 현상이 발생하기 때문에 쿼터니온을 쓴다 정도만 알면 돼

	// 보통의 경우 쿼터니언 -> 오일러각도 -> 연산진행 -> 결과오일러각도 -> 결과쿼터니언 과 같이 연산의 결과 쿼터니언을 사용함
	//   -> 결국 우리는 오일러 각도를 쓰게 되는 것.
	private void Rotation()
	{
		// 트랜스폼의 회전값은 Euler각도 표현이 아닌 Quaternion을 사용함
		transform.rotation = Quaternion.identity; //identity : 회전을 안 한 (0, 0, 0)임 

		// Euler각도를 Quaternion으로 변환
		transform.rotation = Quaternion.Euler(0, 90, 0); // 입력한 오일러를 쿼터니온으로 바꿔줌
														 //transform.rotation.ToEulerAngles(); //쿼터니온을 오일러로 바꿈
	}

	// <트랜스폼 부모-자식 상태>
	// 트랜스폼은 부모 트랜스폼을 가질 수 있음
	// 부모 트랜스폼이 있는 경우 부모 트랜스폼의 위치, 회전, 크기 변경이 같이 적용됨
	// 이를 이용하여 계층적 구조를 정의하는데 유용함 (ex. 팔이 움직이면, 손가락도 같이 움직임)
	// 하이어라키 창 상에서 드래그 & 드롭을 통해 부모-자식 상태를 변경할 수 있음
	private void TransformParent()
	{
		GameObject newGameObject = new GameObject() { name = "NewGameObject" };

		// 부모 지정
		transform.parent = newGameObject.transform;

		// 부모를 기준으로한 트랜스폼
		// transform.localPosition	: 부모트랜스폼이 있는 경우 부모를 기준으로 한 위치 -> 부모 위치를 기준으로 자식의 위치를 역계산하는 게 더 쉬움
		// transform.localRotation	: 부모트랜스폼이 있는 경우 부모를 기준으로 한 회전
		// transform.localScale		: 부모트랜스폼이 있는 경우 부모를 기준으로 한 크기

		

		// 부모 해제
		transform.parent = null;

		//부모가 없는 경우에는 local이나 world나 동일함. -> 왜냐면 부모가 없는 경우 world가 부모기 때문
		//부모 해제하고 나면 월드를 기준으로 위치가 나옴.

		// 월드를 기준으로한 트랜스폼
		// transform.localPosition == transform.position	: 부모트랜스폼이 없는 경우 월드를 기준으로 한 위치
		// transform.localRotation == transform.rotation	: 부모트랜스폼이 없는 경우 월드를 기준으로 한 회전
		// transform.localScale								: 부모트랜스폼이 없는 경우 월드를 기준으로 한 크기


	}
}