using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesingPattern
{
	/// <summary>
	/// ������Ʈ ������
	/// </summary>
	public class ObjectPool : MonoBehaviour
	{
		[SerializeField]
		private Poolable poolablePrefab;

		[SerializeField]
		private int poolSize;

		[SerializeField]
		private int maxSize; //������������ ���ο� ������Ʈ�� ��������� maxSize�̻� �Ѿ�� ������ �������� ��

		private Stack<Poolable> objPool;

		private void Awake()
		{
			objPool = new Stack<Poolable>();
			CreatePool();
		}

		private void Start()
		{
			//CreatePool();
		}

		//�ܺ� ��Ȳ�� ������ ���� �� ������ (�ܺ� �����Ͱ� �ʿ��ϸ�) Start���� �����ϰ�,
		//�ܺ� ��Ȳ�� ������� �ʱ�ȭ �Ǿ�� �ϸ� Awake���� �����ϵ��� ��
		private void CreatePool()
		{
			for (int i = 0; i < poolSize; i++)
			{
				Poolable poolable = Instantiate(poolablePrefab);
				poolable.gameObject.SetActive(false); //�̸� ����� ��������, ������ ���� ����Ǹ� �� �Ǳ� ������ ������ ���ÿ� ������Ʈ�� ��Ȱ��ȭ ��Ŵ
													  //poolable.enabled = false; //�̰� ������Ʈ�� ��Ȱ��ȭ �ϴ� ����. gameObject.SetActive�� Ȱ���ؾ� ������Ʈ ��ü�� ��Ȱ��ȭ �Ǵ� Ȱ��ȭ ��ų �� ����.
				poolable.transform.SetParent(transform); //���̶�Ű â���� ������Ʈ�� �ʹ� �������ϱ� ���ϰ� ���� ���� ObjectPool�� ���� �ڽ����� �־��� -> �ܼ� ������ ȿ��
				poolable.Pool = this; //�ݳ��� ��� ����
				objPool.Push(poolable);
			}
		}

		/// <summary>
		/// Poolable �뿩
		/// </summary>
		/// <returns></returns>
		public Poolable Get()
		{
			Poolable poolable = null;
			if (objPool.Count > 0)
			{
				poolable = objPool.Pop();
				poolable.gameObject.SetActive(true); //����� ���� ������ �Ŷ� ������Ʈ�� ������ ���ÿ� Ȱ��ȭ ��Ŵ
				poolable.transform.parent = null; //�� ���� ���̶�Ű â���� ���� ã�� ���� ���� �ڽĿ� �״� ������Ʈ�� ������ ���� ��.
			}
			else
			{
				poolable = Instantiate(poolablePrefab); //�̸� ����� �� ������Ʈ�� ������ ���� ����� ��
				poolable.Pool = this;
			}

			return poolable;
		}

		/// <summary>
		/// Poolable �ݳ�
		/// </summary>
		/// <param name="poolable">�ݳ��� ������Ʈ</param>
		public void Release(Poolable poolable)
		{
			Debug.Log("Release");

			if (objPool.Count < maxSize) //�ݳ��� �� �ִ� �ִ뷮�� ���� ���� ��쿡�� 
			{
				Debug.Log(objPool.Count);

				poolable.gameObject.SetActive(false); //����� �������� �ٽ� ��Ȱ��ȭ ó�� ��
				poolable.transform.SetParent(transform); //�ٽ� ObjectPool�� �ڽ����� �־���
				objPool.Push(poolable); //�ݳ�
			}
			else
			{
				Debug.Log("Destroy");
				Destroy(poolable.gameObject); //�ִ뷮 ������ ����
			}
		}
	}

}