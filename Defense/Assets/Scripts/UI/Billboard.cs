using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������� UI�� ī�޶� ���⿡�� �� ���̵��� ȸ����Ű�� ���� ��ũ��Ʈ
public class Billboard : MonoBehaviour
{

	private void LateUpdate()
	{
		transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward); //ī�޶� ���� �ִ� ����� �Ȱ��� ������ �ٶ󺸵��� ȸ�� ��Ŵ
	}
}
