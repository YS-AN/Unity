using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 시작하자마자 해야할 작업 들
public class GameSetting 
{
	
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] //RuntimeInitializeOnLoadMethod : 게임 시작하자마자 호출 될 함수로 지정하는 방법!!
																			   //씬 로드 전에? 후에? 등 호출 타이밍도 type으로 지정 가능!
	private static void Init()
	{
		if(GameManager.Instance == null)
		{
			GameObject gameObject = new GameObject() { name = "GameManager" };
			gameObject.AddComponent<GameManager>();
		}
	}
}