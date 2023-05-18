using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootCountView : MonoBehaviour
{ 
	private TMP_Text textView;

	private void Awake()
	{
		textView = GetComponent<TMP_Text>();
	}

	private void OnEnable()
	{
		GameManager.DataMgr.OnShootChanged += ChangeText;
	}

	private void Update()
	{
		GameManager.DataMgr.OnShootChanged -= ChangeText;
	}

	private void ChangeText(int count)
	{
		textView.text = count.ToString();
	}
}
