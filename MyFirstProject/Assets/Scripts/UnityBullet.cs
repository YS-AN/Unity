using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 다른 컴포넌트에 의존하고 있을 떄 RequireComponent하면서 꼭 필요한 컴포넌트를 알려줌
[RequireComponent(typeof(Rigidbody))]
public class UnityBullet : MonoBehaviour
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

    void Start()
    {
        rb.velocity = transform.forward * bulletSpeed;

        Destroy(gameObject, 5f); //5초뒤 삭제
    }

    /// <summary>
    /// 충돌 시 
    /// </summary>
    /// <param name="collision"></param>
	private void OnCollisionEnter(Collision collision)
	{
        Instantiate(explosionEffect, transform.position, transform.rotation); //터지는 효과 넣고,
        Destroy(gameObject); //현재 게임 오브젝트(총알)을 없앰
	}
}
