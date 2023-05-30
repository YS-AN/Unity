using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
	[SerializeField]
	private float mouseSensitivity;

	[SerializeField]
	private Transform cameraRoot;

	[SerializeField]
	private float lookDistance;

	private Vector2 lookDelta;
	private float xRotation;
	private float yRotation;

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	private void Update()
	{
		Rotate();
	}

	private void LateUpdate()
	{
		Look();
	}

	private void OnLook(InputValue value)
	{
		lookDelta = value.Get<Vector2>();
	}

	private void Look()
	{
		yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;

		xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;
		xRotation = Mathf.Clamp(xRotation, -80f, 80f); //x각도가 ~80도 ~ 80도까지만 움직이도록 막아줌

		//FPS는 카메라의 위치와 바로보는 위치가 동일해서 상관 없었지만, 
		//TPS는 플레이어가 바라보는 것과 상관없이 회전을 하기 위해서 localRotation이 아니라 rotation로 바꿈
		cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
	}

	private void Rotate()
	{
		//플레이어가 카메라가 바로보고 있는 곳을 같이 바라보도록 해줌
		//카메라가 바로보는 방향의 lookDistance만큼을 바라보도록 설정함
		Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;

		//lookPoint.y = 0; //카메라 보고 있는 방향으로 플레이어를 보게 해주면 위 아래 볼 때 플레이어가 방향을 위 아래로 틀어지니까
		// (ex. 카메라 위를 바라보면 플레이어가 누워버림)
		//좌우만 움직일 수 있도록 y값은 0으로 고정함.
		lookPoint.y = transform.position.y; //기준 위치를 잡을 수 있도록 함
		transform.LookAt(lookPoint);
	}
}
