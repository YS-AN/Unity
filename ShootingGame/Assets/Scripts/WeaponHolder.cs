using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
	[SerializeField]
	private Gun gun; //�÷��̾ ��� �ִ� �� ������Ʈ 

	//WeaponHolder.transform : ���� ��� �ִ� �� ��ġ

	public void Fire()
	{
		gun.Fire();
	}

	/*
	//�Ѱ����� �����ϴٰ� �ϸ� �Ʒ��� ���� ���� ����Ʈ�� �޾� ������ �����ϴ�

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
