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
	private InputActionReference teleportInput; //teleport Ű�� �������� Ȯ���ϱ� ���� ����
													   //InputActionReference : inputAction �ȿ� �ִ� �ൿ�� �����ؼ� ����� ������

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
	/// Ray interactor�� ���𰡸� ����� ��
	/// </summary>
	/// <param name="args"></param>
	private void OnRaySelectEnter(SelectEnterEventArgs args)	
	{
		//ray interactor�� ���� ������Ʈ�� ������ ��쿡�� locomotion�� ���ϵ��� ��Ȱ�� ó��
		foreach(var provider in locomotionProviders)
		{
			provider.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// ray interactor�� ��Ҵ� ���� ���� ��
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
	/// teleport Ű�� ������ ��
	/// </summary>
	/// <param name="context"></param>
	private void TeleportModeStart(InputAction.CallbackContext context)
	{
		//rayInteractor�� ��Ȱ�� ó���ϰ�, teleport�� Ȱ��ȭ ��Ŵ

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
