using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
	[SerializeField]
	private float mouseSensitivity; //마우스 강도

	[SerializeField]
	private Transform cameraRoot; //카메라 위치

	[SerializeField]
	private Camera MainCamera;

	[SerializeField]
	private Camera FPSCamera;

	private Vector2 lookDelta;
	private float xRotation; //x축 방향의 회전 -> 좌우로 움직임
	private float yRotation; //y축 방향의 회전 -> 위아래로 움직임

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked; //lockState : 커서를 어떻게 잠금할지 설정하는 것
												  //CursorLockMode.Locked : 마우스 커스를 센터에 잡아 둠
												  //CursorLockMode.Confined : 테두리에서 못 벗어나게 함
												  //CursorLockMode.None : 커서를 자유롭게 
		MainCamera.enabled = false;
		FPSCamera.enabled = true;
		
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;

		MainCamera.enabled = true;
		FPSCamera.enabled = false;
	}

	private void Update()
	{
		//Look(); 
		//카메라는 플레이어 이동하고 나서 위치를 조정해야 카메라가 정확한 위치를 잡을 수 있음
		//	-> 만약 카메라가 먼저 회전해버리면, 플레이어는 카메라를 따라서 회전하게 되고, 플레이어가 회전했으니 카메라가 다시 회전하고, 카메라가 회전했으니 플레이어가 회전하고....의 무한 반복이 되면서 플레이어가 뱅뱅도는 현상이 발생할 수 있음
	}

	private void LateUpdate()
	{
		Look(); //따라서 카메라는 LateUpdate에서 조정하도록 함
	}

	private void OnLook(InputValue value)
	{
		lookDelta = value.Get<Vector2>();
	}

	private void Look()
	{
		//실제 회전은 x축 회전이 위아래, y축 회전이 좌우임. 

		yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;
		xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;
		//x방향 기준으로 위를 보는 게 마이너스 회전 값이기 때문에 값을 반전시켜줘야 마우스 움직임에 따라 위아래로 볼 수 있음. 

		//Clamp 최대값을 넘어가면 최대값을, 최소값을 넘어가면 최소값을 주는 함수
		xRotation = Mathf.Clamp(xRotation, -80f, 80f); //x각도가 ~80도 ~ 80도까지만 움직이도록 막아줌

		//위아래로 본다고 해서 플레이어 오브젝트 자체가 움직이면 안 돼 -> 카메라만 위아래로 움직이도록 수정함
		cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);

		//좌우는 그냥 플레이어 자체를 좌우로 움직이디록 함 -> 보통 좌우를 볼 때는 몸을 틀어 보는 경우가 많으니.. 그렇게 만듦
		transform.localRotation = Quaternion.Euler(0, yRotation, 0);
	}

}