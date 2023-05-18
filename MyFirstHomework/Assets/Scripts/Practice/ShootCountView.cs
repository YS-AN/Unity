using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootCountView : MonoBehaviour
{
	private TMP_Text textCnt;

	private void Awake()
	{
		textCnt = GetComponent<TMP_Text>();
	}

	private void OnEnable()
	{
		DataManager.BulletManager.OnShootCountChanged += ChangeText;
	}

	private void OnDisable()
	{
		DataManager.BulletManager.OnShootCountChanged -= ChangeText;
	}

	private void ChangeText(int count)
	{
		textCnt.text = $"{count}";
	}
}
