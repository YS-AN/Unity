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
		buttons["BtnInfo"].onClick.AddListener(() => { Debug.Log("Info"); });
		
	}

	private void OpenPausePopUpUI()
	{
		//Debug.Log("Setting");

		GameManager.UI.ShowPopUpUI<PopUpUI>("UI/SettingPopup");
	}
}
