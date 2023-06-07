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

		//heartText = texts["heartText"]; //���� �����ϸ� heartText�� �巹�׾� ����� �Ͱ� ���� ȿ���� �� �� ���� -> ui���ε��� ���� ���� ����Ƽ ���ε��� ��������
		
		texts["HeartText"].text = "20";
		texts["CoinText"].text = "20";

	}
}
