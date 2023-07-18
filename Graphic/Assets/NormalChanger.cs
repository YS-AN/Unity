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
		
		//�ٷ� Material�� �������� �ʰ�, render�� Material�� ���� �������� �ʴ� ����?
		//�ҽ��� �ִ� ��� Material�� �� ����Ǿ� ���� -> ������ �����ϴ� ��. �׷��� render�� ���� �����ؾ� Ư�� ������Ʈ�� ������ ������
	}

	private void Start()
	{
		material.SetFloat("_NormalScale", 0f);
	}
}
