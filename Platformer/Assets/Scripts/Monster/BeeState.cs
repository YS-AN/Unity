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
			//�ƹ��͵� �� �ϱ�

			if (idleTime > 2) //�ƹ� �͵� �� �ϴ� ���°� 2���̻� ������?
			{
				idleTime = 0;
				owner.PatrolIndex = (owner.PatrolIndex + 1) % owner.BeeData.PatrolPoints.Length;
				owner.ChangeState(owner.States[StateType.Patrol]); //curState = State.Patrol;
			}
			idleTime += Time.deltaTime;

			if (Vector2.Distance(owner.Player.position, owner.transform.position) < owner.BeeData.DetectRange) //�÷��̾� �����ǰ� ������ �������� DetectRange���� ���������,
			{
				owner.ChangeState(owner.States[StateType.Trace]);//curState = State.Trace; //���� ���·� ���¸� ��ȯ
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
			//�÷��̾� �Ѿư���
			var dir = (owner.Player.position - owner.transform.position).normalized;
			//������ - ������� �������� ���ϴ� ���Ͱ� ����.(������ ����)
			//���Ͱ� �÷��̾ ���� ������ϱ� �÷��̾� ��ġ  - ���� ��ġ�� ��
			//�ָ��ֵ�, ������ �ֵ� ������ �ӵ��� �Ѿƿ;� �ϴϱ� normalized�� �Ἥ ���͸� ����ȭ ����

			owner.transform.Translate(dir * owner.BeeData.MoveSpeed * Time.deltaTime); //���� * �ӵ� * �ð�(�ӷ�)

			//�÷��̾�� ���Ͱ� ���� ������ �����? 
			if (Vector2.Distance(owner.Player.position, owner.transform.position) > owner.BeeData.DetectRange)
			{
				owner.ChangeState(owner.States[StateType.Return]); //���Ͱ� �ǵ��ư����� ���� ������
			}
			//�÷��̾�� ���Ͱ� �ʹ� ��������� ���� ���� ������ ������ ��
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
			//���� �ڸ��� ���ư���
			Vector2 dir = (owner.ReutnrPosition - owner.transform.position).normalized; //������ �ӵ��� ���ڸ��� ���ư����� ��
			owner.transform.Translate(dir * owner.BeeData.MoveSpeed * Time.deltaTime);

			//���� �ڸ��� ����??
			// -> �̹��ϰ� �Ҽ��� �ڸ� ���� Ʋ���� �� �ֱ� ������ �ٻ����� Ȯ���ؾ���.
			//	 -> ���� ��ġ�� ���� ��ġ�� ���̰� 0.02���ϸ� �����ߴٰ� �Ǵ���
			if (Vector2.Distance(owner.transform.position, owner.ReutnrPosition) < 0.02f)

			{
				owner.ChangeState(owner.States[StateType.Idle]);//curState = State.Idle;
			}
			//���ڸ��� ���ư��ٰ���, �÷��̾ ���� ������ ������?
			else if (Vector2.Distance(owner.Player.position, owner.transform.position) < owner.BeeData.DetectRange)
			{
				owner.ChangeState(owner.States[StateType.Trace]); //���� ���� ��ȯ��
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
			if (lastAttakTime > 1) //1�ʸ��� �����ϵ��� ����
			{
				UnityEngine.Debug.Log("����");
				lastAttakTime = 0;
			}
			lastAttakTime += Time.deltaTime; //���� �ð��� ������

			if (Vector2.Distance(owner.Player.position, owner.transform.position) > owner.BeeData.AttackRange) //���� ������ �����?
			{
				owner.ChangeState(owner.States[StateType.Return]); //���ڷη� ���ư�
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
			//���� ����
			Vector2 dir = (owner.BeeData.PatrolPoints[owner.PatrolIndex].position - owner.transform.position).normalized;
			owner.transform.Translate(dir * owner.BeeData.MoveSpeed * Time.deltaTime);

			if (Vector2.Distance(owner.transform.position, owner.BeeData.PatrolPoints[owner.PatrolIndex].position) < 0.02f)
			{
				owner.ChangeState(owner.States[StateType.Idle]);
			}
			//���ڸ��� ���ư��ٰ���, �÷��̾ ���� ������ ������?
			else if (Vector2.Distance(owner.Player.position, owner.transform.position) < owner.BeeData.DetectRange)
			{
				owner.ChangeState(owner.States[StateType.Trace]);
			}
		}
	}
}
