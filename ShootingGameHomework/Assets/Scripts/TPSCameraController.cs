using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
	[SerializeField]
	private float mouseSensitivity;

	[SerializeField]
	private Transform camRoot;

	[SerializeField]
	private Transform player;

	[SerializeField]
	private float lookDist;

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
		xRotation = Mathf.Clamp(xRotation, -70f, 70f);
		
		camRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
	}

	private void Rotate()
	{
		Vector3 lookPoint = transform.position + transform.forward * lookDist;

		lookPoint.y = player.position.y;

		player.LookAt(lookPoint);
	}
}