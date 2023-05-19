using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardView : MonoBehaviour
{
	[SerializeField]
	private TMP_Text CurrentScore;

	[SerializeField]
	private TMP_Text BestScore;


	private void OnEnable()
	{
		CurrentScore.text = GameManager.DataInstance.CurrentScore.ToString();
		BestScore.text = GameManager.DataInstance.BestScore.ToString();
	}

}
