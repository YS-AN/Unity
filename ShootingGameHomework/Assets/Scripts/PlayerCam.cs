using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
	private const int Focused = 20;
	private const int Unfocused = 10;

	[Header("Camera")]
	[SerializeField]
	private float mouseSensitivity;

	[SerializeField]
	private Transform cameraRoot;

	[SerializeField]
	private float lookDistance;

	[SerializeField]
	private List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

	private CinemachineVirtualCamera focusedCam;

	private Vector2 lookDelta;

	private float xRotation;
	private float yRotation;

	private bool isTPSCam;

	private void Awake()
	{
		focusedCam = cameras.Where(x => x.Priority == Focused).FirstOrDefault();
	}

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
		if(isTPSCam)
		{
			Rotate();
		}
	}

	private void LateUpdate()
	{
		Look();
	}

	private void OnTransCam(InputValue value)
	{
		focusedCam.Priority = Unfocused;

		focusedCam = (value.isPressed) ? cameras[1] : cameras[0];
		focusedCam.Priority = Focused;

		isTPSCam = (focusedCam.name == "TPSCamera");
	}

	private void OnLook(InputValue value)
	{
		lookDelta = value.Get<Vector2>();
	}

	private void Look()
	{
		yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;

		xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;
		xRotation = Mathf.Clamp(xRotation, -70, 70f);

		if (isTPSCam)
		{
			cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
		}
		else
		{
			cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);
			transform.localRotation = Quaternion.Euler(0, yRotation, 0);
		}
			
	}

	private void Rotate()
	{
		Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;

		lookPoint.y = transform.position.y;
		transform.LookAt(lookPoint);
	}
}
