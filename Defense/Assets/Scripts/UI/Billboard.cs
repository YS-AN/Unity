using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//평면적인 UI를 카메라 방향에서 잘 보이도록 회전시키기 위한 스크립트
public class Billboard : MonoBehaviour
{

	private void LateUpdate()
	{
		transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward); //카메라가 보고 있는 방향과 똑같은 방향을 바라보도록 회전 시킴
	}
}
