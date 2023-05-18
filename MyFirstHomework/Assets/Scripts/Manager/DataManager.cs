using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	private static DataManager instance;
	public static DataManager Instance { get { return instance; } }

	private static DataMgrBullet bulletManager;
	public static DataMgrBullet BulletManager { get { return bulletManager; } }

	private void Awake()
	{
		if(instance != null)
		{
			Destroy(this);
		}

		instance = this;

		DontDestroyOnLoad(this);

		InitManagers();
	}

	private void OnDestroy()
	{
		if(instance == this)
		{
			instance = null;
		}
	}

	private void InitManagers()
	{
		GameObject bulletMgr = new GameObject { name = "BulletManager" };
		bulletMgr.transform.SetParent(transform);
		bulletManager = bulletMgr.AddComponent<DataMgrBullet>();
	}
}
