using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSceneFlow : MonoBehaviour
{
	public enum State
	{
		Ready,
		Play,
		GameOver
	}

	[SerializeField]
	private State CurrentState;

	//게임의 각각의 상황에 맞는 동작을 이벤트에 추가하여 관리======
	public UnityEvent OnReadyed;
	public UnityEvent OnPlayed;
	public UnityEvent OnGameOver;
	//=========================================================

	public void Readyed()
	{
		GameManager.DataInstance.CurrentScore = 0;

		CurrentState = State.Ready; //현재 상황을 외부에서 확인할 수 있도록 현재 상황을 넣어줌
		OnReadyed?.Invoke(); //OnReadyed 이벤트 없으면 실행 안 함
	}

	public void Play()
	{
		CurrentState = State.Play;
		OnPlayed?.Invoke(); 
	}

	public void GameOver()
	{
		CurrentState = State.GameOver;
		OnGameOver?.Invoke(); 
	}

	private void Start()
	{
		Readyed(); //게임 처음 상황이 Readyed니까 게임 시작과 동시에 Readyed가 실행되도록 의도함
	}
}
