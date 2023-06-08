using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingSceneUI : SceneUI
{
	protected override void Awake()
	{
		base.Awake();

		buttons["BtnSetting"].onClick.AddListener(() => { OpenPausePopUpUI();  });
		buttons["BtnVolume"].onClick.AddListener(() => { Debug.Log("Volume"); });
		buttons["BtnInfo"].onClick.AddListener(() => { OpenInfoWindowUI(); });
		
	}

	private void OpenPausePopUpUI()
	{
		//Debug.Log("Setting");

		GameManager.UI.ShowPopUpUI<PopUpUI>("UI/SettingPopup");
	}

	private void OpenInfoWindowUI()
	{
		GameManager.UI.ShowWindowUI<WindowUI>("UI/InfoWindowUI");
	}
}
