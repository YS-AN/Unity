using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상태 자체를 인터페이스로 쓰는 경우도 있음
public interface IState
{

}

public abstract class StateBase<TOwner> where TOwner : MonoBehaviour
{
	public StateBase(TOwner owner)
	{
		this.owner = owner;
	}

	protected TOwner owner; //어떤 컴포넌트에서 나온 상태인지를 일반화에서 기록해둠
							//  -> 왜냐면, 캡슐화 시켜서 가지고 있을 때 각 상태에 따라 필요한 컴포넌트를 가져오는 게 어렵기 때문임
							//	 ex. BeeController에서 IdleUpdate메소드 만들 떄 player의 position이 필요함.
							//			-> 일반화를 통해 원본 컴포넌트를 가져오면 받기가 쉬움


	/// <summary>
	/// 초기화 메소드
	/// </summary>
	//public abstract void Setup();

	/// <summary>
	/// 업데이트 : 상태 변경에 따른 실행 메소드가 변경될 메소드임 
	/// </summary>
	public abstract void Update(); //상태에 따른 업데이트가 필요하니까 Update는 무조건 생성하도록 수 있도록 추상 메소드로 선언해둠

	/// <summary>
	/// 상태에 진입했을 때 할 행동
	/// </summary>
	public abstract void Enter();

	/// <summary>
	/// 상태가 마무리 됐을 때 할 행동
	/// </summary>
	public abstract void Exit();


}