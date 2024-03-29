using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	private static PoolManager poolManager;
	private static ResourceManager resourceManager;
	private static UIManager uiManager;

	public static GameManager Instance { get { return instance; } }
	public static PoolManager Pool { get { return poolManager; } }
	public static ResourceManager Resource { get { return resourceManager; } }
	public static UIManager UI { get { return uiManager; } }

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
			return;
		}

		instance = this;
		DontDestroyOnLoad(this);
		InitManagers();
	}

	private void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	private void InitManagers()
	{
		//보통 리소스 메니저는 다른 매니저에서 많이 참조를 하기 때문에 리소스 매니저를 우선적으로 생성해주는 것이 오류를 방지할 수 있다.
		GameObject resourceObj = new GameObject();
		resourceObj.name = "ResourceManager";
		resourceObj.transform.parent = transform;
		resourceManager = resourceObj.AddComponent<ResourceManager>();


		GameObject poolObj = new GameObject();
		poolObj.name = "PoolManager";
		poolObj.transform.parent = transform;
		poolManager = poolObj.AddComponent<PoolManager>();


		GameObject uiObj = new GameObject();
		uiObj.name = "UIManager";
		uiObj.transform.parent = transform;
		uiManager = resourceObj.AddComponent<UIManager>();
	}
}