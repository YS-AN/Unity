using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PracticeBullet0516 : MonoBehaviour
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

	private void Start()
	{
		rb.velocity = Vector3.forward * bulletSpeed; //�ӷ�
		Destroy(this.gameObject, 3); //3�ʾȿ� �Ѿ� ������
	}

	private void OnCollisionEnter(Collision collision)
	{
		Instantiate(explosionEffect, transform.position, transform.rotation); //�Ѿ��� ���� ��ġ, ���� ���⿡�� ȿ�� ����
		Destroy(this.gameObject); //���� �Ѿ� ������Ʈ ����
	}
}
