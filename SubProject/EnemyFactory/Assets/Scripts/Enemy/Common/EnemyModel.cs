using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;
	public float MoveSpeed { get { return moveSpeed; } }

	[SerializeField]
	private float detectRange; //추적범위
	public float DetectRange { get { return detectRange; } }

	[SerializeField]
	private float attackRange; //공격 범위
	public float AttackRange { get { return attackRange; } }

	[SerializeField]
	private float attackTime; //공격 시간 (쿨타임?)
	public float AttackTime { get { return attackTime; } }

	[SerializeField]
	private float attackPower; //공격 힘 (damage)
	public float AttackPower { get { return attackPower; } }

	[SerializeField]
	private float maxHP;
	public float MaxHP { get { return maxHP; } }
}
