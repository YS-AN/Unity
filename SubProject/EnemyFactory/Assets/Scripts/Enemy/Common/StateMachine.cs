using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TState, TOwner> where TOwner : MonoBehaviour
{
	private TOwner owner;
	private Dictionary<TState, EnemyState<TState, TOwner>> states;
	private EnemyState<TState, TOwner> curState;

	public StateMachine(TOwner owner)
	{
		this.owner = owner;
		this.states = new Dictionary<TState, EnemyState<TState, TOwner>>();
	}

	public void AddState(TState state, EnemyState<TState, TOwner> stateBase)
	{
		states.Add(state, stateBase);
	}

	/// <summary>
	/// 생성된 모든 상태에 대해 초기화 작업을 진행한다. (각 상태의 SetUp메소드 실행)
	/// </summary>
	/// <param name="startState">SetUp완료 후 처음으로 세팅할 상태</param>
	public void SetUp(TState startState)
	{
		foreach (var state in states.Values)
		{
			state.Setup();
		}

		curState = states[startState];
		curState.Enter();
	}

	public void Update()
	{
		curState.Update();
		curState.Transition();
	}

	public void ChangeState(TState newState)
	{
		curState.Exit();
		curState = states[newState];
		curState.Enter();
	}
}
