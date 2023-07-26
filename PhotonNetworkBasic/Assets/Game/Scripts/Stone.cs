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
		//소유주가 아니면 오젝트 삭제 못하도록 함 -> 돌은 방장만 그렴 -> 따라서 삭제도 방장만 가능하도록 함. 
		if(!photonView.IsMine)
			return;

		if(transform.position.magnitude > 200f) //transform.position.magnitude : 원점에서부터의 거리 -> 원점에서 한 200정도 떨어지면 삭제시키기
		{
			PhotonNetwork.Destroy(photonView); //
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!photonView.IsMine) //방장(마스터)에서만 충돌 체크하도록 함
			return;

		if (other.gameObject.name == "Bullet(Clone)")
		{
			//부딪혔을 대 반응 구현 : 총알 소유주한테 점수 추가 
			//	-> roomGameObject가 상황을 주최하게 끔 처리함 = 서버에서 판단하는 것과 동일한 효과
			other.GetComponent<Bullet>().GetSocre();
			PhotonNetwork.Destroy(photonView);
		}
	}
}
