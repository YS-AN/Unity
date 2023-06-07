using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 모든 UI의 기본이 될 클래스 
///  -> 모든 UI의 공통적인 요소를 가지고 있음
/// </summary>
public class BaseUI : MonoBehaviour
{
    protected Dictionary<string, RectTransform> transforms; // UI는 transform을 상속받은 RectTransform을 사용함.
    protected Dictionary<string, Button> buttons;
    protected Dictionary<string, TMP_Text> texts;
    
    protected virtual void Awake()
    {
        BindChildren();
    }

	private void BindChildren()
	{
        transforms = new Dictionary<string, RectTransform>();
        buttons = new Dictionary<string, Button>(); 
        texts = new Dictionary<string, TMP_Text>();

		//BaseUI를 기준으로 하위 오브젝트를 모두 찾아오기 (Transform자체는 모든 오브젝트가 가지고 있기 때문에 모든 하위 오브젝트를 가져올 수 있음)
		RectTransform[] children = GetComponentsInChildren<RectTransform>();

		//하위 오브젝트를 순회하며 dictionary에 넣어주기
		foreach (var child in children)
        {
            string key = child.gameObject.name;

            if (transforms.ContainsKey(key)) //중복된 이름 있으면 추가에 제외
                continue; 

            transforms.Add(key, child);

			Button obj = child.GetComponent<Button>();
			if (obj != null)
				buttons.Add(key, obj);

			//BindDictionary(child, buttons);
			BindDictionary(child, texts);
		}
	}

    private void BindDictionary<T> (RectTransform child, Dictionary<string, T> dic)
    {
		T obj = child.GetComponent<T>();
		if (obj != null)
			dic.Add(child.gameObject.name, obj);
	}
}
