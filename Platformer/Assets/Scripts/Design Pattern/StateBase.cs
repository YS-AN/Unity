using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ��ü�� �������̽��� ���� ��쵵 ����
public interface IState
{

}

public abstract class StateBase<TOwner> where TOwner : MonoBehaviour
{
	public StateBase(TOwner owner)
	{
		this.owner = owner;
	}

	protected TOwner owner; //� ������Ʈ���� ���� ���������� �Ϲ�ȭ���� ����ص�
							//  -> �ֳĸ�, ĸ��ȭ ���Ѽ� ������ ���� �� �� ���¿� ���� �ʿ��� ������Ʈ�� �������� �� ��Ʊ� ������
							//	 ex. BeeController���� IdleUpdate�޼ҵ� ���� �� player�� position�� �ʿ���.
							//			-> �Ϲ�ȭ�� ���� ���� ������Ʈ�� �������� �ޱⰡ ����


	/// <summary>
	/// �ʱ�ȭ �޼ҵ�
	/// </summary>
	//public abstract void Setup();

	/// <summary>
	/// ������Ʈ : ���� ���濡 ���� ���� �޼ҵ尡 ����� �޼ҵ��� 
	/// </summary>
	public abstract void Update(); //���¿� ���� ������Ʈ�� �ʿ��ϴϱ� Update�� ������ �����ϵ��� �� �ֵ��� �߻� �޼ҵ�� �����ص�

	/// <summary>
	/// ���¿� �������� �� �� �ൿ
	/// </summary>
	public abstract void Enter();

	/// <summary>
	/// ���°� ������ ���� �� �� �ൿ
	/// </summary>
	public abstract void Exit();


}