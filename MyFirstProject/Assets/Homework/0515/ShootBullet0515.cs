using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 다른 컴포넌트에 의존하고 있을 떄 RequireComponent하면서 꼭 필요한 컴포넌트를 알려줌
[RequireComponent(typeof(Rigidbody))]
public class ShootBullet0515 : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;

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
}
