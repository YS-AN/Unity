using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	private EventSystem eventSystem;

	private Canvas popUpCanvas; //팝업 UI는 모두 popUpCanvas의 하위 자식으로 만들 예정
								//Canvas를 벗어난 UI는 가끔 설정하지 않은 위치에 나올 수 있기 때문에 모든 ui는 Canvas의 하위메뉴로 들어가 있어야하기 때문임

	private Stack<PopUpUI> popUpStack; //ui는 statck 형식임!! -> 열린 순서에 맞게 차례대로 닫혀야 함

	private void Awake()
	{
		//EventSystem이 무조건 필요하니까 리소시스화 해서 처음 시작과 동시에 이벤트 시스템을 추가하도록 함
		eventSystem = GameManager.Resource.Instantiate<EventSystem>("UI/EventSystem");
		eventSystem.transform.parent = transform; //UIManager로 넣어주면 자연스럽게 DontDestory 안으로 들어가니 모든 씬에서 존재하는 효과를 볼 수 있음

		popUpCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
		popUpCanvas.gameObject.name = "PopUpCanvas";
		popUpCanvas.sortingOrder = 100; //모든 게임 씬 위에 나타나도록 order를 좀 높게 줌

		popUpStack = new Stack<PopUpUI>(); 
	}

	public T ShowPopUpUI<T>(T popUpUi) where T : PopUpUI
	{
		//이전에 열린 PopUp UI는 안 보이도록 설정해서 최상위에 있는 팝업 화면이 잘 보이도록 함
		// => 새로운 팝업 화면을 열기 전에 최상단의 화면(지금 활성화 되어 있는 팝업 화면)을 비활성화 처리함 
		if (popUpStack.Count > 0)
		{
			PopUpUI prevUI = popUpStack.Peek(); //가장 최상위 ui를 가져옴
			prevUI.gameObject.SetActive(false); //비활성화함
		}

		T ui = GameManager.Pool.GetUI(popUpUi); //ui대여

		ui.transform.SetParent(popUpCanvas.transform, worldPositionStays : false);
		//보통 계층구조가 되면 하위 오브젝트는 상대 위치로 설정이 됨. -> 상위 오브젝트에서 (N, M, Z)만큼 떨어진 위치. => worldPositionStays = true (기본값)
		//worldPositionStays = false가되면 그냥 내가 지정한 위치가 됨.
		//보통 UI는 그냥 지정한 위치에 그대로 나와야하니 worldPositionStays를 false로 설정함

		popUpStack.Push(ui);

		Time.timeScale = 0; //timeScale을 0으로 만듦 = 시간을 멈춤 => 일시정지 효과

		return ui;
	}

	public T ShowPopUpUI<T>(string path) where T : PopUpUI
	{
		T ui = GameManager.Resource.Load<T>(path);
		return ShowPopUpUI(ui);
	}

	public void ClosePopUpUI()
	{
		PopUpUI ui =  popUpStack.Pop();
		GameManager.Pool.Release(ui.gameObject); //ui반납

		if(popUpStack.Count > 0) //아직 팝업 열려있는 팝업 화면이 남아있닫면?
		{
			//가장 최상위 ui를 가져와 활성화 시켜줌 -> 전에 열렸던 창은 스택에서 꺼낸 후 가장 최상단 화면을 활성화함 
			PopUpUI prevUI = popUpStack.Peek(); 
			prevUI.gameObject.SetActive(true); 
		}

		if(popUpStack.Count == 0) //현재 열린 창이 하나도 없다면?
		{
			Time.timeScale = 1; //시간을 다시 흐르게 함 -> 일시정지 해제
		}
	}
}
