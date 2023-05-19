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

	//������ ������ ��Ȳ�� �´� ������ �̺�Ʈ�� �߰��Ͽ� ����======
	public UnityEvent OnReadyed;
	public UnityEvent OnPlayed;
	public UnityEvent OnGameOver;
	//=========================================================

	public void Readyed()
	{
		GameManager.DataInstance.CurrentScore = 0;

		CurrentState = State.Ready; //���� ��Ȳ�� �ܺο��� Ȯ���� �� �ֵ��� ���� ��Ȳ�� �־���
		OnReadyed?.Invoke(); //OnReadyed �̺�Ʈ ������ ���� �� ��
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
		Readyed(); //���� ó�� ��Ȳ�� Readyed�ϱ� ���� ���۰� ���ÿ� Readyed�� ����ǵ��� �ǵ���
	}
}
