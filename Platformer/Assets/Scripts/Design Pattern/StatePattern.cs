using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	���� ���� :
		��ü���� �� ���� �ϳ��� ���¸��� ������ �ϸ� ��ü�� ���� ���¿� �ش��ϴ� �ൿ���� ������
	 
	���� 
	 1. ������ �ڷ������� ��ü�� ���� �� �ִ� ���µ��� ����
	 2. ���� ���¸� �����ϴ� ������ �ʱ� ���¸� ����
	 3. ��ü�� �ൿ�� �־ ���� ���¸��� �ൿ�� ����
	 4. ��ü�� ���� ������ �ൿ�� ���� �� ���� ��ȭ�� ���� �Ǵ�
	 5. ���� ��ȭ�� �־�� �ϴ� ��� ���� ���¸� ��� ���·� ����
	 6. ���°� ����� ��� ���� �ൿ�� �־ �ٲ� ���¸��� �ൿ�� ����
	 
	���� :
	 1. ��ü�� ������ �ൿ�� ������ ���ǹ��� ���·� ó���� �����ϹǷ�, ����ó���� ���� �δ��� ����
	 2. ��ü�� ������ �������¿� ���� ������� ������¸��� ó���ϹǷ�, ����ӵ��� �پ 
		(�¶��� ���� �� ���� ���͸� ��Ʈ�� �ϴ� ��� ��κ� ���� �������� ������ �� 
		 but. �ӵ��� ���� ��� �ൿ�� ���ȭ �Ǳ� ������ �ȶ��� ������ �Ұ�����. �ȶ��� ���ʹ� ���� �������δ� ������ ����)
	 3. ��ü�� ���õ� ��� ������ ������ ���¿� �л��Ű�Ƿ�, �ڵ尡 �����ϰ� �������� ����
	 
	���� :
	 1. ������ ������ ��Ȯ���� �ʰų� ������ ���� ���, ���� ���� �ڵ尡 �������� �� ����
	 2. ���¸� Ŭ������ ĸ��ȭ ��Ű�� ���� ��� ���°� ������ �����ϹǷ�, ��������Ģ�� �ؼ����� ����
	    (���� ���� �� ���͸��� 100���� �ٲ�µ�, ���ڱ� ���� �޼ҵ忡�� ���͸��� 0���� �ٲ���� �� ���� -> ĸ��ȭ�� �ʼ���)
	 3. ������ ������ ��ü�� ���������� �����ϴ� ���, ������ ������ ���� �ڵ差�� �����ϰ� ��
		(�ڵ差�� ����������, �������� ���̴� ���� ����.) -> �ʹ� ������ �ֵ��� ���� ���� �������� �������� ���ڴ� �ǹ̿��� ���� ����
 */

//���� ���� ����
namespace DesignPattern
{
	public class State
	{
		public class Mobile
		{
			/// <summary>
			/// �޴��� ���� ���� ��Ȳ�� ������
			/// </summary>
			public enum State 
			{ 
				Off, //����
				Normal, //����
				Charge, //���� ��
				FullCharged //���� 
			}

			/// <summary>
			/// ���� ���¸� ������ �� �ִ� ����
			/// </summary>
			private State state = State.Normal; //�ʱ� ���´� Normal�� ����

			private bool charging = false;
			private float battery = 50.0f;

			private void Update()
			{
				//�� ���¿� �´� ��Ȳ�� ������Ʈ�� �� �ֵ��� ��
				switch (state)
				{
					case State.Off:
						OffUpdate();
						break;
					case State.Normal:
						NormalUpdate();
						break;
					case State.Charge:
						ChargeUpdate();
						break;
					case State.FullCharged:
						FullChargedUpdate();
						break;
				}
			}

			private void OffUpdate()
			{
				// Off work
				// Do nothing

				//����, off���ٰ�, ������ �����Ѵٸ�? 
				if (charging)
				{
					state = State.Charge; //�ٷ� ������ �ϴ� �޼ҵ带 ȣ������ �ʰ�, ���� ���� ������ ���� -> ����Ʈ�� ���� ��ȯ�� �ϴ� ��
				}
			}

			private void NormalUpdate()
			{
				// Normal work
				battery -= 1.5f * Time.deltaTime;

				if (charging)
				{
					state = State.Charge;
				}
				else if (battery <= 0)
				{
					state = State.Off; //���͸��� �� ����, ���� ���·� ��ȯ�� ��! 
				}
			}

			private void ChargeUpdate()
			{
				// Charge work
				battery += 25f * Time.deltaTime;

				if (!charging)
				{
					state = State.Normal;
				}
				else if (battery >= 100)
				{
					state = State.FullCharged;
				}
			}

			private void FullChargedUpdate()
			{
				// FullCharged work
				
				if (!charging)
				{
					state = State.Normal;
				}

				//���͸� ������ ����
			}

			public void ConnectCharger()
			{
				charging = true;
			}

			public void DisConnectCharger()
			{
				charging = false;
			}
		}
	}
}