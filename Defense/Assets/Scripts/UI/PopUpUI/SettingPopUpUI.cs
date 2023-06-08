using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPopUpUI : PopUpUI
{
	protected override void Awake()
	{
		base.Awake();

		buttons["BtnContinue"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
		buttons["BtnSetting"].onClick.AddListener(() => { GameManager.UI.ShowPopUpUI<PopUpUI>("UI/ConfigPopUpUI"); });
		//buttons["BtnExit"].onClick.AddListener(() => { GameManager.UI.ShowPopUpUI("UI/ConfigPopUpUI"); });
	}
}
