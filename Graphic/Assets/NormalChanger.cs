using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChanger : MonoBehaviour
{
	Renderer render;
	Material material;

	private void Awake()
	{
		render = GetComponent<Renderer>();
		material = render.material;  
		
		//바로 Material에 접근하지 않고, render의 Material를 통해 접근하지 않는 이유?
		//소스상에 있는 모든 Material이 다 변경되어 버림 -> 원본을 변경하는 꼴. 그래서 render를 통해 접근해야 특정 오브젝트만 변경이 가능함
	}

	private void Start()
	{
		material.SetFloat("_NormalScale", 0f);
	}
}
