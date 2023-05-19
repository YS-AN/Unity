using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
	public event Action<int> OnBestScoreChanged;

	private int bestScore;
	public int BestScore
	{
		get { return bestScore; } 
		set { bestScore = value; OnBestScoreChanged?.Invoke(bestScore); }
	}

	public event Action<int> OnCurrentScoreChanged;

	private int currentScore;
	public int CurrentScore { 
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
