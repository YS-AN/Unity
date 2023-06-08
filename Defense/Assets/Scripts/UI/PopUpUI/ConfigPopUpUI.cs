using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigPopUpUI : PopUpUI
{
	protected override void Awake()
	{
		base.Awake();

		buttons["BtnSave"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
		buttons["BtnCancel"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });

		

	}
}
