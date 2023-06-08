using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

//IPointer�� ���� ����ϴ� �������̽���

public class TowerPlace : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDragAndDropEvent
{
	[SerializeField]
	private Color normal;

	[SerializeField]
	private Color onMouse;

	private Renderer render; //Renderer�� ���� ����. �� �����Ϸ��� �ʿ���

	private void Awake()
	{
		render = GetComponent<Renderer>();
	}

	/// <summary>
	/// ���콺�� Ŭ������ ��
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerClick(PointerEventData eventData)
	{
		/* //Ŭ�� �׽�Ʈ
		if(eventData.button == PointerEventData.InputButton.Left) //���콺 ��Ŭ���� ���
		{
			Debug.Log($"��Ŭ�� : {eventData.clickCount}, {eventData.clickTime}");
		}
		else if (eventData.button == PointerEventData.InputButton.Right) //���콺 ��Ŭ���� ���
		{
			Debug.Log($"��Ŭ�� : {eventData.clickCount}, {eventData.clickTime}");
		}
		//*/

		BuildInGameUI buildUI = GameManager.UI.ShowInGameUI<BuildInGameUI>("UI/BuildInGameUI");
		buildUI.SetTarget(transform); //UI�� ���� ������Ʈ�� ���� �ٴϵ��� ������

		buildUI.towerPlace = this; //towerPlace�� ���� towerPlace�� ������
	}

	/// <summary>
	/// ���콺 �÷����� ��
	/// </summary>
	/// <param name="eventData"></param>
	/// <exception cref="System.NotImplementedException"></exception>
	public void OnPointerEnter(PointerEventData eventData)
	{
		render.material.color = onMouse;
	}

	/// <summary>
	/// ���콺 ������ ��
	/// </summary>
	/// <param name="eventData"></param>
	/// <exception cref="System.NotImplementedException"></exception>
	public void OnPointerExit(PointerEventData eventData)
	{
		render.material.color = normal;
	}

	/// <summary>
	/// �巹�� ���� ���� ���
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrag(PointerEventData eventData)
	{
		transform.position += new Vector3(eventData.delta.x, 0, eventData.delta.y);
	}

	/// <summary>
	/// Ÿ�� �Ǽ�
	/// </summary>
	/// <param name="data"></param>
	public void BuildTower(TowerData data)
	{
		//=> Ÿ�� �Ǽ� = TowerPlace�� �����ϰ�, ���� TowerPlace ��ġ�� Ÿ���� ����

		GameManager.Resource.Destroy(gameObject); //���� towerPlace ����
		GameManager.Resource.Instantiate(data.Towers[0].Tower, transform.position, transform.rotation);
		//���� ��ġ�� ���� 1 Ÿ���� ����
	}
}
