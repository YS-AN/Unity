using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityAudio : MonoBehaviour
{
	/************************************************************************
	  * 오디오 (Audio)
	  * 
	  * 게임월드에서 플레이어에게 들려주는 청각적인 요소
	  * 소리를 듣는 AudioListener 컴포넌트와 소리를 발생시키는 AudioSource 컴포넌트 (오른쪽에 몬스터가 있으면 오른쪽 소리를 더 크게 만들어주기 위해 나눠둠)
	  * 소리들의 결과를 믹스, 효과적용 및 마스터링 작업하는 Audio Mixer 
	  ************************************************************************/

	// <Audio Listener & Source>
	// Audio Listener : 씬에서 주어진 오디오 소스로부터의 입력을 수신하여 사운드를 재생
	//					주로 씬에서 "카메라에 부착"되어 있으며, 하나만 유지시킴
	//					오디오 리스너가 한개만 있도록 유지해야 함 -> 보는 방향에 따라 소리 출력이 달라질 수 있기 때문임. (메인에만 오디오 소스를 가져가는 게 좋음)

	// Audio Source : 씬에서 오디오 클립을 재생
	//					오디오 클립을 재생하기 위한 다양한 오디오효과를 적용할 수 있음

	// <Audio Mixer>
	// 다양한 오디오 소스 믹스, 효과 적용 및 마스터링 작업진행
	// 오디오의 그룹을 나누어 여러 효과를 적용하고 최종적으로 합치는 작업을 진행
}
