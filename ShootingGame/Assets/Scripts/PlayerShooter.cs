using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
	public TrailRenderer bulletTrail;

	[SerializeField]
	private float reloadTime;

	[SerializeField]
	private Rig rig;

	[SerializeField]
	private WeaponHolder weaponHolder; 

	private Animator  animator;

	private bool isReloading;

	private void Awake()
	{
		animator = GetComponent<Animator>();

		//var startInfo = animator.GetCurrentAnimatorStateInfo();

		//weaponHolder = GetComponentInChildren<WeaponHolder>(); //�̷������� �ڽ� ������Ʈ�� ������Ʈ�� ���� ã�� �� ���� 
	}

	private void OnFire(InputValue value)
	{
		if (isReloading)
			return;

		Fire();
	}

	private void Fire()
	{
		weaponHolder.Fire();
		animator.SetTrigger("Fire");
	}


	private void OnReload(InputValue value)
	{
		//������ �� �� ��ġ�� ���� ��� �ִ� ��ġ���� ������ �ϴٺ��� ���ڿ�������
		// -> ����� �ÿ��� AimRig�� weight�� 0���� ����� ������ �����ϰ� ����� �� -> �������� ó�� �ִϸ��̼� ��ġ���� �����ϵ��� ��.
		//�ٵ� �ִϸ��̼��� ���� �� �Ŀ��� �ٽ� ���� �� ��ġ�� ���������ϴϱ� �ڷ�ƾ�� Ȱ���ؿ� �ִϸ��̼� ��� �ð� ��ŭ�� ���ߵ��� ��.

		if (isReloading)
			return;

		StartCoroutine(ReloadRoutine());
	}

	IEnumerator ReloadRoutine()
	{
		animator.SetTrigger("Reload");
		isReloading = true;
		rig.weight = 0;
		yield return new WaitForSeconds(reloadTime); //reloadTime��ŭ ��ٷȴٰ�
		isReloading = false; //�������� Ǯ����
		rig.weight = 1;
	}

	
}
