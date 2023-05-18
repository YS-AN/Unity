using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TransCamera : MonoBehaviour
{
	private const int Focused = 20;
	private const int Unfocused = 10;

	[SerializeField]
	private List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

	[SerializeField]
	private CinemachineVirtualCamera MainCam;

	private CinemachineVirtualCamera focusedCam;

	private void Awake()
	{
		foreach (var cam in cameras)
		{
			cam.Priority = Unfocused;
		}
	}

	private void Start()
	{
		MainCam.Priority = Focused;
	}

	private void OnTransCamera(InputValue value)
	{
		if (value.isPressed)
		{
			Debug.Log("[OnTransCamera] isPressed");

			focusedCam = cameras.Where(x => x.name == "Muzzle Cam").FirstOrDefault();

			if (focusedCam != null)
			{
				MainCam.Priority = Unfocused;
				focusedCam.Priority = Focused;

			}
		}
		else
		{
			Debug.Log("[OnTransCamera] isNotPressed");

			if (focusedCam != null)
			{
				focusedCam.Priority = Unfocused;
				MainCam.Priority = Focused;

				focusedCam = null;
			}
		}
	}


}
