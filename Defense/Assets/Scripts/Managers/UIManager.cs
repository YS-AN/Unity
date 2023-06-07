using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	private EventSystem eventSystem;

	private Canvas popUpCanvas; //�˾� UI�� ��� popUpCanvas�� ���� �ڽ����� ���� ����
								//Canvas�� ��� UI�� ���� �������� ���� ��ġ�� ���� �� �ֱ� ������ ��� ui�� Canvas�� �����޴��� �� �־���ϱ� ������

	private Stack<PopUpUI> popUpStack; //ui�� statck ������!! -> ���� ������ �°� ���ʴ�� ������ ��

	private void Awake()
	{
		//EventSystem�� ������ �ʿ��ϴϱ� ���ҽý�ȭ �ؼ� ó�� ���۰� ���ÿ� �̺�Ʈ �ý����� �߰��ϵ��� ��
		eventSystem = GameManager.Resource.Instantiate<EventSystem>("UI/EventSystem");
		eventSystem.transform.parent = transform; //UIManager�� �־��ָ� �ڿ������� DontDestory ������ ���� ��� ������ �����ϴ� ȿ���� �� �� ����

		popUpCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
		popUpCanvas.gameObject.name = "PopUpCanvas";
		popUpCanvas.sortingOrder = 100; //��� ���� �� ���� ��Ÿ������ order�� �� ���� ��

		popUpStack = new Stack<PopUpUI>(); 
	}

	public T ShowPopUpUI<T>(T popUpUi) where T : PopUpUI
	{
		//������ ���� PopUp UI�� �� ���̵��� �����ؼ� �ֻ����� �ִ� �˾� ȭ���� �� ���̵��� ��
		// => ���ο� �˾� ȭ���� ���� ���� �ֻ���� ȭ��(���� Ȱ��ȭ �Ǿ� �ִ� �˾� ȭ��)�� ��Ȱ��ȭ ó���� 
		if (popUpStack.Count > 0)
		{
			PopUpUI prevUI = popUpStack.Peek(); //���� �ֻ��� ui�� ������
			prevUI.gameObject.SetActive(false); //��Ȱ��ȭ��
		}

		T ui = GameManager.Pool.GetUI(popUpUi); //ui�뿩

		ui.transform.SetParent(popUpCanvas.transform, worldPositionStays : false);
		//���� ���������� �Ǹ� ���� ������Ʈ�� ��� ��ġ�� ������ ��. -> ���� ������Ʈ���� (N, M, Z)��ŭ ������ ��ġ. => worldPositionStays = true (�⺻��)
		//worldPositionStays = false���Ǹ� �׳� ���� ������ ��ġ�� ��.
		//���� UI�� �׳� ������ ��ġ�� �״�� ���;��ϴ� worldPositionStays�� false�� ������

		popUpStack.Push(ui);

		Time.timeScale = 0; //timeScale�� 0���� ���� = �ð��� ���� => �Ͻ����� ȿ��

		return ui;
	}

	public T ShowPopUpUI<T>(string path) where T : PopUpUI
	{
		T ui = GameManager.Resource.Load<T>(path);
		return ShowPopUpUI(ui);
	}

	public void ClosePopUpUI()
	{
		PopUpUI ui =  popUpStack.Pop();
		GameManager.Pool.Release(ui.gameObject); //ui�ݳ�

		if(popUpStack.Count > 0) //���� �˾� �����ִ� �˾� ȭ���� �����ִݸ�?
		{
			//���� �ֻ��� ui�� ������ Ȱ��ȭ ������ -> ���� ���ȴ� â�� ���ÿ��� ���� �� ���� �ֻ�� ȭ���� Ȱ��ȭ�� 
			PopUpUI prevUI = popUpStack.Peek(); 
			prevUI.gameObject.SetActive(true); 
		}

		if(popUpStack.Count == 0) //���� ���� â�� �ϳ��� ���ٸ�?
		{
			Time.timeScale = 1; //�ð��� �ٽ� �帣�� �� -> �Ͻ����� ����
		}
	}
}
