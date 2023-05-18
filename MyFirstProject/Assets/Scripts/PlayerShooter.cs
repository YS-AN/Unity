using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
	/// <summary>
	/// �����̴� ���� ����
	/// </summary>
	private Animator animator;

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

	public UnityEvent OnFired; //UnityEvent�� ����ȭ�� ������



	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void MakeBullet()
	{
		Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation); //�Ѿ� ����
		GameManager.DataMgr.AddShootCount(1); //�Ѿ��� �� ������ count�� 1�� ������ -> �Ѿ��� �� ��� ������ �� �� ����

		OnFired?.Invoke(); // /* ~ */ ����Ƽ �̺�Ʈ�� ��
		/*
		animator.SetTrigger("Fire"); //�ִϸ��̼� ���� 
		//*/
	}

	/// <summary>
	/// [B] Ű�� ������ �� �Ѿ� �߻�
	/// </summary>
	/// <param name="value"></param>
	private void OnFire(InputValue value)
	{
		//Debug.Log("OnFire");

		//OnFire �� �۵��ϴ��� Ȯ��
		//Debug.Log("OnFire");

		//Instantiate(bulletPrefab); //Instantiate �������� ���� ����� ���� ������Ű�� ��

		/*
		GameObject obj = Instantiate(bulletPrefab ); //���� ������� ���� �ִ� ���� ������Ʈ ��������

		//��ġ�� ������ ��ũ�� ����
		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;
		//*/

		//��(/* ~ */) �ڵ带 �� �����ϰ� ���� -> ������ ���ÿ� ��ġ�� ȸ�� ���� ��
		//GameObject obj = Instantiate(bulletPrefab, transform.position, transform.rotation);

		//��ũ�� �߹��� ������ -> �ڼ��� ���� �Ѿ��� ��ũ�� �߹ؿ��� ������ ���� ����
		//�׷��� ��ũ �ֵ��� ��ġ���� �Ѿ��� �������� ������
		//  -> ��ũ �ֵ��� ��ġ�� �� ��ġ�� �����, �� ��ġ���� �Ѿ��� ��������, position�� rotation�� ����.
		//		-> ��ũ�� ��ġ�� ������ �����ϰ� �������� �ϱ� ������ ��ũ�� �ڽ����� ������ ������ ��
		//GameObject obj = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);

		MakeBullet();
	}

	/// <summary>
	/// �Ѿ� ���� ��ƾ (�ڷ�ƾ Ȱ���� ��Ÿ�� ����) 
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
	/// ���� : �ڷ�ƾ�� Ȱ���� N�ʿ� �ѹ� �Ѿ��� �����ϵ��� ���� -> [n]Ű ������ ����
	/// </summary>
	/// <param name="value"></param>
	private void OnRepeatFire(InputValue value)
	{
		//Debug.Log("OnRepeatFire");

		if (value.isPressed)
		{
			//Debug.Log("������");
			bulletRoution = StartCoroutine(BulletMakeRoutine());
		}
		else
		{
			//Debug.Log("����");
			StopCoroutine(bulletRoution);
		}
	}
}
