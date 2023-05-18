using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
	/// <summary>
	/// 움직이는 방향 저장
	/// </summary>
	private Vector3 moveDir;
	private Vector3 rotateDir;

	private Rigidbody rb;
	private AudioSource firingAudio;
	private Animator animator;

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
		firingAudio.priority = 120;

		animator = GetComponent<Animator>();
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
	}

	private void OnMove(InputValue value)
	{
		moveDir.z = value.Get<Vector2>().y;
	}

	private void Rotate() //float x
	{
		this.gameObject.transform.Rotate(Vector3.up, rotateDir.x * rotateSpeed * Time.deltaTime, Space.Self);
	}

	private void OnRotate(InputValue value)
	{
		rotateDir.x = value.Get<Vector2>().x;
	}

	public void ShootBullet()
	{
		animator.SetTrigger("Shoot");
		firingAudio.Play();
		Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
	}

	IEnumerator MakeBullets()
	{
		while (true)
		{
			ShootBullet();
			yield return new WaitForSeconds(repeatTime); //repeatTime 쉬고 총알 생성
		}
	}

	private void OnFire(InputValue value)
	{
		ShootBullet();
	}

	private void OnRepeatFire(InputValue value)
	{
		if (value.isPressed)
		{
			bulletMakeRoution = StartCoroutine(MakeBullets());
		}
		else
		{
			StopCoroutine(bulletMakeRoution);
		}
	}

}
