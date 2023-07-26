using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;

	private Player player;

	private Rigidbody rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = transform.forward * moveSpeed;
		Destroy(gameObject, 3f);
	}

	public void SetPlayer(Player player)
	{
		this.player = player;
	}

	/// <summary>
	/// ���ɸ� �ð� �����ؼ� �Ÿ� ����
	/// </summary>
	public void ApplyLag(float lag)
	{
		transform.position += rigidbody.velocity * lag; //�ӵ� * �ð� = �Ÿ� (���� ��ġ) -> ���ɸ� �ð���ŭ ���� ��ġ�� �̵���Ŵ 
	}

	public void GetSocre()
	{
		//player���� ���� �߰�
	}

}
