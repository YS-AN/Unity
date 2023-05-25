using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	public event Action<int> OnAttacked;

	private int bestScore;
	public int BestScore
	{
		get { return bestScore; }
		set { bestScore = value; OnAttacked?.Invoke(bestScore); }
	}

	public event Action<int> OnCurrentScoreChanged;

	private int currentScore;
	public int CurrentScore
	{
		get { return currentScore; }
		set
		{
			currentScore = value;
			OnCurrentScoreChanged?.Invoke(currentScore);

			if (currentScore > bestScore)
			{
				BestScore = currentScore;
			}
		}
	}
}
