using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityPlayerMoveController : MonoBehaviour
{
	/// <summary>
	/// 움직이는 방향 저장
	/// </summary>
	private Vector3 moveDir;
	private Rigidbody rb;

	[SerializeField]
	private float movePower;

	[SerializeField]
	private float jumpPower;

	[SerializeField]
	private float rotateSpeed;

	[Header("Shooter")]
	[SerializeField]
	private GameObject bulletPrefab; //복사할 총알의 prefab받기

	/// <summary>
	/// 총알 발사 위치
	/// </summary>
	[SerializeField]
	private Transform bulletPoint;

	/// <summary>
	/// 총알 반복 시간
	/// </summary>
	[SerializeField]
	private float repeatTime;

	private void Awake()
	{
		//moveDir = this.gameObject.transform.position;
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		rb.AddForce(moveDir * movePower, ForceMode.Force); //움직이고 싶은 방향에 힘을 실어줌
														   //AddForce : 지속적으로 가하는 힘
														   //Force : 지그시 밀기 (시간을 나눠서 지속적으로 힘을 줌)

		this.gameObject.transform.localScale += Vector3.one * 0.0001f;

		Camera.main.transform.LookAt(this.gameObject.transform);
	}

	private void Jump()
	{
		Debug.Log("jump");

		rb.AddForce(moveDir * jumpPower, ForceMode.Impulse); //Impulse : 힘을 한번에 줌 -> (1초에 줄 힘을 한번에 훅하고 주는 방식)
	}


	private void OnMove(InputValue value)
	{
		//키보드 왼쪽, 오른쪽 / 위 아래
		//y축은 위 아래야.
		// 근데 우리가 움직일 떄 위 아래 움직인다고 해서 갑자기 공이 위로 뜨면 이상해.
		// 그러니까 동서남북 느낌으로 움직일 수 있또록 y를 z에 넣어줌 z가 앞뒤고, x가 좌우니까

		moveDir.x = value.Get<Vector2>().x;
		moveDir.z = value.Get<Vector2>().y; //보통 Z가 앞뒤임
	}

	private void OnRotate(InputValue value)
	{
		Debug.Log($"{value.Get<Vector2>().x}, {value.Get<Vector2>().y}");
		transform.Rotate(0, value.Get<Vector2>().y * rotateSpeed * Time.deltaTime, 0);
	}

	private void OnJump(InputValue value)
	{
		moveDir.y = value.Get<Vector2>().y;



		Jump(); //점프를 누를 때만 작동하도록 함
				//Move는 프레이 마다 움직임이 바뀌지만 jump는 누를 때만 반응해야해서, 호출 위치가 다른 것
	}

	/// <summary>
	/// 스페이스바 눌렀을 때 총알 발사
	/// </summary>
	/// <param name="value"></param>
	private void OnFire(InputValue value)
	{
		//OnFire 잘 작동하는지 확인
		//Debug.Log("OnFire");

		//Instantiate(bulletPrefab); //Instantiate 프리팹을 새로 만들어 씬에 안착시키는 것

		/*
		GameObject obj = Instantiate(bulletPrefab ); //새로 만들어진 씬에 있는 게임 오브젝트 프리팹임

		//위치랑 방향을 탱크와 맞춤
		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;
		//*/

		//위(/* ~ */) 코드를 더 간단하게 만듦 -> 생성과 동시에 위치와 회전 값을 줌
		//GameObject obj = Instantiate(bulletPrefab, transform.position, transform.rotation);

		//탱크는 발밑이 기준임 -> 자세히 보면 총알이 탱크의 발밑에서 나가는 것이 보임
		//그래서 탱크 주둥이 위치에서 총알이 나오도록 변경함
		//  -> 탱크 주둥이 위치를 빈 위치로 만들고, 그 위치에서 총알이 나오도록, position과 rotation을 맞춤.
		//		-> 탱크와 위치나 방향이 동일하게 움직여야 하기 때문에 탱크의 자식으로 들어가도록 설정한 것
		//GameObject obj = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);

		Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
	}

	/// <summary>
	/// 총알 생성 루틴 (코루틴 활용해 쿨타임 적용)
	/// </summary>
	private Coroutine bulletRoution;

	IEnumerator BulletMakeRoutine()
	{
		while (true)
		{
			Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
			yield return new WaitForSeconds(repeatTime);
		}
	}

	/// <summary>
	/// 연사 : 코루틴을 활용해 N초에 한번 총알을 생성하도록 변경
	/// </summary>
	/// <param name="value"></param>
	private void OnRepeatFire(InputValue value)
	{
		if (value.isPressed)
		{
			Debug.Log("누르다");
			bulletRoution = StartCoroutine(BulletMakeRoutine());
		}
		else
		{
			Debug.Log("떼다");
			StopCoroutine(bulletRoution);
		}
	}

	
}