using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ٸ� ������Ʈ�� �����ϰ� ���� �� RequireComponent�ϸ鼭 �� �ʿ��� ������Ʈ�� �˷���
[RequireComponent(typeof(Rigidbody))]
public class UnityBullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed; //�Ѿ� �߻� ���ǵ� �޾ƿ�

    [SerializeField]
    private GameObject explosionEffect; //�Ѿ��� ������Ʈ�� �浹�� �� ������ ȿ��

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

    /// <summary>
    /// �浹 �� 
    /// </summary>
    /// <param name="collision"></param>
	private void OnCollisionEnter(Collision collision)
	{
        Instantiate(explosionEffect, transform.position, transform.rotation); //������ ȿ�� �ְ�,
        Destroy(gameObject); //���� ���� ������Ʈ(�Ѿ�)�� ����
	}
}
