using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamMove0512 : MonoBehaviour
{
	/// <summary>
	/// 추적할 타겟 오브젝트의 Transform
	/// </summary>
	public Transform Target;

	/// <summary>
	/// 카메라와 오브젝트 사이의 거리
	/// </summary>
	public float Dist;

	/// <summary>
	/// 카메라 높이
	/// </summary>
	public float Height;

	/// <summary>
	/// 카메라 회전율
	/// </summary>
	public float Ratio;

	/// <summary>
	/// 카메라의 Transform
	/// </summary>
	private Transform cameraTf;

	private void Awake()
	{
		cameraTf = GetComponent<Transform>();
	}

	private void Update()
	{
		//부드러운 회전을 위한 Mathf.LerpAngle
		float currLerpAngle = Mathf.LerpAngle(cameraTf.eulerAngles.y, Target.eulerAngles.y, Ratio * Time.deltaTime);

		// 오일러 타입을 쿼터니언으로 바꾸기
		Quaternion rot = Quaternion.Euler(0, currLerpAngle, 0);
		
		// 카메라 위치를 타겟 회전 각도만큼 회전 후
		// 지정한 거리와 높이 만큼 띄움
		cameraTf.position = Target.position - (rot * Vector3.forward * Dist) + (Vector3.up * Height);

		// 타겟 바라보기
		cameraTf.LookAt(Target);
	}
}
