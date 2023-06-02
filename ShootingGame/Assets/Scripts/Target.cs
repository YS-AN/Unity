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
		//Destroy(gameObject); //맞으면 현재 게임 오브젝트를 삭제함

		rigidbody?.AddForceAtPosition(-10 * hit.normal, hit.point, ForceMode.Impulse);//AddForceAtPosition : 지정한 곳에서 힘을 가했다!
	}
}
