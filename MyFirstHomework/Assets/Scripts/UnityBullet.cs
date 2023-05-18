using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// �ٸ� ������Ʈ�� �����ϰ� ���� �� RequireComponent�ϸ鼭 �� �ʿ��� ������Ʈ�� �˷���
[RequireComponent(typeof(Rigidbody))]
public class UnityBullet : MonoBehaviour
{
	[SerializeField]
	private float bulletSpeed; //�Ѿ� �߻� ���ǵ� �޾ƿ�

	[SerializeField]
	private GameObject explosionEffect; //�Ѿ��� ������Ʈ�� �浹�� �� ������ ȿ��
	
	private Rigidbody rb;
	//private AudioSource explosionAudio;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Start()
	{
		rb.velocity = transform.forward * bulletSpeed;

		Destroy(gameObject, 5f); //5�ʵ� ����

		//explosionAudio = explosionEffect.GetComponent<AudioSource>();
		//explosionAudio.priority = 130;
	}

	/// <summary>
	/// �浹 �� 
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter(Collision collision)
	{
		/* [�޸�]
		 * this.gameObject(�Ѿ�)�� AudioSource�߰��ؼ� ������ ����� ȿ���� �ְ�, ���� ���� ������Ʈ�� ���ַ��� �ϴ�
		   ���� �÷��̿� ���ÿ� ������Ʈ�� �����Ǿ ���尡 ������� �ʴ� ������ �߻���. 
		
			AudioSource�� this.gameObject�� �ƴ϶� explosionEffect�� �־ ���������� ��� �ǵ��� ��.
			-> �ٵ� explosionEffect�� ��ü�� AudioSource�� �־���. �׷��� �׳� explosionEffect�� �ִ� �� ����ϱ�� ��

		*/

		Instantiate(explosionEffect, transform.position, transform.rotation); //������ ȿ�� �ְ�,

		Destroy(gameObject); //���� ���� ������Ʈ(�Ѿ�) ����

	}
}
