using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesingPattern
{
	/// <summary>
	/// 오브젝트 보관소
	/// </summary>
	public class ObjectPool : MonoBehaviour
	{
		[SerializeField]
		private Poolable poolablePrefab;

		[SerializeField]
		private int poolSize;

		[SerializeField]
		private int maxSize; //생성시점에서 새로운 오브젝트를 만들었더라도 maxSize이상 넘어가면 무조건 버리도록 함

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

		//외부 상황에 영향을 받을 것 같으면 (외부 데이터가 필요하면) Start에서 실행하고,
		//외부 상황과 상관없이 초기화 되어야 하면 Awake에서 실행하도록 함
		private void CreatePool()
		{
			for (int i = 0; i < poolSize; i++)
			{
				Poolable poolable = Instantiate(poolablePrefab);
				poolable.gameObject.SetActive(false); //미리 만들기 위함이지, 만들자 마자 실행되면 안 되기 때문에 생성과 동시에 오브젝트를 비활성화 시킴
													  //poolable.enabled = false; //이건 컴포넌트를 비활성화 하는 것임. gameObject.SetActive를 활용해야 오브젝트 자체를 비활성화 또는 활성화 시킬 수 있음.
				poolable.transform.SetParent(transform); //하이라키 창에서 오브젝트가 너무 많아지니까 편하게 보기 위해 ObjectPool의 하위 자식으로 넣어줌 -> 단순 폴더링 효과
				poolable.Pool = this; //반납할 장소 지정
				objPool.Push(poolable);
			}
		}

		/// <summary>
		/// Poolable 대여
		/// </summary>
		/// <returns></returns>
		public Poolable Get()
		{
			Poolable poolable = null;
			if (objPool.Count > 0)
			{
				poolable = objPool.Pop();
				poolable.gameObject.SetActive(true); //사용을 위해 꺼내는 거라 오브젝트를 꺼냄과 동시에 활성화 시킴
				poolable.transform.parent = null; //쓸 때는 하이라키 창에서 쉽게 찾기 위해 하위 자식에 뒀던 오브젝트를 상위로 꺼내 줌.
			}
			else
			{
				poolable = Instantiate(poolablePrefab); //미리 만들어 둔 오브젝트가 없으면 새로 만들어 줌
				poolable.Pool = this;
			}

			return poolable;
		}

		/// <summary>
		/// Poolable 반납
		/// </summary>
		/// <param name="poolable">반납할 오브젝트</param>
		public void Release(Poolable poolable)
		{
			Debug.Log("Release");

			if (objPool.Count < maxSize) //반납할 수 있는 최대량이 넘지 않은 경우에는 
			{
				Debug.Log(objPool.Count);

				poolable.gameObject.SetActive(false); //사용이 끝났으니 다시 비활성화 처리 함
				poolable.transform.SetParent(transform); //다시 ObjectPool의 자식으로 넣어줌
				objPool.Push(poolable); //반납
			}
			else
			{
				Debug.Log("Destroy");
				Destroy(poolable.gameObject); //최대량 넘으면 삭제
			}
		}
	}

}