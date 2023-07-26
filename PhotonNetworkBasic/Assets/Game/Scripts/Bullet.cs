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
	/// 렉걸린 시간 적용해서 거리 조정
	/// </summary>
	public void ApplyLag(float lag)
	{
		transform.position += rigidbody.velocity * lag; //속도 * 시간 = 거리 (현재 위치) -> 렉걸린 시간만큼 현재 위치를 이동시킴 
	}

	public void GetSocre()
	{
		//player에게 점수 추가
	}

}
