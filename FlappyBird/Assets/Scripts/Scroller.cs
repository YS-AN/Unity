using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scroller : MonoBehaviour
{
	[SerializeField]
	private Transform[] childs;

	[SerializeField]
	private float Speed; //이동 속도

	[SerializeField]
	private Transform startPoint; // 오브젝트 시작 지점

	[SerializeField]
	private Transform endPoint; //오브젝트 종료 지점 

	private void Update()
	{
		Scroll();
	}

	private void Scroll()
	{
		for(int i = 0; i < childs.Length; i++)
		{
			childs[i].Translate(Vector2.left * Speed * Time.deltaTime, Space.World); //속력은 꼭 Time.deltaTime 넣어주기! 

			if (childs[i].position.x < endPoint.position.x) //만약 현재 위치가 끝 위치보다 더 뒤쪽으로 밀리면?
			{
				childs[i].position = startPoint.position; //넘어간 위치는 시작 위치를 순간이동해줌 -> 무한 스크롤링이 가능해짐
			}
		}
	}
}
