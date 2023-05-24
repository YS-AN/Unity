using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeeState
{
	public class IdleState : StateBase<BeeController>
	{
		private float idleTime;

		public IdleState(BeeController owner) : base(owner)
		{
		}

		public override void Enter()
		{
			idleTime = 0;
			owner.BeeData.StateText.text = "Idle";
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
			//아무것도 안 하기

			if (idleTime > 2) //아무 것도 안 하는 상태가 2초이상 지나면?
			{
				idleTime = 0;
				owner.PatrolIndex = (owner.PatrolIndex + 1) % owner.BeeData.PatrolPoints.Length;
				owner.ChangeState(owner.States[StateType.Patrol]); //curState = State.Patrol;
			}
			idleTime += Time.deltaTime;

			if (Vector2.Distance(owner.Player.position, owner.transform.position) < owner.BeeData.DetectRange) //플레이어 포지션과 몬스터의 포지션이 DetectRange보다 가까워지면,
			{
				owner.ChangeState(owner.States[StateType.Trace]);//curState = State.Trace; //추적 상태로 상태를 변환
			}
		}
	}


	public class TraceState : StateBase<BeeController>
	{
		public TraceState(BeeController owner) : base(owner)
		{

		}

		public override void Enter()
		{
			owner.BeeData.StateText.text = "Trace";
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
			//플레이어 쫓아가기
			var dir = (owner.Player.position - owner.transform.position).normalized;
			//도착지 - 출발지를 도착지를 향하는 백터가 나옴.(백터의 연산)
			//몬스터가 플레이어를 향해 가야햐니까 플레이어 위치  - 몬스터 위치를 함
			//멀리있든, 가까이 있든 일정한 속도록 쫓아와야 하니까 normalized를 써서 백터를 정규화 해줌

			owner.transform.Translate(dir * owner.BeeData.MoveSpeed * Time.deltaTime); //방향 * 속도 * 시간(속력)

			//플레이어와 몬스터가 추적 범위를 벗어나면? 
			if (Vector2.Distance(owner.Player.position, owner.transform.position) > owner.BeeData.DetectRange)
			{
				owner.ChangeState(owner.States[StateType.Return]); //몬스터가 되돌아가도록 상태 변경함
			}
			//플레이어와 몬스터가 너무 가까워져서 공격 범위 안으로 들어왔을 때
			else if (Vector2.Distance(owner.Player.position, owner.transform.position) < owner.BeeData.AttackRange)
			{
				owner.ChangeState(owner.States[StateType.Attack]);
			}
		}
	}


	public class ReturnState : StateBase<BeeController>
	{
		public ReturnState(BeeController owner) : base(owner)
		{

		}


		public override void Enter()
		{
			owner.BeeData.StateText.text = "Return";
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
			//원래 자리로 돌아가기
			Vector2 dir = (owner.ReutnrPosition - owner.transform.position).normalized; //일정한 속도로 제자리로 돌아가도록 함
			owner.transform.Translate(dir * owner.BeeData.MoveSpeed * Time.deltaTime);

			//원래 자리에 도착??
			// -> 미묘하게 소수점 자리 수로 틀어질 수 있기 때문에 근삿값으로 확인해야함.
			//	 -> 현재 위치와 원래 위치의 차이가 0.02이하면 도착했다고 판단함
			if (Vector2.Distance(owner.transform.position, owner.ReutnrPosition) < 0.02f)

			{
				owner.ChangeState(owner.States[StateType.Idle]);//curState = State.Idle;
			}
			//제자리로 돌아가다가도, 플레이어가 추적 범위에 들어오면?
			else if (Vector2.Distance(owner.Player.position, owner.transform.position) < owner.BeeData.DetectRange)
			{
				owner.ChangeState(owner.States[StateType.Trace]); //공격 모드로 변환함
			}
		}
	}

	public class AttackState : StateBase<BeeController>
	{
		private float lastAttakTime;

		public AttackState(BeeController owner) : base(owner)
		{

		}

		public override void Enter()
		{
			owner.BeeData.StateText.text = "Attack";
			lastAttakTime = 0;
		}

		public override void Exit()
		{
		}


		public override void Update()
		{
			if (lastAttakTime > 1) //1초마다 공격하도록 수정
			{
				UnityEngine.Debug.Log("공격");
				lastAttakTime = 0;
			}
			lastAttakTime += Time.deltaTime; //단위 시간을 누적함

			if (Vector2.Distance(owner.Player.position, owner.transform.position) > owner.BeeData.AttackRange) //공격 범위를 벗어나면?
			{
				owner.ChangeState(owner.States[StateType.Return]); //제자로로 돌아감
			}
		}
	}

	public class PatrolState : StateBase<BeeController>
	{
		public PatrolState(BeeController owner) : base(owner)
		{

		}


		public override void Enter()
		{
			owner.BeeData.StateText.text = "Patrol";
		}

		public override void Exit()
		{

		}

		public override void Update()
		{
			//순찰 진행
			Vector2 dir = (owner.BeeData.PatrolPoints[owner.PatrolIndex].position - owner.transform.position).normalized;
			owner.transform.Translate(dir * owner.BeeData.MoveSpeed * Time.deltaTime);

			if (Vector2.Distance(owner.transform.position, owner.BeeData.PatrolPoints[owner.PatrolIndex].position) < 0.02f)
			{
				owner.ChangeState(owner.States[StateType.Idle]);
			}
			//제자리로 돌아가다가도, 플레이어가 추적 범위에 들어오면?
			else if (Vector2.Distance(owner.Player.position, owner.transform.position) < owner.BeeData.DetectRange)
			{
				owner.ChangeState(owner.States[StateType.Trace]);
			}
		}
	}
}
