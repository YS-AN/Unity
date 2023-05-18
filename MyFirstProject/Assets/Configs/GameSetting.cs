using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� �������ڸ��� �ؾ��� �۾� ��
public class GameSetting 
{
	
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] //RuntimeInitializeOnLoadMethod : ���� �������ڸ��� ȣ�� �� �Լ��� �����ϴ� ���!!
																			   //�� �ε� ����? �Ŀ�? �� ȣ�� Ÿ�ֵ̹� type���� ���� ����!
	private static void Init()
	{
		if(GameManager.Instance == null)
		{
			GameObject gameObject = new GameObject() { name = "GameManager" };
			gameObject.AddComponent<GameManager>();
		}
	}
}