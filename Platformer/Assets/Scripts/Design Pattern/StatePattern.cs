using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	상태 패턴 :
		객체에게 한 번에 하나의 상태만을 가지게 하며 객체는 현재 상태에 해당하는 행동만을 진행함
	 
	구현 
	 1. 열거형 자료형으로 객체가 가질 수 있는 상태들을 정의
	 2. 현재 상태를 저장하는 변수에 초기 상태를 지정
	 3. 객체는 행동에 있어서 현재 상태만의 행동을 진행
	 4. 객체는 현재 상태의 행동을 진행 후 상태 변화에 대해 판단
	 5. 상태 변화가 있어야 하는 경우 현재 상태를 대상 상태로 지정
	 6. 상태가 변경된 경우 다음 행동에 있어서 바뀐 상태만의 행동을 진행
	 
	장점 :
	 1. 객체가 진행할 행동을 복잡한 조건문을 상태로 처리가 가능하므로, 조건처리에 대한 부담이 적음
	 2. 객체가 가지는 여러상태에 대한 연산없이 현재상태만을 처리하므로, 연산속도가 뛰어남 
		(온라인 게임 중 여러 몬스터를 컨트롤 하는 경우 대부분 상태 패턴으로 구현을 함 
		 but. 속도가 빠른 대신 행동이 모듈화 되기 때문에 똑똑한 연산은 불가능함. 똑똑한 몬스터는 상태 패턴으로는 무리가 있음)
	 3. 객체와 관련된 모든 동작을 각각의 상태에 분산시키므로, 코드가 간결하고 가독성이 좋음
	 
	단점 :
	 1. 상태의 구분이 명확하지 않거나 갯수가 많은 경우, 상태 변경 코드가 복잡해질 수 있음
	 2. 상태를 클래스로 캡슐화 시키지 않은 경우 상태간 간섭이 가능하므로, 개방폐쇄원칙이 준수되지 않음
	    (충전 중일 떄 배터리를 100으로 바꿨는데, 갑자기 방전 메소드에서 배터리를 0으로 바꿔버릴 수 있음 -> 캡슐화가 필수임)
	 3. 간단한 동작의 객체에 상태패턴을 적용하는 경우, 오히려 관리할 상태 코드량이 증가하게 됨
		(코드량이 많아지더라도, 가독성을 높이는 것이 좋음.) -> 너무 간단한 애들은 굳이 상태 패턴으로 구현하지 말자는 의미에서 넣은 것임
 */

//상태 패턴 예시
namespace DesignPattern
{
	public class State
	{
		public class Mobile
		{
			/// <summary>
			/// 휴대폰 충전 중인 상황을 나열함
			/// </summary>
			public enum State 
			{ 
				Off, //방전
				Normal, //보통
				Charge, //충전 중
				FullCharged //완충 
			}

			/// <summary>
			/// 현재 상태를 보관할 수 있는 변수
			/// </summary>
			private State state = State.Normal; //초기 상태는 Normal로 지정

			private bool charging = false;
			private float battery = 50.0f;

			private void Update()
			{
				//각 상태에 맞는 상황을 업데이트할 수 있도록 함
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

				//만약, off였다가, 충전을 시작한다면? 
				if (charging)
				{
					state = State.Charge; //바로 충전을 하는 메소드를 호출하지 않고, 상태 값만 변경을 해줌 -> 포인트는 상태 변환만 하는 것
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
					state = State.Off; //베터리를 다 쓰면, 오프 상태로 전환만 함! 
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

				//배터리 충전을 멈춤
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