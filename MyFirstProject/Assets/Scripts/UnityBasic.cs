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
		// <게임오브젝트 접근>
		// 컴포넌트가 붙어있는 게임오브젝트는 gameObject 속성을 이용하여 접근가능
		thisGameObject = gameObject; // gameObject = 컴포넌트가 붙어있는 게임오브젝트

		string thisName = gameObject.name;         // 게임오브젝트의 이름 접근
		bool thisActive = gameObject.activeSelf; // 게임오브젝트의 활성화 여부 접근
		string thisTag = gameObject.tag;           // 게임오브젝트의 태그 접근
		int thisLayer = gameObject.layer;       // 게임오브젝트의 레이어 접근

		// <씬 내의 다른 게임오브젝트 접근>
		findWithName = GameObject.Find("Player"); // 이름으로 찾기 -> 이름이 동일할 경우, 제일 먼저 찾은 오브젝트를 반환 (없으면 null반환)
												  // 찾는 기준 : 하이라키 상에서 하위에 있는 것 먼저 검색
												  // 찾음과 동시에 해당 오브젝트에 대한 load를 같이 함(awake(), start() 등 호출)

		if (findWithName != null)
			Debug.Log(findWithName.tag);

		findWithTag = GameObject.FindGameObjectWithTag("Player"); // 태그로 하나 찾기 -> 리턴 값이 GameObject
		findsWithTag = GameObject.FindGameObjectsWithTag("Player"); // 태그로 모두 찾기 -> 리턴 값이 GameObject[]임

		//이름으로 찾는 것 보다 태그로 찾는 것을 권장! -> 이름보단 태그가 검색 수가 낮기 때문

		// <새로운 게임오브젝트 생성>
		// 실제로 new를 사용해서 스크립트 상에서 게임 오브젝트를 생성, 컨트롤도 가능은 함
		newGameObject = new GameObject();

		// <게임오브젝트 삭제>
		//if (findWithTag != null)
		//	Destroy(findWithTag); //Destroy는 마무리할 때 호출하는 이벤트니까 삭제하면서 호출함. -> 마무리해라..! o
		// 지연삭제 같은 기능 활성 가능
	}

	/// <summary>
	/// 유니티에서 제공하는 컴포넌트 기본 함수들
	/// </summary>
	void ComponentBasic()
	{
		// <게임오브젝트에서 다른 컴포넌트 접근>
		// GetComponent를 이용하여 게임오브젝트 내 컴포넌트 접근
		gameObjectGetComponent = GetComponent<AudioSource>();

		// 컴포넌트에서 GetComponent를 사용할 경우 부착되어 있는 게임오브젝트를 기준으로 접근
		componentGetComponent = GetComponent<AudioSource>(); // == gameObject.GetComponent<AudioSource>();

		component = otherGameObject.GetComponent<AudioSource>();   // 게임오브젝트의 컴포넌트 접근
		components = otherGameObject.GetComponents<AudioSource>(); // 게임오브젝트의 컴포넌트들 접근

		//게임 오브젝트를 기준으로 자신과 자식들 오브젝트의 컴포넌트를 찾아옴
		childComponent = otherGameObject.GetComponentInChildren<AudioSource>();     // 자식 게임오브젝트 포함 컴포넌트 접근
		childComponents = otherGameObject.GetComponentsInChildren<AudioSource>();   // 자식 게임오브젝트 포함 컴포넌트들 접근 (하위 자식들을 동시에 다 같이 작업해줘야할 때 사용)
																					// -> 플레이어 만들면 플레이서 신체 부품마다 따로 움직임을 갖는 경우가 있음.
																					//    이럴 때 플레이어가 반투명해져야 한다면? 모든 플레이어의 부품을 찾아 반투명으로 바꿔주기 위해 많이 사용함

		//나에서부터 상위 부모로 검색하면서 부모 게임 오브젝트에 있는 컴포넌트를 검색함 (그렇게까지 자주 사용하진 않음)
		parentComponent = otherGameObject.GetComponentInParent<AudioSource>();   // 부모 게임오브젝트 포함 컴포넌트 접근
		parentComponents = otherGameObject.GetComponentsInParent<AudioSource>(); // 부모 게임오브젝트 포함 컴포넌트들 접근

		// <씬 내의 컴포넌트 탐색>
		findWithType = FindObjectOfType<Rigidbody>();   // 씬 내의 컴포넌트 하나 찾기
		findsWithType = FindObjectsOfType<Rigidbody>(); // 씬 내의 컴포넌트 모두 찾기
														// 플레이어랑 싸우다가 몬스터가 이긴 경우, 더 이상 플레이어 따라오지 않고,
														// 몬스터끼리 환호성 지르게 하기 위해서, 모든 컴포넌트를 찾아 행동을 변경해줄 때 사용 
														// 매번 프레임마다 사용하는 것은 권장하지 않음. 왜냐면 컴포넌트에 따라서 검색해야해서 이름처럼 오브젝트에 대해 냅다 다 검색해줘야해!
														// 이것도 과부화가 많이 걸릴 수 있는 부분임

		// <컴포넌트 추가>
		//Rigidbody rigid = new Rigidbody(); // 가능하나 의미없음, 컴포넌트는 게임오브젝트에 부착되어 동작함에 의미가 있음 -> 인스펙터 창에서 해당 컴포넌트가 추가 되어 있어야해
		// 그게 유니티 컨셉이기 때문임  -> 컴포넌트로 쉽게 기능을 추가, 제거할 수 있도록 함
		Rigidbody rigid = addComponent.AddComponent<Rigidbody>();  // 게임오브젝트에 컴포넌트 추가 -> 인스펙터 창에다가 컴포넌트 추가하는 것과 동일한 행동임 -> 이건 가능해

		// <컴포넌트 삭제>
		Destroy(destroyComponent);
	}
}