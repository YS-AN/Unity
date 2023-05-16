using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PracticeBullet0516 : MonoBehaviour
{
	[SerializeField]
	private float bulletSpeed; //총알 발사 스피드 받아옴

	[SerializeField]
	private GameObject explosionEffect; //총알이 오브젝트와 충돌할 때 보여줄 효과

	private Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		rb.velocity = Vector3.forward * bulletSpeed; //속력
		Destroy(this.gameObject, 3); //3초안에 총알 제거함
	}

	private void OnCollisionEnter(Collision collision)
	{
		Instantiate(explosionEffect, transform.position, transform.rotation); //총알의 현재 위치, 현재 방향에서 효과 생성
		Destroy(this.gameObject); //현재 총알 오브젝트 제거
	}
}
