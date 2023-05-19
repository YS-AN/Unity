using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Play : MonoBehaviour
{
	public void GamePlay()
	{
		GameSceneFlow gameSceneFlow = GetComponent<GameSceneFlow>();
		gameSceneFlow.Play();
	}
}
