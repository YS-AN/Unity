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
		//유니티는 컴포넌트이기 때문에 new 키워드 이용하지 않고, 프로그램 상에서도 충분히 인스턴스 생성이 가능함 (컴포넌트 추가 등)
		// -> 때문에 별도의 조치가 필요함
		if(instance != null) //먼저 선점해서 생성이 되어 있다면? 
		{
			Destroy(this);  //지금 만들려고 시도한 오브젝트를 제거함

			//=> 유니티 상에서 빈 오브젝트 생성 후 GameManager N개 추가하면, 게임 시작과 동시에 1개를 빼고 나머지 오브젝트는 다 삭제 됨. 
		}

		instance = this;

		DontDestroyOnLoad(this); //싱글톤은 씬 전환 후에도 유지되어야 하기 때문에 보통은 싱글톤들은 DontDestroyOnLoad에 포함시켜줌


		//그렇다면 만약, 안 만들어지면 어떡하지? 
		// -> 게임 시작하자마자 일단 자동으로 싱글톤을 생성해 주도록 함! (Assets/Configs/GameSetting.cs 참고)

		InitManagers();
	}

	private void Start()
	{
		GameManager.Instance.GameStart();
	}

	private void OnDestroy()
	{
		//마무리 작업
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
		dataObj.transform.SetParent(transform); //DataManager를 GameManager의 자식으로 설정함
		dataManager = dataObj.AddComponent<DataManager>();
	}
}
