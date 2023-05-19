using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scroller : MonoBehaviour
{
	[SerializeField]
	private Transform[] childs;

	[SerializeField]
	private float Speed; //�̵� �ӵ�

	[SerializeField]
	private Transform startPoint; // ������Ʈ ���� ����

	[SerializeField]
	private Transform endPoint; //������Ʈ ���� ���� 

	private void Update()
	{
		Scroll();
	}

	private void Scroll()
	{
		for(int i = 0; i < childs.Length; i++)
		{
			childs[i].Translate(Vector2.left * Speed * Time.deltaTime, Space.World); //�ӷ��� �� Time.deltaTime �־��ֱ�! 

			if (childs[i].position.x < endPoint.position.x) //���� ���� ��ġ�� �� ��ġ���� �� �������� �и���?
			{
				childs[i].position = startPoint.position; //�Ѿ ��ġ�� ���� ��ġ�� �����̵����� -> ���� ��ũ�Ѹ��� ��������
			}
		}
	}
}
