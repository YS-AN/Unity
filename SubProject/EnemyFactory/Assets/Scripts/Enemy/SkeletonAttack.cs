using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SkeletonAttack : MonoBehaviour
{
	[SerializeField]
	private float AttackSpeed; //���� �ӵ�

	Transform target;
	Vector3 targetPos;

	void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
		targetPos = target.position;
	}

	private void Update()
	{
		DoAttack();
		ClearAttack();
	}

	private void DoAttack()
	{
		var dir = (targetPos - transform.position).normalized;
		transform.Translate(dir * AttackSpeed * Time.deltaTime);
	}

	private void ClearAttack()
	{
		//��ǥ������ �����ϸ�?
		if (Vector2.Distance(transform.position, targetPos) < 0.02f)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == target.name)
		{
			//todo.�÷��̾� ������ ����
			Debug.Log("����!");
			Destroy(gameObject);
		}
	}
}
