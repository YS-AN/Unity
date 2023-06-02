using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IHittable
{
	private Rigidbody rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Hit(RaycastHit hit, int damage)
	{
		//Destroy(gameObject); //������ ���� ���� ������Ʈ�� ������

		rigidbody?.AddForceAtPosition(-10 * hit.normal, hit.point, ForceMode.Impulse);//AddForceAtPosition : ������ ������ ���� ���ߴ�!
	}
}
