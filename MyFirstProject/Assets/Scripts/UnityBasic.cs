using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityBasic : MonoBehaviour
{
	[Header("This GameObject")]
	public GameObject thisGameObject;

	[Header("Find GameObject")]
	public GameObject findWithName;
	public GameObject findWithTag;
	public GameObject[] findsWithTag;

	[Header("New & Destory GameObject")]
	public GameObject newGameObject;
	public GameObject destroyGameObject;

	[Space(30)]

	[Header("GetComponent On SameGameObject")]
	public AudioSource gameObjectGetComponent;
	public AudioSource componentGetComponent;

	[Header("GetComponent On OtherGameObject")]
	public GameObject otherGameObject;

	public AudioSource component;
	public AudioSource childComponent;
	public AudioSource parentComponent;
	public AudioSource[] components;
	public AudioSource[] childComponents;
	public AudioSource[] parentComponents;

	[Header("Find Component")]
	public Rigidbody findWithType;
	public Rigidbody[] findsWithType;

	[Header("Add & Destroy Component")]
	public GameObject addComponent;
	public Component destroyComponent;

	private void Start()
	{
		//GameObjectBasic();
		//ComponentBasic();
	}

	void GameObjectBasic()
	{
		// <���ӿ�����Ʈ ����>
		// ������Ʈ�� �پ��ִ� ���ӿ�����Ʈ�� gameObject �Ӽ��� �̿��Ͽ� ���ٰ���
		thisGameObject = gameObject; // gameObject = ������Ʈ�� �پ��ִ� ���ӿ�����Ʈ

		string thisName = gameObject.name;         // ���ӿ�����Ʈ�� �̸� ����
		bool thisActive = gameObject.activeSelf; // ���ӿ�����Ʈ�� Ȱ��ȭ ���� ����
		string thisTag = gameObject.tag;           // ���ӿ�����Ʈ�� �±� ����
		int thisLayer = gameObject.layer;       // ���ӿ�����Ʈ�� ���̾� ����

		// <�� ���� �ٸ� ���ӿ�����Ʈ ����>
		findWithName = GameObject.Find("Player"); // �̸����� ã�� -> �̸��� ������ ���, ���� ���� ã�� ������Ʈ�� ��ȯ (������ null��ȯ)
												  // ã�� ���� : ���̶�Ű �󿡼� ������ �ִ� �� ���� �˻�
												  // ã���� ���ÿ� �ش� ������Ʈ�� ���� load�� ���� ��(awake(), start() �� ȣ��)

		if (findWithName != null)
			Debug.Log(findWithName.tag);

		findWithTag = GameObject.FindGameObjectWithTag("Player"); // �±׷� �ϳ� ã�� -> ���� ���� GameObject
		findsWithTag = GameObject.FindGameObjectsWithTag("Player"); // �±׷� ��� ã�� -> ���� ���� GameObject[]��

		//�̸����� ã�� �� ���� �±׷� ã�� ���� ����! -> �̸����� �±װ� �˻� ���� ���� ����

		// <���ο� ���ӿ�����Ʈ ����>
		// ������ new�� ����ؼ� ��ũ��Ʈ �󿡼� ���� ������Ʈ�� ����, ��Ʈ�ѵ� ������ ��
		newGameObject = new GameObject();

		// <���ӿ�����Ʈ ����>
		//if (findWithTag != null)
		//	Destroy(findWithTag); //Destroy�� �������� �� ȣ���ϴ� �̺�Ʈ�ϱ� �����ϸ鼭 ȣ����. -> �������ض�..! o
		// �������� ���� ��� Ȱ�� ����
	}

	/// <summary>
	/// ����Ƽ���� �����ϴ� ������Ʈ �⺻ �Լ���
	/// </summary>
	void ComponentBasic()
	{
		// <���ӿ�����Ʈ���� �ٸ� ������Ʈ ����>
		// GetComponent�� �̿��Ͽ� ���ӿ�����Ʈ �� ������Ʈ ����
		gameObjectGetComponent = GetComponent<AudioSource>();

		// ������Ʈ���� GetComponent�� ����� ��� �����Ǿ� �ִ� ���ӿ�����Ʈ�� �������� ����
		componentGetComponent = GetComponent<AudioSource>(); // == gameObject.GetComponent<AudioSource>();

		component = otherGameObject.GetComponent<AudioSource>();   // ���ӿ�����Ʈ�� ������Ʈ ����
		components = otherGameObject.GetComponents<AudioSource>(); // ���ӿ�����Ʈ�� ������Ʈ�� ����

		//���� ������Ʈ�� �������� �ڽŰ� �ڽĵ� ������Ʈ�� ������Ʈ�� ã�ƿ�
		childComponent = otherGameObject.GetComponentInChildren<AudioSource>();     // �ڽ� ���ӿ�����Ʈ ���� ������Ʈ ����
		childComponents = otherGameObject.GetComponentsInChildren<AudioSource>();   // �ڽ� ���ӿ�����Ʈ ���� ������Ʈ�� ���� (���� �ڽĵ��� ���ÿ� �� ���� �۾�������� �� ���)
																					// -> �÷��̾� ����� �÷��̼� ��ü ��ǰ���� ���� �������� ���� ��찡 ����.
																					//    �̷� �� �÷��̾ ������������ �Ѵٸ�? ��� �÷��̾��� ��ǰ�� ã�� ���������� �ٲ��ֱ� ���� ���� �����

		//���������� ���� �θ�� �˻��ϸ鼭 �θ� ���� ������Ʈ�� �ִ� ������Ʈ�� �˻��� (�׷��Ա��� ���� ������� ����)
		parentComponent = otherGameObject.GetComponentInParent<AudioSource>();   // �θ� ���ӿ�����Ʈ ���� ������Ʈ ����
		parentComponents = otherGameObject.GetComponentsInParent<AudioSource>(); // �θ� ���ӿ�����Ʈ ���� ������Ʈ�� ����

		// <�� ���� ������Ʈ Ž��>
		findWithType = FindObjectOfType<Rigidbody>();   // �� ���� ������Ʈ �ϳ� ã��
		findsWithType = FindObjectsOfType<Rigidbody>(); // �� ���� ������Ʈ ��� ã��
														// �÷��̾�� �ο�ٰ� ���Ͱ� �̱� ���, �� �̻� �÷��̾� ������� �ʰ�,
														// ���ͳ��� ȯȣ�� ������ �ϱ� ���ؼ�, ��� ������Ʈ�� ã�� �ൿ�� �������� �� ��� 
														// �Ź� �����Ӹ��� ����ϴ� ���� �������� ����. �ֳĸ� ������Ʈ�� ���� �˻��ؾ��ؼ� �̸�ó�� ������Ʈ�� ���� ���� �� �˻��������!
														// �̰͵� ����ȭ�� ���� �ɸ� �� �ִ� �κ���

		// <������Ʈ �߰�>
		//Rigidbody rigid = new Rigidbody(); // �����ϳ� �ǹ̾���, ������Ʈ�� ���ӿ�����Ʈ�� �����Ǿ� �����Կ� �ǹ̰� ���� -> �ν����� â���� �ش� ������Ʈ�� �߰� �Ǿ� �־����
		// �װ� ����Ƽ �����̱� ������  -> ������Ʈ�� ���� ����� �߰�, ������ �� �ֵ��� ��
		Rigidbody rigid = addComponent.AddComponent<Rigidbody>();  // ���ӿ�����Ʈ�� ������Ʈ �߰� -> �ν����� â���ٰ� ������Ʈ �߰��ϴ� �Ͱ� ������ �ൿ�� -> �̰� ������

		// <������Ʈ ����>
		Destroy(destroyComponent);
	}
}