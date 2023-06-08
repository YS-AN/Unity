using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CreateAssetMenu : Create �ȿ� ���� �޴��� ����� ��Ʈ����Ʈ��
// fileName : default file Name
// menuName : create ���� �޴� ��ġ ����
[CreateAssetMenu(fileName = "TowerData", menuName = "Data/Tower")] 
public class TowerData : ScriptableObject, ISerializationCallbackReceiver
{
	//���� �����̱� ������ ������ ������ �Ǹ� �� �����Ͱ� �״�� ���� ����. (������ ���� ����� ���� ������ ��)
	// => ��ũ���ͺ� �����͸� �� �����ϸ� �� ��... => �ܺο��� ���� ���ϵ��� ���°� ���� 
	//public TowerInfo[] towers;

	[SerializeField]
	private TowerInfo[] towers;
	public TowerInfo[] Towers { get { return towers; } }

	//ISerializationCallbackReceiver�� ���ؼ� �����ϸ鼭 �Ѱ��ִ� ����� ����.
	//�� �κ��� �� ã�ƺ���!!

	public void OnBeforeSerialize()
	{
	
	}

	public void OnAfterDeserialize()
	{
		
	}

	[Serializable] //Serializable : Ŭ���� ����ȭ �ϴ� ���
	public class TowerInfo
	{
		public Tower Tower;

		/// <summary>
		/// �Ǽ� �ð�
		/// </summary>
		public float BuildTime;

		/// <summary>
		/// �Ǽ����
		/// </summary>
		public int BuildCost;

		/// <summary>
		/// �Ǹź��
		/// </summary>
		public int SellCost;

		/// <summary>
		/// ���� ������
		/// </summary>
		public int Damage;

		/// <summary>
		/// ���� ������
		/// </summary>
		public int Delay;

		/// <summary>
		/// ���� ����
		/// </summary>
		public int Range;

		//�޼ҵ嵵 ��ũ���ͺ� ������Ʈ�� ���� �� ����
		public virtual void Attack()
		{
			Debug.Log("����");
		}

		// ��ũ���ͺ��� Ȱ�� ����� ���ù�����
		// => ���� ���·� ����� ���� �������� �����ֱ� ���·� Ȱ���� ������

		//AI�ൿ�̶�� �ؼ�, Ư�� �ൿ�� ����� ���� �� ����.
		//	=> Youtube.Pluggable AI With Scriptable Objects

		//����Ƽ���� ������ ��ũ���ͺ� ���� ��� 3����
		//https://unity.com/kr/how-to/architect-game-code-scriptable-objects

	}
}
