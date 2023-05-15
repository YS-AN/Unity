using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityInput : MonoBehaviour
{
	/************************************************************************
	 * 유니티 입력
	 * 
	 * 유니티에서 사용자의 명령을 감지할 수 있는 수단
	 * 사용자는 외부 장치를 이용하여 게임을 제어할 수 있음
	 * 유니티는 다양한 타입의 입력기기(키보드 및 마우스, 조이스틱, 터치스크린 등)를 지원가능한 멀티 플렛폼 게임 엔진임
	 ************************************************************************/

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		//InputByDevice();
		//InputByInputManager();

	}

	// 장치 기반 입력 -> Input
	// 특정한 장치를 기준으로 입력 감지
	// 특정한 장치의 입력을 감지하기 때문에 여러 플랫폼에 대응이 어려움 (멀티 플랫폼 환경에서는 비추임)
	void InputByDevice()
	{
		//키보드 전용 입력
		if (Input.GetKeyUp(KeyCode.Space)) //키에서 손을 때는 순간
		{
			Debug.Log("KEY UP");
		}
		if (Input.GetKeyDown(KeyCode.Space)) //키 누른 순간
		{
			Debug.Log("KEY DOWN");
		}
		if (Input.GetKey(KeyCode.Space)) //키 누르는 중
		{
			Debug.Log("KEY PRESSING");
		}

		//마우스 전용 입력
		// 왼쪽 = 0 / 오른쪽 = 1
		if (Input.GetMouseButton(0))
		{
			Debug.Log("KEY PRESSING");
		}
		if (Input.GetMouseButtonUp(0))
		{
			Debug.Log("KEY UP");
		}
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("KEY DOWN");
		}
	}

	// 행동에 따른 입력 -> InputManager
	// 여러 장치의 입력을 입력매니저에 "이름과 입력"을 정의
	// 입력매니저의 이름으로 정의한 입력의 변경사항을 확인
	//  (유니티 에디터의 Edit -> Project Settings -> Input Manager 에서 관리)
	//
	// 단, 유니티 초창기의 방식이기 때문에 키보드, 마우스, 조이스틱에 대한 장치만을 고려함
	// 추가) VR Oculus Integraion Kit 같은 경우 입력매니저와 유사한 방식을 사용
	private void InputByInputManager()
	{
		// 버튼 입력
		// Fire1 : 키보드(Left Ctrl), 마우스(Left Button), 조이스틱(button0)으로 정의 됨.
		// 버튼 : 누르는 단발성 행위임.
		if (Input.GetButton("Fire1"))
			Debug.Log("Fire1 is pressing");
		if (Input.GetButtonDown("Fire1"))
			Debug.Log("Fire1 is down");
		if (Input.GetButtonUp("Fire1"))
			Debug.Log("Fire1 is up");

		// 축 입력
		// Horizontal(수평) : 키보드(a,d / ←, →), 조이스틱(왼쪽 아날로그스틱 좌우)
		float x = Input.GetAxis("Horizontal"); //얼마만큼 눌렀는지를 받아와야 하기 때문에 반환 값이 float임
											   // Vertical(수직) : 키보드(w,s / ↑, ↓), 조이스틱(왼쪽 아날로그스틱 상하)
		float y = Input.GetAxis("Vertical");

		Debug.Log(x + " " + y);
	}

	//VR은 입력 매니저가 없음. 왜냐면 inputManager보다 VR이 더 늦게 나왔기 때문이지!
	//같은 이유로 입력 매니저에 모바일에 대한 지원도 부족한 상태이긴 함. 
	// 우리는 InputSystem 사용함.
	// 근데 사실 asset store에서 Oculus InputManager 다운 받아서 쓰면 가능은 한데.... 더 이상 지원은 안 해준다고 공표해서... 그냥 Input system을 쓰자...

	// <InputSystem>
	// Unity 2019.1 부터 지원하게 된 입력방식 -> 유니티에서는 앞으로 input system을 활용하겠다고 발표함. (근데 아직 회사들은 inputManager를 주로 이용하지)
	// 컴포넌트를 통해 입력의 변경사항을 확인
	// GamePad, JoyStick, Mouse, Keyboard, Pointer, Pen, TouchScreen, XR 기기 등 여러 기기들을 지원하고 있음
	private void InputByInputSystem()
	{
		// InputSystem은 코드로 구현하지 않고, "이벤트 방식"으로 구현됨 (기존 input과는 쓰는 방법이 다름!)
		// Update마다 입력변경사항을 확인하는 방식 대신 변경이 있을 경우 이벤트로 확인
		// 메시지를 통해 받는 방식과 이벤트 함수를 직접 연결하는 방식 등으로 구성
	}

	// Jump 입력에 반응하는 OnJump 메시지 함수 (내가 imput system에 jump에 대해 정의했기 때문에 사용이 가능한 것)
	/*
	private void OnJump(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();
		Debug.Log("ON Jump");
	}
	//*/
}