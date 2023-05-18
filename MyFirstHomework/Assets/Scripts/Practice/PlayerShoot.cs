using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
	public UnityEvent OnShootBullet;

	private Coroutine bulletMakeRoution;

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


	public void ShootBullet()
	{
		OnShootBullet?.Invoke();

		var newBullet = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
		DataManager.BulletManager.AddShootCount(1);
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
