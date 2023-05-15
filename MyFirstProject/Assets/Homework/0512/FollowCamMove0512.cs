using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamMove0512 : MonoBehaviour
{
	/// <summary>
	/// ������ Ÿ�� ������Ʈ�� Transform
	/// </summary>
	public Transform Target;

	/// <summary>
	/// ī�޶�� ������Ʈ ������ �Ÿ�
	/// </summary>
	public float Dist;

	/// <summary>
	/// ī�޶� ����
	/// </summary>
	public float Height;

	/// <summary>
	/// ī�޶� ȸ����
	/// </summary>
	public float Ratio;

	/// <summary>
	/// ī�޶��� Transform
	/// </summary>
	private Transform cameraTf;

	private void Awake()
	{
		cameraTf = GetComponent<Transform>();
	}

	private void Update()
	{
		//�ε巯�� ȸ���� ���� Mathf.LerpAngle
		float currLerpAngle = Mathf.LerpAngle(cameraTf.eulerAngles.y, Target.eulerAngles.y, Ratio * Time.deltaTime);

		// ���Ϸ� Ÿ���� ���ʹϾ����� �ٲٱ�
		Quaternion rot = Quaternion.Euler(0, currLerpAngle, 0);
		
		// ī�޶� ��ġ�� Ÿ�� ȸ�� ������ŭ ȸ�� ��
		// ������ �Ÿ��� ���� ��ŭ ���
		cameraTf.position = Target.position - (rot * Vector3.forward * Dist) + (Vector3.up * Height);

		// Ÿ�� �ٶ󺸱�
		cameraTf.LookAt(Target);
	}
}
