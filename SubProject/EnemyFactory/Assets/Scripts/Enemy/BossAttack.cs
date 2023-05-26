using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
	[SerializeField]
	private float AttackSpeed; //공격 속도

	Transform target => GameObject.FindGameObjectWithTag("Player").transform;
	Vector3 targetPos;

	void Start()
	{
		targetPos = GameObject.FindGameObjectWithTag("Player").transform.position;
	}

	private void Update()
	{
		DoAttack();
		ClearAttack();
	}

	private void DoAttack()
	{
		transform.Translate(transform.right * AttackSpeed * Time.deltaTime);
	}

	private void ClearAttack()
	{
		//목표지점에 도달하면?
		if (Vector2.Distance(transform.position, targetPos) < 0.02f)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == target.name)
		{
			//todo.플레이어 데미지 감소
			Debug.Log("공격!");
			Destroy(gameObject);
		}
	}
}
