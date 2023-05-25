using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Path;
using UnityEngine;
using Bossmob;

public class BossmobController : Enemy
{
	private StateMachine<EnemyStateType, BossmobController> stateMachine;

	protected override void Awake()
	{
		base.Awake();

		stateMachine = new StateMachine<EnemyStateType, BossmobController>(this);
		stateMachine.AddState(EnemyStateType.Idle, new IdleState(this, stateMachine));
		stateMachine.AddState(EnemyStateType.Attack, new AttackState(this, stateMachine));
		stateMachine.AddState(EnemyStateType.Die, new DieState(this, stateMachine));
	}

	protected override void Start()
	{
		base.Start();

		stateMachine.SetUp(EnemyStateType.Idle);
	}

	private void Update()
	{
		stateMachine.Update();
	}

	protected void Die()
	{
		stateMachine.ChangeState(EnemyStateType.Die);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, DataModel.AttackRange);
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
		}

		public override void Exit()
		{
		}

		public override void Transition()
		{
		}

		public override void Update()
		{
		}
	}

	public class DieState : EnemyStatePattern<BossmobController>
	{
		public DieState(BossmobController owner, StateMachine<EnemyStateType, BossmobController> stateMachine)
			: base(owner, stateMachine)
		{
		}

		public override void Setup()
		{
			target = owner.Target;
		}

		public override void Enter()
		{
			owner.Destroy();
		}

		public override void Exit()
		{
		}

		public override void Transition()
		{

		}

		public override void Update()
		{
		}
	}
}
