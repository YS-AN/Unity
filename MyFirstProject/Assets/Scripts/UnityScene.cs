using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnityScene : MonoBehaviour
{
	/************************************************************************
	  * 씬 (Scene)
	  * 
	  * 유니티에서 게임월드를 구성하는 단위 -> 장면 장면의 구성단위를 나눠서 개발함
	  * 프로젝트에 원하는 수만큼 씬을 포함할 수 있음
	  * 단일 씬에서 간단한 게임을 빌드할 수도 있으며, 여러 씬을 전환하며 게임을 진행 할 수도 있음
	  * 다중 씬을 이용하여 여러 씬을 동시에 열어 같은 게임월드에서 사용도 가능함
	  ************************************************************************/

	// <빌드 설정>
	// 씬에 대한 스크립팅 전, 게임 빌드 설정에서 씬을 포함시켜야 해당 씬을 사용 가능
	//   -> 왜냐면 씬을 만든다고 해서 무조건 쓸 건 아니기 때문임. 필요한 씬만 골라서 빌드 설정을 해주는 게 더 효율적임

	// <씬 로드>
	//  -> 다 로드 될 때까지 기다렸다가 보여줌 -> 렉걸린 것 같이 보임
	public void ChangeSceneByName(string sceneName)
	{
		
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single); //sceneName 이름과 동일한 새로운 씬을 불러옴
																 //LoadSceneMode.Single : 해당 씬을 추가적으로 로드함 (다중씬이 됨)
	}

	public void ChangeSceneByIndex(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
	}

	// <비동기 씬 로드>
	public void ChangeSceneASync(string sceneName)
	{
		// 비동기 씬 로드 : 백그라운드로 씬을 로딩하도록 하여 게임 중 멈춤이 없도록 함
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

		asyncOperation.allowSceneActivation = true;     // 씬 로딩 완료시 바로 씬 로드를 진행하는지 여부 (로딩 다 되면 바로 넘어가겠니?)
		bool isLoad = asyncOperation.isDone;            // 씬 로딩이 완료되었는지 판단
		float progress = asyncOperation.progress;       // 씬 로딩률 확인
		asyncOperation.completed += (oper) => { };      // 씬 로딩 완료시 진행할 이벤트 추가
	}

	// <Don't destroy on load> 
	// 지워지지 않는 오브젝트 관리하는 씬 (ex.인벤토리 - 다음 씬 넘어간다고 인벤토리가 제거 되면 안돼)
	// 씬의 전환에도 제거되지 않기 원하는 게임오브젝트의 경우 지워지지 않는 씬의 오브젝트로 추가하는 방법을 사용
	// (동작 방법은 다중 씬을 통한 로드시에 제거되지 않는 씬을 구성하는 방법)
	public void SetDontDestroyOnLoad(GameObject go)
	{
		DontDestroyOnLoad(go);
	}

	// <씬 추가>
	public void AddSceneByName(string sceneName)
	{
		SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
	}

	public void AddSceneByIndex(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
	}

}