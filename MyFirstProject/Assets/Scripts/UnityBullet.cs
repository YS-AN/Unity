using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ٸ� ������Ʈ�� �����ϰ� ���� �� RequireComponent�ϸ鼭 �� �ʿ��� ������Ʈ�� �˷���
[RequireComponent(typeof(Rigidbody))]
public class UnityBullet : MonoBehaviour
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

        Destroy(gameObject, 5f); //5�ʵ� ����
    }
}
