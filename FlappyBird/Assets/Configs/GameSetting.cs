using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSetting
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		InitGameManager();
	}

	private static void InitGameManager()
	{
		if(GameManager.Instance  == null)
		{
			GameObject gameObj = new GameObject() { name = "GameManager" };
			gameObj.AddComponent<GameManager>();
		}
	}
}
