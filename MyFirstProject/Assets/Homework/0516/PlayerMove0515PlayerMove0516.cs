using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove0516 : MonoBehaviour
{
	/// <summary>
	/// 움직이는 방향 저장
	/// </summary>
	private Vector3 moveDir;
	
	private Rigidbody rb;

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


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void OnMove(InputValue value)
	{
		Debug.Log($"[OnMove] ({value.Get<Vector2>().x}, {value.Get<Vector2>().y})");

		if (value.Get<Vector2>().y > 0)
		{
			transform.Translate(Vector3.forward * movePower * Time.deltaTime, Space.Self);
		}
		else if (value.Get<Vector2>().y < 0)
		{
			transform.Translate(Vector3.back * movePower * Time.deltaTime, Space.Self);
		}
	}

	private void OnRoate(InputValue value)
	{
		//todo. 방향키 좌우에 따라서 회전하도록 
	}
}
