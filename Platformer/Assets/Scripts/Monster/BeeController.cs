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

	private StateBase<BeeController> curState; //private StateType curState; //���� ���� 

	public BeeModel BeeData;

	private Transform player;
	public Transform Player { get { return player; } }

	private Vector3 returnPos;
	public Vector3 ReutnrPosition { get { return returnPos; } }

	private int patrolIndex = 0; //���� �������� ��ġ�� �˱� ���� index���� -> ���� �������� ���� �ε����� �־���
	public int PatrolIndex { get {  return patrolIndex; } set { patrolIndex = value; } }

	private void Start()
	{
		//curState = StateType.Idle; //�� ���۰� ���ÿ� ���� ���¸� Idle�� ����
		curState = states[StateType.Idle]; //ĸ��ȭ�� ���� -> �ʱ� ���� Idle�� �ٲ�
		curState.Enter();

		player = GameObject.FindGameObjectWithTag("Player").transform; //�±׳� �̸����� ã�� �� ������ �����ϴ� ����� �ƴ�

		returnPos = transform.position; //���� ��ġ�� �־���
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

	//���� ������Ʈ ���� ********************************************************
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

		//�ƹ��͵� �� �ϱ�
		

		if (idleTime > 2) //�ƹ� �͵� �� �ϴ� ���°� 2���̻� ������?
		{
			idleTime = 0;
			patrolIndex = (patrolIndex + 1) % BeeData.PatrolPoints.Length;

			curState = StateType.Patrol; 
		}
		idleTime += Time.deltaTime;

		if (Vector2.Distance(player.position, transform.position) < BeeData.DetectRange) //�÷��̾� �����ǰ� ������ �������� DetectRange���� ���������,
		{
			curState = StateType.Trace; //���� ���·� ���¸� ��ȯ
		}
	}

	private void TraceUpdate()
	{
		BeeData.StateText.text = "Trace";

		//�÷��̾� �Ѿư���
		var dir = (player.position - transform.position).normalized; //������ - ������� �������� ���ϴ� ���Ͱ� ����.(������ ����)
																	 //���Ͱ� �÷��̾ ���� ������ϱ� �÷��̾� ��ġ  - ���� ��ġ�� ��
																	 //�ָ��ֵ�, ������ �ֵ� ������ �ӵ��� �Ѿƿ;� �ϴϱ� normalized�� �Ἥ ���͸� ����ȭ ����
		transform.Translate(dir * BeeData.MoveSpeed * Time.deltaTime); //���� * �ӵ� * �ð�(�ӷ�)

		if (Vector2.Distance(player.position, transform.position) > BeeData.DetectRange) //�÷��̾�� ���Ͱ� ���� ������ �����? 
		{
			curState = StateType.Return; //���Ͱ� �ǵ��ư����� ���� ������
		}
		else if (Vector2.Distance(player.position, transform.position) < BeeData.AttackRange)//�÷��̾�� ���Ͱ� �ʹ� ��������� ���� ���� ������ ������ ��
		{
			curState = StateType.Attack;
		}
	}

	private void ReturnUpdate()
	{
		BeeData.StateText.text = "Return";

		//���� �ڸ��� ���ư���
		Vector2 dir = (returnPos - transform.position).normalized; //������ �ӵ��� ���ڸ��� ���ư����� ��
		transform.Translate(dir * BeeData.MoveSpeed * Time.deltaTime);

		if(Vector2.Distance(transform.position, returnPos) < 0.02f) //���� �ڸ��� �����ϸ�?
																	// -> �̹��ϰ� �Ҽ��� �ڸ� ���� Ʋ���� �� �ֱ� ������ �ٻ����� Ȯ���ؾ���.
																	//	 -> ���� ��ġ�� ���� ��ġ�� ���̰� 0.02���ϸ� �����ߴٰ� �Ǵ���
		{
			curState = StateType.Idle;
		}

		else if (Vector2.Distance(player.position, transform.position) < BeeData.DetectRange) //���ڸ��� ���ư��ٰ���, �÷��̾ ���� ������ ������?
		{
			curState = StateType.Trace; //���� ���� ��ȯ��
		}
	}

	private float lastAttakTime = 0;

	private void AttackUpdate()
	{
		BeeData.StateText.text = "Attack";

		if(lastAttakTime > 1) //1�ʸ��� �����ϵ��� ����
		{
			UnityEngine.Debug.Log("����");
			lastAttakTime = 0;
		}
		lastAttakTime += Time.deltaTime; //���� �ð��� ������

		if (Vector2.Distance(player.position, transform.position) > BeeData.AttackRange) //���� ������ �����?
		{
			curState = StateType.Return; //���ڷη� ���ư�
		}
	}

	private void PatrolUpdate()
	{
		BeeData.StateText.text = "Patrol";

		//���� ����
		Vector2 dir = (BeeData.PatrolPoints[patrolIndex].position - transform.position).normalized;
		transform.Translate(dir * BeeData.MoveSpeed * Time.deltaTime);

		if (Vector2.Distance(transform.position, BeeData.PatrolPoints[patrolIndex].position) < 0.02f)
		{
			curState = StateType.Idle;
		}
		else if (Vector2.Distance(player.position, transform.position) < BeeData.DetectRange) //���ڸ��� ���ư��ٰ���, �÷��̾ ���� ������ ������?
		{
			curState = StateType.Trace;
		}
	}

	//*/
	//**************************************************************************


	// /* ~ */ �κ��� ���� ĸ��ȭ �� StateBase�� ��ӹ޾� ���鵵�� ������***********
	private void Update()
	{
		//���� ���� ���(fsm)�� ���� AI����� ���� ���� 
		//todo. fsm �����غ���!!
		
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