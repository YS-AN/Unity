using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
	private TMP_Text CurrentScore;

	private void Awake()
	{
		CurrentScore = GetComponent<TMP_Text>();
	}

	private void OnEnable()
	{
		GameManager.DataInstance.OnCurrentScoreChanged += ChangeScore;
	}

	private void OnDisable()
	{
		GameManager.DataInstance.OnCurrentScoreChanged -= ChangeScore;
	}

	private void ChangeScore(int score)
	{
		CurrentScore.text = score.ToString();
	}
}
