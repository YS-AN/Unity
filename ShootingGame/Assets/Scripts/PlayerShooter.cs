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

		//weaponHolder = GetComponentInChildren<WeaponHolder>(); //이런식으로 자식 오브젝트는 컴포넌트를 통해 찾을 수 있음 
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
		//재장전 시 손 위치가 총을 잡고 있는 위치에서 동작을 하다보니 부자연스러움
		// -> 재장신 시에는 AimRig의 weight를 0으로 만들어 동작을 투명하게 만들어 줌 -> 재장전을 처음 애니메이션 위치에서 진행하도록 함.
		//근데 애니메이션이 실행 된 후에는 다시 손이 총 위치로 가져가야하니까 코루틴을 활용해여 애니메이션 재생 시간 만큼만 멈추도록 함.

		if (isReloading)
			return;

		StartCoroutine(ReloadRoutine());
	}

	IEnumerator ReloadRoutine()
	{
		animator.SetTrigger("Reload");
		isReloading = true;
		rig.weight = 0;
		yield return new WaitForSeconds(reloadTime); //reloadTime만큼 기다렸다가
		isReloading = false; //재장전을 풀어줌
		rig.weight = 1;
	}

	
}
