using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStateType
{
    Idle, 
    Trace,
    Attack,
    Return,
	Die,
	None
}

/// <summary>
/// StateBase
/// </summary>
/// <typeparam name="TState"></typeparam>
/// <typeparam name="TOwner"></typeparam>
public abstract class EnemyState<TState, TOwner> where TOwner : MonoBehaviour
{
	protected TOwner owner;
	protected StateMachine<TState, TOwner> stateMachine;

	public EnemyState(TOwner owner, StateMachine<TState, TOwner> stateMachine)
	{
		this.owner = owner;
		this.stateMachine = stateMachine;
	}

	public abstract void Setup();
	public abstract void Enter();
	public abstract void Update();
	public abstract void Transition();
	public abstract void Exit();
}
