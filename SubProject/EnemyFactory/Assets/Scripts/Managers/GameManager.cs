using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	public static GameManager Instance { get { return instance; } }

	private static DataManager dataInstance;
	public static DataManager DataInstance { get { return dataInstance; } }

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

	private void InitManagers()
	{
		GameObject dataObj = new GameObject() { name = "DataManager" };
		dataInstance = dataObj.AddComponent<DataManager>();
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

}
