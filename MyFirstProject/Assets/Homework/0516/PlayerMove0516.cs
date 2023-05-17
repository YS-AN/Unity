using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove0516 : MonoBehaviour
{
	/// <summary>
	/// 움직이는 방향 저장
	/// </summary>
	private Vector3 moveDir;
	private Vector3 rotateDir;

	private Rigidbody rb;
	private AudioSource firingAudio;

	private Coroutine bulletMakeRoution;

	/// <summary>
	/// 움직이는 속도
	/// </summary>
	[SerializeField]
	private float movePower;

	/// <summary>
	/// 회전 속도
	/// </summary>
	[SerializeField]
	private float rotateSpeed;

	[SerializeField]
	private Camera MapCam;

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

	/// <summary>
	/// 총알 발사 효과
	/// </summary>
	[SerializeField]
	private AudioClip bulletSound;


	

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();

		firingAudio = this.gameObject.AddComponent<AudioSource>();
		firingAudio.clip = bulletSound;
	}

	private void Start()
	{
	}

	private void Update()
	{
		Move();
		Rotate();
	}

	private void Move()
	{
		transform.Translate(moveDir * movePower * Time.deltaTime, Space.Self);

		MapCam.transform.Translate(moveDir * movePower * Time.deltaTime, this.transform);
	}

	private void OnMove(InputValue value)
	{
		//Debug.Log($"[OnMove] ({value.Get<Vector2>().x}, {value.Get<Vector2>().y})");

		moveDir.z = value.Get<Vector2>().y;

		//update 없이 OnMove만 하니까 찔끔 찔끔 움직일 때마다 방향키를 눌러줘야함 -> OnMove에서는 이동 방향만 받고, Update에서 프레임마다 이동하도록 수정함
		//if (value.Get<Vector2>().y > 0)
		//{
		//	transform.Translate(Vector3.forward * movePower * Time.deltaTime, Space.Self);
		//}
		//else if (value.Get<Vector2>().y < 0)
		//{
		//	transform.Translate(Vector3.back * movePower * Time.deltaTime, Space.Self);
		//}
	}

	private void Rotate() //float x
	{
		this.gameObject.transform.Rotate(Vector3.up * rotateSpeed* Time.deltaTime * rotateDir.x, Space.Self);
	}

	private void OnRotate(InputValue value)
	{
		rotateDir.x = value.Get<Vector2>().x;
		//Rotate(value.Get<Vector2>().x);
		/*
		if (value.isPressed)
		{
			Debug.Log("[OnRotate] isPress");
			//Rotate(value.Get<Vector2>().x);
		}
		*/
	}

	private void MakeBullet()
	{
		firingAudio.Play();
		Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
	}

	IEnumerator MakeBullets()
	{
		while(true)
		{
			MakeBullet();
			yield return new WaitForSeconds(repeatTime); //repeatTime 쉬고 총알 생성
		}
	}

	private void OnFire(InputValue value)
	{
		MakeBullet();
	}

	private void OnRepeatFire(InputValue value)
	{
		if(value.isPressed)
		{
			bulletMakeRoution = StartCoroutine(MakeBullets());
		}
		else
		{
			StopCoroutine(bulletMakeRoution);
		}
	}
}
