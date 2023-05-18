using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
	private static GameManager instance;
	public static GameManager Instance { get { return instance; } }

	private static DataManager dataManager;
	public static DataManager DataMgr { get { return dataManager; } }

	private void Awake()
	{
		//����Ƽ�� ������Ʈ�̱� ������ new Ű���� �̿����� �ʰ�, ���α׷� �󿡼��� ����� �ν��Ͻ� ������ ������ (������Ʈ �߰� ��)
		// -> ������ ������ ��ġ�� �ʿ���
		if(instance != null) //���� �����ؼ� ������ �Ǿ� �ִٸ�? 
		{
			Destroy(this);  //���� ������� �õ��� ������Ʈ�� ������

			//=> ����Ƽ �󿡼� �� ������Ʈ ���� �� GameManager N�� �߰��ϸ�, ���� ���۰� ���ÿ� 1���� ���� ������ ������Ʈ�� �� ���� ��. 
		}

		instance = this;

		DontDestroyOnLoad(this); //�̱����� �� ��ȯ �Ŀ��� �����Ǿ�� �ϱ� ������ ������ �̱������ DontDestroyOnLoad�� ���Խ�����


		//�׷��ٸ� ����, �� ��������� �����? 
		// -> ���� �������ڸ��� �ϴ� �ڵ����� �̱����� ������ �ֵ��� ��! (Assets/Configs/GameSetting.cs ����)

		InitManagers();
	}

	private void Start()
	{
		GameManager.Instance.GameStart();
	}

	private void OnDestroy()
	{
		//������ �۾�
		if(instance == this)
		{
			instance = null;
		}
	}

	public void GameStart()
	{

	}

	private void InitManagers()
	{
		GameObject dataObj = new GameObject { name = "DataManager" };
		dataObj.transform.SetParent(transform); //DataManager�� GameManager�� �ڽ����� ������
		dataManager = dataObj.AddComponent<DataManager>();
	}
}
