using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facade : MonoBehaviour
{
	/*
		퍼사드 패턴 :
		커다란 코드 부분에 대한 간략화된 인터페이스를 제공하는 구조적 디자인 패턴
		퍼사드 객체는 복잡한 소프트웨어 바깥쪽의 코드가 라이브러리의 안쪽 코드에 의존하는 일을 감소
		복잡한 과정을 간단하게 접근할 수 있는 인터페이스를 제공

		구현 :
		1. Facade 클래스는 Subsystem 클래스들을 혼합하여 Client가 요청할 수 있는 간단한 함수를 제공
	
		장점 :
		1. Subsystem 간의 결합도를 낮춤
		2. Client 입장에서 더욱 간결한 코드로 접근할 수 있음
		
		단점 :
		1. Client에게 내부 Subsystem까지 숨길 수 없음
		2. Client가 Subsystem 내부의 클래스를 직접 사용하는 것을 막을 수 없음

		=> 역할별로 컴포넌트를 나눠서 관리하자!!
		
		UnityPlayerMoveController.cs를 동작별로 나눠서 PlayerMover, PlayerShooter로 분리함
	*/

}
