using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCoroutine : MonoBehaviour
{
	/************************************************************************
	 * 코루틴 (Coroutine) -> 정말 중요함!!
	 * 
	 * 코(Co-, 협업) + 루틴 = 같이 흘러가는 루틴
	 * 
	 * 작업을 다수의 프레임에 분산할 수 있는 비동기식 작업 
		(비동기식 작업 = 안 보고 있어도 알아서 하고 있있는 작업)
	 * 반복가능한 작업을 분산하여 진행하며, 실행을 일시정지하고 중단한 부분부터 다시시작할 수 있음
	 * 단, 코루틴은 스레드가 아니며 코루틴의 작업은 여전히 메인 스레드에서 실행
	 *  -> 게임은 동기화 문제 때문에 멀티 스레드를 그렇게 좋아하지 않음. 그래서 코루틴은 그냥 메인 루틴에서 분산 작업을 하는 것임
	 ************************************************************************/

	private Coroutine routine;

	// <코루틴 진행>
	// 반복가능한 작업을 StartCorouine을 통해 실행
	private void CoroutineStart()
	{
		//StartCoroutine를 통해 시작시키면, update와는 별개로 알아서 동작하고 있음
		routine = StartCoroutine(SubRoutine()); 
	}

	// "반복"작업이기 때문에 반환형은 반드시 IEnumerator로 나와야 함
	IEnumerator SubRoutine()
	{
		//3초 기다렸다가 로그 찍기
		yield return new WaitForSeconds(3f); //3초마다 리턴함

		Debug.Log($"3초 지남. 로그 찍기");

		/*
		for (int i = 0; i < 10; i++)
		{
			Debug.Log($"코루틴 {i}초 지남");

			yield return new WaitForSeconds(1); //반환형이 반복자이기 때문에 yield 키워드로 반환 함
		}
		*/
	}

	// <코루틴 종료>
	// StopCoroutine을 통해 진행 중인 코루틴 종료
	// StopAllCoroutine을 통해 진행 중인 모든 코루틴 종료
	// 반복가능한 작업이 모두 완료되었을 경우 자동 종료
	// 코루틴을 진행시킨 스크립트가 비활성화된 경우 자동 종료

	private void CoroutineStop()
	{
		StopCoroutine(routine); // 지정한 코루틴 종료
		StopAllCoroutines(); // 모든 코루틴 종료
	}

	// <코루틴 시간 지연>
	// 코루틴은 시간 지연을 정의하여 반복가능한 작업의 진행 타이밍을 지정할 수 있음
	IEnumerator CoRoutineWait()
	{
		yield return new WaitForSeconds(1); // n초간 시간지연
		yield return null; // 시간지연 없음 (지연 없이 1프레임만 기다리게 하게는 방법)
	}
}
