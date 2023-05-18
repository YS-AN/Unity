using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// 다른 컴포넌트에 의존하고 있을 떄 RequireComponent하면서 꼭 필요한 컴포넌트를 알려줌
[RequireComponent(typeof(Rigidbody))]
public class UnityBullet : MonoBehaviour
{
	[SerializeField]
	private float bulletSpeed; //총알 발사 스피드 받아옴

	[SerializeField]
	private GameObject explosionEffect; //총알이 오브젝트와 충돌할 때 보여줄 효과
	
	private Rigidbody rb;
	//private AudioSource explosionAudio;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Start()
	{
		rb.velocity = transform.forward * bulletSpeed;

		Destroy(gameObject, 5f); //5초뒤 삭제

		//explosionAudio = explosionEffect.GetComponent<AudioSource>();
		//explosionAudio.priority = 130;
	}

	/// <summary>
	/// 충돌 시 
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter(Collision collision)
	{
		/* [메모]
		 * this.gameObject(총알)에 AudioSource추가해서 터지는 사운드와 효과를 넣고, 현재 게임 오브젝트를 없애려고 하니
		   사운드 플레이와 동시에 오브젝트가 삭제되어서 사운드가 재생되지 않는 현상이 발생함. 
		
			AudioSource를 this.gameObject가 아니라 explosionEffect에 넣어서 정상적응로 재생 되도록 함.
			-> 근데 explosionEffect에 자체에 AudioSource가 있었음. 그래서 그냥 explosionEffect에 있는 거 사용하기로 함

		*/

		Instantiate(explosionEffect, transform.position, transform.rotation); //터지는 효과 넣고,

		Destroy(gameObject); //현재 게임 오브젝트(총알) 제거

	}
}
