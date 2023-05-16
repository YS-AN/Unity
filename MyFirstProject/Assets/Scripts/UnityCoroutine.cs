using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCoroutine : MonoBehaviour
{
	/************************************************************************
	 * �ڷ�ƾ (Coroutine) -> ���� �߿���!!
	 * 
	 * ��(Co-, ����) + ��ƾ = ���� �귯���� ��ƾ
	 * 
	 * �۾��� �ټ��� �����ӿ� �л��� �� �ִ� �񵿱�� �۾� 
		(�񵿱�� �۾� = �� ���� �־ �˾Ƽ� �ϰ� ���ִ� �۾�)
	 * �ݺ������� �۾��� �л��Ͽ� �����ϸ�, ������ �Ͻ������ϰ� �ߴ��� �κк��� �ٽý����� �� ����
	 * ��, �ڷ�ƾ�� �����尡 �ƴϸ� �ڷ�ƾ�� �۾��� ������ ���� �����忡�� ����
	 *  -> ������ ����ȭ ���� ������ ��Ƽ �����带 �׷��� �������� ����. �׷��� �ڷ�ƾ�� �׳� ���� ��ƾ���� �л� �۾��� �ϴ� ����
	 ************************************************************************/

	private Coroutine routine;

	// <�ڷ�ƾ ����>
	// �ݺ������� �۾��� StartCorouine�� ���� ����
	private void CoroutineStart()
	{
		//StartCoroutine�� ���� ���۽�Ű��, update�ʹ� ������ �˾Ƽ� �����ϰ� ����
		routine = StartCoroutine(SubRoutine()); 
	}

	// "�ݺ�"�۾��̱� ������ ��ȯ���� �ݵ�� IEnumerator�� ���;� ��
	IEnumerator SubRoutine()
	{
		//3�� ��ٷȴٰ� �α� ���
		yield return new WaitForSeconds(3f); //3�ʸ��� ������

		Debug.Log($"3�� ����. �α� ���");

		/*
		for (int i = 0; i < 10; i++)
		{
			Debug.Log($"�ڷ�ƾ {i}�� ����");

			yield return new WaitForSeconds(1); //��ȯ���� �ݺ����̱� ������ yield Ű����� ��ȯ ��
		}
		*/
	}

	// <�ڷ�ƾ ����>
	// StopCoroutine�� ���� ���� ���� �ڷ�ƾ ����
	// StopAllCoroutine�� ���� ���� ���� ��� �ڷ�ƾ ����
	// �ݺ������� �۾��� ��� �Ϸ�Ǿ��� ��� �ڵ� ����
	// �ڷ�ƾ�� �����Ų ��ũ��Ʈ�� ��Ȱ��ȭ�� ��� �ڵ� ����

	private void CoroutineStop()
	{
		StopCoroutine(routine); // ������ �ڷ�ƾ ����
		StopAllCoroutines(); // ��� �ڷ�ƾ ����
	}

	// <�ڷ�ƾ �ð� ����>
	// �ڷ�ƾ�� �ð� ������ �����Ͽ� �ݺ������� �۾��� ���� Ÿ�̹��� ������ �� ����
	IEnumerator CoRoutineWait()
	{
		yield return new WaitForSeconds(1); // n�ʰ� �ð�����
		yield return null; // �ð����� ���� (���� ���� 1�����Ӹ� ��ٸ��� �ϰԴ� ���)
	}
}
