using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitSetting
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		if (GameManager.Instance == null)
		{
			GameObject gameObject = new GameObject() { name = "GameManager" };
			gameObject.AddComponent<GameManager>();
		}

		if (DataManager.Instance == null)
		{
			GameObject gameObject = new GameObject() { name = "DataManager" };
			gameObject.AddComponent<DataManager>();
		}
	}
}
