using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private float ZoomSpeed;

	[SerializeField]
	private float MoveSpeed;

	[SerializeField]
	private float Padding; //얼마나 테두리 가까이에 있을지를 정함

	
	private Vector3 moveDir;
	private float zoomScroll;

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Confined; //마우스가 게임창을 벗어나지 못하게 막음
	}

	private void nDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	private void LateUpdate()
	{
		Move();
		Zoom();
	}

	private void OnPointer(InputValue value)
	{
		Vector2 mousePos = value.Get<Vector2>();

		moveDir.x = MovePosition(mousePos.x, Screen.width); //Screen.width : 스크린의 가로 크기
		moveDir.y = MovePosition(mousePos.y, Screen.height); //Screen.height : 스크린의 세로 크기
	}

	private float MovePosition(float mousePoint, int screenSize)
	{
		//x축이면 좌, 우
		//y축이면 위, 아래

		//마우스 위치가 테두리에 가까이 있을 때 움직이는 방식 -> Padding범위를 받아서 padding에 있을 때만 움직이도록 함

		if (mousePoint <= Padding) //마우스가 왼쪽이나 위쪽 거의 끝에 있을 떄
		{
			return -1; //왼쪽이나 위쪽으로 이동
		}
		else if (mousePoint >= screenSize - Padding) //마우스가 스크린 끝쪽 (오른쪽이나 아래쪽)에 있을 때
		{
			return 1; //오른쪽이나 아래쪽으로 이동
		}
		else //padding범위 밖으면 움직이지 않음
		{
			return 0;
		}
	}

	private void Move()
	{
		//World를 기준으로 움직여야 아래로 카메라가 추락하는 일을 방지할 수 있음 
		transform.Translate(Vector3.forward * moveDir.y * MoveSpeed * Time.deltaTime, Space.World); 
		transform.Translate(Vector3.right * moveDir.x * MoveSpeed * Time.deltaTime, Space.World);
	}

	private void OnZoom(InputValue value)
	{
		zoomScroll = value.Get<Vector2>().y;
	}

	private void Zoom()
	{
		transform.Translate(Vector3.forward * zoomScroll * ZoomSpeed * Time.deltaTime, Space.Self); //forward = 확대
	}
}
