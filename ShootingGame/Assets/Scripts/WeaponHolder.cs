using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
	[SerializeField]
	private Gun gun; //플레이어가 들고 있는 총 오브젝트 

	//WeaponHolder.transform : 총을 들고 있는 손 위치

	public void Fire()
	{
		gun.Fire();
	}

	/*
	//총게임을 구현하다고 하면 아래와 같이 총을 리스트로 받아 구현이 가능하다

	List<Gun> gunList = new List<Gun>();

	public Gun Swap(int i)
	{
		return gunList[i];
	}

	public void GetGun(Gun newGun)
	{
		gunList.Add(newGun);
	}
	//*/
}
