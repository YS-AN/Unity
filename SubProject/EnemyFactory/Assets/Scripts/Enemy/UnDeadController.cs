using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnDead;
using UnityEditor.Experimental.GraphView;

public class UnDeadController : Enemy
{
	private StateMachine<EnemyStateType, UnDeadController> stateMachine;

	protected override void Awake()
	{
		base.Awake();

		stateMachine = new StateMachine<EnemyStateType, UnDeadController>(this);
		stateMachine.AddState(EnemyStateType.Idle, new IdleState(this, stateMachine));
		stateMachine.AddState(EnemyStateType.Trace, new TraceState(this, stateMachine));
		stateMachine.AddState(EnemyStateType.Attack, new AttackState(this, stateMachine));
		stateMachine.AddState(EnemyStateType.Return, new ReturnState(this, stateMachine));
		stateMachine.AddState(EnemyStateType.Die, new DieState(this, stateMachine));
	}

	protected override void Start()
	{
		base.Start();

		stateMachine.SetUp(EnemyStateType.Idle);
	}

	private void Update()
	{
		if(curHP > 0)
		{
			stateMachine.Update();
		}
		else
		{
			Die();
		}
	}

	protected void Die()
	{
		stateMachine.ChangeState(EnemyStateType.Die);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, DataModel.DetectRange);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, DataModel.AttackRange);
	}
}


namespace UnDead
{
	public class IdleState : EnemyStatePattern<UnDeadController>
	{
		public IdleState(UnDeadController owner, StateMachine<EnemyStateType, UnDeadController> stateMachine)
			: base(owner, stateMachine)
		{
		}

		public override void Setup()
		{
			target = owner.Target;
		}

		public override void Enter()
		{
			rigidbody.velocity = Vector2.zero;
		}

		public override void Exit()
		{
		}

		public override void Transition()
		{
			//플레이어가 추적 범위로 들어오면?
			if (Vector2.Distance(target.position, transform.position) < owner.DataModel.DetectRange)
			{
				stateMachine.ChangeState(EnemyStateType.Trace); //추적 상태로 변경
			}
		}

		public override void Update()
		{
		}
	}

	//todo. 좌우로만 따라가도록 수정
	public class TraceState : EnemyStatePattern<UnDeadController>
	{
		public TraceState(UnDeadController owner, StateMachine<EnemyStateType, UnDeadController> stateMachine)
			: base(owner, stateMachine)
		{
		}

		public override void Setup()
		{
			target = owner.Target;
		}

		public override void Enter()
		{
			animator.SetBool("IsMove", true);
		}

		public override void Exit()
		{
			animator.SetBool("IsMove", false);
		}

		public override void Transition()
		{
			//플레이어가 공격 범위로 들어오면?
			if (Vector2.Distance(target.position, transform.position) < owner.DataModel.AttackRange)
			{
				stateMachine.ChangeState(EnemyStateType.Attack); //공격 상태로 변경
			}

			//플레이어가 추적 범위를 벗어나면?
			else if (Vector2.Distance(target.position, transform.position) > owner.DataModel.DetectRange)
			{
				stateMachine.ChangeState(EnemyStateType.Return); //제자리로 상태 변경
			}
		}

		public override void Update()
		{
			//플레이어 쫓아가기
			renderer.flipX = (target.position.x < transform.position.x);

			var dir = (target.position - transform.position).normalized;
			transform.Translate(dir * owner.DataModel.MoveSpeed * Time.deltaTime);
		}
	}

	public class AttackState : EnemyStatePattern<UnDeadController>
	{
		private float lastAttakTime;

		public AttackState(UnDeadController owner, StateMachine<EnemyStateType, UnDeadController> stateMachine)
			: base(owner, stateMachine)
		{
		}

		public override void Setup()
		{
			target = owner.Target;
		}

		public override void Enter()
		{
			animator.SetBool("IsAttack", true);
			lastAttakTime = 0;
		}

		public override void Exit()
		{
			animator.SetBool("IsAttack", false);
		}

		public override void Transition()
		{
			//플레이어가 공격 범위를 벗어나면?
			if (Vector2.Distance(target.position, transform.position) > owner.DataModel.AttackRange)
			{
				stateMachine.ChangeState(EnemyStateType.Trace); //추적 상태로 변경
			}
		}

		public override void Update()
		{
			if (lastAttakTime > owner.DataModel.AttackTime)
			{
				animator.SetTrigger("DoAttack");

				//todo. 공격 구현 
				Debug.Log($"[공격] {owner.DataModel.AttackPower}");

				lastAttakTime = 0;
			}
			lastAttakTime += Time.deltaTime;

		}
	}

	public class ReturnState : EnemyStatePattern<UnDeadController>
	{
		public ReturnState(UnDeadController owner, StateMachine<EnemyStateType, UnDeadController> stateMachine)
			: base(owner, stateMachine)
		{
		}

		public override void Setup()
		{
			target = owner.Target;
		}

		public override void Enter()
		{
			animator.SetBool("IsMove", true);
		}

		public override void Exit()
		{
			animator.SetBool("IsMove", false);
		}

		public override void Transition()
		{
			//제자리로 돌아오면? 
			if (Vector2.Distance(transform.position, owner.ReutnrPosition) < 0.02f)
			{
				Debug.Log("[ReturnState] 제자리로!!");
				stateMachine.ChangeState(EnemyStateType.Idle); //대기 상태로 변경
			}

			//플레이어가 추적 범위로 들어오면?
			else if (Vector2.Distance(target.position, transform.position) < owner.DataModel.DetectRange)
			{
				stateMachine.ChangeState(EnemyStateType.Trace); //추적 상태로 변경
			}
		}

		public override void Update()
		{
			renderer.flipX = (owner.ReutnrPosition.x < transform.position.x);

			Vector2 dir = (owner.ReutnrPosition - transform.position).normalized; //일정한 속도로 제자리로 돌아가도록 함
			transform.Translate(dir * owner.DataModel.MoveSpeed * Time.deltaTime);
		}
	}

	public class DieState : EnemyStatePattern<UnDeadController>
	{
		public DieState(UnDeadController owner, StateMachine<EnemyStateType, UnDeadController> stateMachine)
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
