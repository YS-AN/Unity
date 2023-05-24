using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using BeeState;

public enum StateType { Idle, Trace, Return, Attack, Patrol, Size }

public class BeeController : MonoBehaviour
{
	private Dictionary<StateType, StateBase<BeeController>> states;
	public Dictionary<StateType, StateBase<BeeController>> States { get { return states; } }

	private StateBase<BeeController> curState; //private StateType curState; //현재 상태 

	public BeeModel BeeData;

	private Transform player;
	public Transform Player { get { return player; } }

	private Vector3 returnPos;
	public Vector3 ReutnrPosition { get { return returnPos; } }

	private int patrolIndex = 0; //현재 순찰중인 위치를 알기 위한 index변수 -> 현재 순찰중인 곳의 인덱스를 넣어줌
	public int PatrolIndex { get {  return patrolIndex; } set { patrolIndex = value; } }

	private void Start()
	{
		//curState = StateType.Idle; //씬 시작과 동시에 현재 상태를 Idle로 세팅
		curState = states[StateType.Idle]; //캡슐화로 변경 -> 초기 값을 Idle로 바꿈
		curState.Enter();

		player = GameObject.FindGameObjectWithTag("Player").transform; //태그나 이름으로 찾는 건 솔직히 권장하는 방법은 아님

		returnPos = transform.position; //시작 위치를 넣어줌
	}

	private void Awake()
	{
		states = new Dictionary<StateType, StateBase<BeeController>>();

		states.Add(StateType.Idle, new IdleState(this));
		states.Add(StateType.Trace, new TraceState(this));
		states.Add(StateType.Return, new ReturnState(this));
		states.Add(StateType.Attack, new AttackState(this));
		states.Add(StateType.Patrol, new PatrolState(this));
	}

	//상태 업데이트 구현 ********************************************************
	/*
	private void Update()
	{
		switch(curState)
		{
			case StateType.Idle:
				IdleUpdate();
				break;
			case StateType.Trace:
				TraceUpdate();
				break;
			case StateType.Return:
				ReturnUpdate();
				break;
			case StateType.Attack:
				AttackUpdate();
				break;
			case StateType.Patrol:
				PatrolUpdate();
				break;
		}
	}

	private float idleTime = 0;
	private void IdleUpdate()
	{
		BeeData.StateText.text = "Idle";

		//아무것도 안 하기
		

		if (idleTime > 2) //아무 것도 안 하는 상태가 2초이상 지나면?
		{
			idleTime = 0;
			patrolIndex = (patrolIndex + 1) % BeeData.PatrolPoints.Length;

			curState = StateType.Patrol; 
		}
		idleTime += Time.deltaTime;

		if (Vector2.Distance(player.position, transform.position) < BeeData.DetectRange) //플레이어 포지션과 몬스터의 포지션이 DetectRange보다 가까워지면,
		{
			curState = StateType.Trace; //추적 상태로 상태를 변환
		}
	}

	private void TraceUpdate()
	{
		BeeData.StateText.text = "Trace";

		//플레이어 쫓아가기
		var dir = (player.position - transform.position).normalized; //도착지 - 출발지를 도착지를 향하는 백터가 나옴.(백터의 연산)
																	 //몬스터가 플레이어를 향해 가야햐니까 플레이어 위치  - 몬스터 위치를 함
																	 //멀리있든, 가까이 있든 일정한 속도록 쫓아와야 하니까 normalized를 써서 백터를 정규화 해줌
		transform.Translate(dir * BeeData.MoveSpeed * Time.deltaTime); //방향 * 속도 * 시간(속력)

		if (Vector2.Distance(player.position, transform.position) > BeeData.DetectRange) //플레이어와 몬스터가 추적 범위를 벗어나면? 
		{
			curState = StateType.Return; //몬스터가 되돌아가도록 상태 변경함
		}
		else if (Vector2.Distance(player.position, transform.position) < BeeData.AttackRange)//플레이어와 몬스터가 너무 가까워져서 공격 범위 안으로 들어왔을 때
		{
			curState = StateType.Attack;
		}
	}

	private void ReturnUpdate()
	{
		BeeData.StateText.text = "Return";

		//원래 자리로 돌아가기
		Vector2 dir = (returnPos - transform.position).normalized; //일정한 속도로 제자리로 돌아가도록 함
		transform.Translate(dir * BeeData.MoveSpeed * Time.deltaTime);

		if(Vector2.Distance(transform.position, returnPos) < 0.02f) //원래 자리에 도착하면?
																	// -> 미묘하게 소수점 자리 수로 틀어질 수 있기 때문에 근삿값으로 확인해야함.
																	//	 -> 현재 위치와 원래 위치의 차이가 0.02이하면 도착했다고 판단함
		{
			curState = StateType.Idle;
		}

		else if (Vector2.Distance(player.position, transform.position) < BeeData.DetectRange) //제자리로 돌아가다가도, 플레이어가 추적 범위에 들어오면?
		{
			curState = StateType.Trace; //공격 모드로 변환함
		}
	}

	private float lastAttakTime = 0;

	private void AttackUpdate()
	{
		BeeData.StateText.text = "Attack";

		if(lastAttakTime > 1) //1초마다 공격하도록 수정
		{
			UnityEngine.Debug.Log("공격");
			lastAttakTime = 0;
		}
		lastAttakTime += Time.deltaTime; //단위 시간을 누적함

		if (Vector2.Distance(player.position, transform.position) > BeeData.AttackRange) //공격 범위를 벗어나면?
		{
			curState = StateType.Return; //제자로로 돌아감
		}
	}

	private void PatrolUpdate()
	{
		BeeData.StateText.text = "Patrol";

		//순찰 진행
		Vector2 dir = (BeeData.PatrolPoints[patrolIndex].position - transform.position).normalized;
		transform.Translate(dir * BeeData.MoveSpeed * Time.deltaTime);

		if (Vector2.Distance(transform.position, BeeData.PatrolPoints[patrolIndex].position) < 0.02f)
		{
			curState = StateType.Idle;
		}
		else if (Vector2.Distance(player.position, transform.position) < BeeData.DetectRange) //제자리로 돌아가다가도, 플레이어가 추적 범위에 들어오면?
		{
			curState = StateType.Trace;
		}
	}

	//*/
	//**************************************************************************


	// /* ~ */ 부분을 따로 캡슐화 된 StateBase를 상속받아 만들도록 변경함***********
	private void Update()
	{
		//유한 상태 기계(fsm)가 몬스터 AI만들기 가장 쉬움 
		//todo. fsm 적용해보기!!
		
		curState.Update();
	}

	public void ChangeState(StateBase<BeeController> state)
	{
		curState.Exit();
		state.Enter();

		curState = state;
	}
	//**************************************************************************


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, BeeData.DetectRange);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, BeeData.AttackRange);
	}
}