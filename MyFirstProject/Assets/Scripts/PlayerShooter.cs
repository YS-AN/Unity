using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
	/// <summary>
	/// 움직이는 방향 저장
	/// </summary>
	private Animator animator;

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

	public UnityEvent OnFired; //UnityEvent도 직렬화가 가능함



	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void MakeBullet()
	{
		Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation); //총알 생성
		GameManager.DataMgr.AddShootCount(1); //총알을 쏠 때마다 count가 1씩 증가함 -> 총알을 총 몇발 쐈는지 알 수 있음

		OnFired?.Invoke(); // /* ~ */ 유니티 이벤트로 뺌
		/*
		animator.SetTrigger("Fire"); //애니메이션 실행 
		//*/
	}

	/// <summary>
	/// [B] 키를 눌렀을 때 총알 발사
	/// </summary>
	/// <param name="value"></param>
	private void OnFire(InputValue value)
	{
		//Debug.Log("OnFire");

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

		MakeBullet();
	}

	/// <summary>
	/// 총알 생성 루틴 (코루틴 활용해 쿨타임 적용) 
	/// </summary>
	private Coroutine bulletRoution;

	IEnumerator BulletMakeRoutine()
	{
		while (true)
		{
			MakeBullet();
			yield return new WaitForSeconds(repeatTime);
		}
	}

	/// <summary>
	/// 연사 : 코루틴을 활용해 N초에 한번 총알을 생성하도록 변경 -> [n]키 누르면 연사
	/// </summary>
	/// <param name="value"></param>
	private void OnRepeatFire(InputValue value)
	{
		//Debug.Log("OnRepeatFire");

		if (value.isPressed)
		{
			//Debug.Log("누르다");
			bulletRoution = StartCoroutine(BulletMakeRoutine());
		}
		else
		{
			//Debug.Log("떼다");
			StopCoroutine(bulletRoution);
		}
	}
}
