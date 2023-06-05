using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

//IPointer는 많이 사용하는 인터페이스임

public class TowerPlace : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDragAndDropEvent
{
	[SerializeField]
	private Color normal;

	[SerializeField]
	private Color onMouse;

	private Renderer render; //Renderer에 색이 있음. 색 변경하려면 필요함

	private void Awake()
	{
		render = GetComponent<Renderer>();
	}

	/// <summary>
	/// 마우스로 클릭했을 때
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left) //마우스 좌클릭한 경우
		{
			Debug.Log($"좌클릭 : {eventData.clickCount}, {eventData.clickTime}");
		}
		else if (eventData.button == PointerEventData.InputButton.Right) //마우스 우클릭한 경우
		{
			Debug.Log($"우클릭 : {eventData.clickCount}, {eventData.clickTime}");
		}
		
	}

	/// <summary>
	/// 마우스 올려놨을 때
	/// </summary>
	/// <param name="eventData"></param>
	/// <exception cref="System.NotImplementedException"></exception>
	public void OnPointerEnter(PointerEventData eventData)
	{
		render.material.color = onMouse;
	}

	/// <summary>
	/// 마우스 나갔을 때
	/// </summary>
	/// <param name="eventData"></param>
	/// <exception cref="System.NotImplementedException"></exception>
	public void OnPointerExit(PointerEventData eventData)
	{
		render.material.color = normal;
	}

	/// <summary>
	/// 드레그 했을 때의 결과
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrag(PointerEventData eventData)
	{
		transform.position += new Vector3(eventData.delta.x, 0, eventData.delta.y);
	}
}
