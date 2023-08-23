using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using static UnityEngine.InputSystem.InputAction;

public class InteractorController : MonoBehaviour
{
	[SerializeField]
	private XRDirectInteractor directInteractor;

	[SerializeField]
	private XRRayInteractor rayInteractor;

	[SerializeField]
	private XRRayInteractor teleportInteractor;


	[SerializeField]
	private InputActionReference teleportInput; //teleport 키를 눌렀는지 확인하기 위한 변수
													   //InputActionReference : inputAction 안에 있는 행동을 지정해서 사용이 가능함

	[SerializeField]
	private List<LocomotionProvider> locomotionProviders;

	private void Awake()
	{
		if (rayInteractor != null)
			rayInteractor.gameObject.SetActive(true);

		if(teleportInteractor != null)
			teleportInteractor.gameObject.SetActive(false);

	}

	private void OnEnable()
	{
		if (rayInteractor != null)
		{
			rayInteractor.selectEntered.AddListener(OnRaySelectEnter);
			rayInteractor.selectExited.AddListener(OnRaySelectExit);
			rayInteractor.uiHoverEntered.AddListener(OnUIHoverEnter);
			rayInteractor.uiHoverExited.AddListener(OnUIHoverExit);
		}

		if(teleportInput != null)
		{
			InputAction teleportAction = teleportInput.action;
			teleportAction.performed += TeleportModeStart;
			teleportAction.canceled += TeleportModeEnd;
		}
	}

	private void OnDisable()
	{
		if (rayInteractor != null)
		{
			rayInteractor.selectEntered.RemoveListener(OnRaySelectEnter);
			rayInteractor.selectExited.RemoveListener(OnRaySelectExit);
			rayInteractor.uiHoverEntered.RemoveListener(OnUIHoverEnter);
			rayInteractor.uiHoverExited.RemoveListener(OnUIHoverExit);
		}

		if (teleportInput != null)
		{
			InputAction teleportAction = teleportInput.action;
			teleportAction.performed -= TeleportModeStart;
			teleportAction.canceled -= TeleportModeEnd;
		}
	}

	/// <summary>
	/// Ray interactor로 무언가를 잡았을 때
	/// </summary>
	/// <param name="args"></param>
	private void OnRaySelectEnter(SelectEnterEventArgs args)	
	{
		//ray interactor를 통해 오브젝트에 접근한 경우에는 locomotion을 못하도록 비활성 처리
		foreach(var provider in locomotionProviders)
		{
			provider.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// ray interactor로 잡았던 것을 놨을 때
	/// </summary>
	/// <param name="args"></param>
	private void OnRaySelectExit(SelectExitEventArgs args)
	{
		foreach (var provider in locomotionProviders)
		{
			provider.gameObject.SetActive(true);
		}
	}

	private void OnUIHoverEnter(UIHoverEventArgs args)
	{
		foreach (var provider in locomotionProviders)
		{
			provider.gameObject.SetActive(true);
		}
	}

	private void OnUIHoverExit(UIHoverEventArgs args)
	{
		foreach (var provider in locomotionProviders)
		{
			provider.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// teleport 키를 눌렀을 떄
	/// </summary>
	/// <param name="context"></param>
	private void TeleportModeStart(InputAction.CallbackContext context)
	{
		//rayInteractor는 비활성 처리하고, teleport는 활성화 시킴

		if (rayInteractor != null)
			rayInteractor.gameObject.SetActive(false); 

		if(teleportInteractor != null)
			teleportInteractor.gameObject.SetActive(true);
	}

	private void TeleportModeEnd(InputAction.CallbackContext context)
	{
		if (rayInteractor != null)
			rayInteractor.gameObject.SetActive(true);

		if (teleportInteractor != null)
			teleportInteractor.gameObject.SetActive(false);
	}
}
