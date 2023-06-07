using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoSceneUI : SceneUI
{
	public TMP_Text heartText;
	public TMP_Text CoinText;

	protected override void Awake()
	{
		base.Awake();

		//heartText = texts["heartText"]; //게임 시작하면 heartText를 드레그앤 드롭한 것과 같은 효과를 볼 수 있음 -> ui바인딩을 통해 쉽게 유니티 바인딩이 가능해짐
		
		texts["HeartText"].text = "20";
		texts["CoinText"].text = "20";

	}
}
