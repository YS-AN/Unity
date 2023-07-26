using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviourPun
{
	[SerializeField] 
	private float moveSpeed;

	private Rigidbody rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();

		if(photonView.InstantiationData != null)
		{
			Vector3 force = (Vector3)photonView.InstantiationData[0];
			Vector3 torque = (Vector3)photonView.InstantiationData[1];

			rigidbody.AddForce(force, ForceMode.Impulse);
			rigidbody.AddForce(torque, ForceMode.Impulse);
		}
	}

	private void Update()
	{
		//�����ְ� �ƴϸ� ����Ʈ ���� ���ϵ��� �� -> ���� ���常 �׷� -> ���� ������ ���常 �����ϵ��� ��. 
		if(!photonView.IsMine)
			return;

		if(transform.position.magnitude > 200f) //transform.position.magnitude : �������������� �Ÿ� -> �������� �� 200���� �������� ������Ű��
		{
			PhotonNetwork.Destroy(photonView); //
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!photonView.IsMine) //����(������)������ �浹 üũ�ϵ��� ��
			return;

		if (other.gameObject.name == "Bullet(Clone)")
		{
			//�ε����� �� ���� ���� : �Ѿ� ���������� ���� �߰� 
			//	-> roomGameObject�� ��Ȳ�� �����ϰ� �� ó���� = �������� �Ǵ��ϴ� �Ͱ� ������ ȿ��
			other.GetComponent<Bullet>().GetSocre();
			PhotonNetwork.Destroy(photonView);
		}
	}
}
