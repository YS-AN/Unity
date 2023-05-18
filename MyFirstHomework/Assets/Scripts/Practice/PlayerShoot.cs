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
	private GameObject bulletPrefab; //������ �Ѿ��� prefab�ޱ�

	/// <summary>
	/// �Ѿ� �߻� ��ġ
	/// </summary>
	[SerializeField]
	private Transform bulletPoint;

	/// <summary>
	/// �Ѿ� �ݺ� �ð�
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
			yield return new WaitForSeconds(repeatTime); //repeatTime ���� �Ѿ� ����
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
