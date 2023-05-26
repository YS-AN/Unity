using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Path;
using UnityEngine;
using Bossmob;

public class BossmobController : Enemy
{
	[Header("Shooter")]
	[SerializeField]
	private GameObject AttackObj;

	[SerializeField]
	private Transform AttackPoint;

	private Transform StartAtkPoint;


	private StateMachine<EnemyStateType, BossmobController> stateMachine;

	protected override void Awake()
	{
		base.Awake();

		stateMachine = new StateMachine<EnemyStateType, BossmobController>(this);
		stateMachine.AddState(EnemyStateType.Idle, new IdleState(this, stateMachine));
		stateMachine.AddState(EnemyStateType.Attack, new AttackState(this, stateMachine));

		StartAtkPoint = AttackPoint;
	}

	protected override void Start()
	{
		base.Start();

		stateMachine.SetUp(EnemyStateType.Idle);
	}

	private void Update()
	{
		if (curHP > 0)
		{
			stateMachine.Update();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, DataModel.AttackRange);
	}

	public override void GetDamage(float damage)
	{
		if (curHP <= 0)
			return;

		base.GetDamage(damage);

		if (curHP <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		Destroy(20f);
	}

	private Coroutine ballAtkRoutine;

	public void CoroutineStart()
	{
		ballAtkRoutine = StartCoroutine(BallAttackRoutine());
	}

	public void CoroutineStop()
	{
		StopCoroutine(ballAtkRoutine);
	}

	IEnumerator BallAttackRoutine()
	{
		while(true)
		{
			yield return new WaitForSeconds(DataModel.AttackTime);
			MakeAttack();
		}
	}

	public void MakeAttack()
	{
		for(int i=0; i<5; i++)
		{
			AttackPoint.Rotate(0, 0, 10);
			Instantiate(AttackObj, AttackPoint.position, AttackPoint.rotation);
		}

		AttackPoint.Rotate(0, 0, -50);
	}
}


namespace Bossmob
{
	public class IdleState : EnemyStatePattern<BossmobController>
	{
		public IdleState(BossmobController owner, StateMachine<EnemyStateType, BossmobController> stateMachine)
			: base(owner, stateMachine)
		{
		}

		public override void Setup()
		{
			target = owner.Target;
		}

		public override void Enter()
		{
		}

		public override void Exit()
		{
		}

		public override void Transition()
		{
			//플레이어가 공격 범위로 들어오면
			if (Vector2.Distance(target.position, transform.position) < owner.DataModel.AttackRange)
			{
				stateMachine.ChangeState(EnemyStateType.Attack); //추적 상태로 변경
			}
		}

		public override void Update()
		{
		}
	}

	public class AttackState : EnemyStatePattern<BossmobController>
	{
		private float lastAttakTime;

		public AttackState(BossmobController owner, StateMachine<EnemyStateType, BossmobController> stateMachine)
			: base(owner, stateMachine)
		{
		}

		public override void Setup()
		{
			target = owner.Target;
		}

		public override void Enter()
		{
			owner.CoroutineStart();
		}

		public override void Exit()
		{
			owner.CoroutineStop();
		}

		public override void Transition()
		{
			//플레이어가 공격 범위를 벗어나면?
			if (Vector2.Distance(target.position, transform.position) > owner.DataModel.AttackRange)
			{
				stateMachine.ChangeState(EnemyStateType.Idle); 
			}
		}

		public override void Update()
		{
		}
	}
}
