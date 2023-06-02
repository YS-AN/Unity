using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool; //유니티엔진에서 오브젝트 풀을 지원해 줌. (2021부터 지원해줌)
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
	Dictionary<string, ObjectPool<GameObject>> poolDic; //여러 오브젝트 풀을 갖기 위해 Dictionary로 선언함

	private void Awake()
	{
		poolDic = new Dictionary<string, ObjectPool<GameObject>>();
	}

	//유니트 엔진 오브젝트 풀은 없으면 알아서 만들어주고, 꽉 차 있는데 반납하면 알아서 삭제해줌 

	/*
	public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		if (poolDic.ContainsKey(prefab.name) == false) //한번도 오브젝트 풀이 만들어지지 않다면? 
		{
			CreatePool(prefab.name, prefab); //오브젝트 풀을 만들어 줌
		}

		GameObject obj = poolDic[prefab.name].Get();

		obj.transform.position = posittion;
		return obj;ion;
		obj.transform.rotation = rota
	}

	public GameObject Get(GameObject prefab)
	{
		return Get(prefab, Vector3.zero, Quaternion.identity);
	}
	//*/

	//일반화 형태로 변형 

	//현업에서 쓰는 형태라기 보단 개념을 이해시키기 위한 형태라고 할 수 있음
	//실제 사용 시에는 boxing, unboxing 고려해서 만들기!
	
	public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object
	{
		//object로 제약 조건을 걸면, GameObject 또는 Component가 온다고 가정할 수 있음 

		if (original is GameObject)
		{
			GameObject prefab = original as GameObject;

			if (poolDic.ContainsKey(prefab.name) == false) //한번도 오브젝트 풀이 만들어지지 않다면? 
			{
				CreatePool(prefab.name, prefab); //오브젝트 풀을 만들어 줌
			}

			GameObject obj = poolDic[prefab.name].Get();

			obj.transform.position = position;
			obj.transform.rotation = rotation;
			return obj as T;
		}
		else if (original is Component)
		{
			Component component = original as Component;
			string key = component.gameObject.name;

			if (poolDic.ContainsKey(key) == false)
			{
				CreatePool(key, component.gameObject); //오브젝트 풀을 만들어 줌
			}

			GameObject obj = poolDic[key].Get();

			obj.transform.position = position;
			obj.transform.rotation = rotation;
			return obj.GetComponent<T>();

		}
		else //그 외에는 null을 return함
		{
			return null; 
		}
	}

	public T Get<T>(T original) where T : Object
	{
		return Get(original, Vector3.zero, Quaternion.identity);
	}

	private void CreatePool(string key, GameObject prefab) 
	{
		ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
			createFunc: () =>
			{
				GameObject obj = Instantiate(prefab);
				obj.name = key;
				return obj;
			},
			actionOnGet: (GameObject obj) =>
			{
				obj.SetActive(true);
				obj.transform.parent = null;

			},
			actionOnRelease: (GameObject obj) =>
			{
				obj.SetActive(false);
				obj.transform.SetParent(transform);
			},
			actionOnDestroy: (GameObject obj) =>
			{
				Destroy(obj);
			}
		);

		poolDic.Add(prefab.name, pool); //prefab의 이름을 key로 사용해서 key오타로 인한 검색 누락의 위험을 제거함
	}

	public bool Release(GameObject prefab)
	{
		if (poolDic.ContainsKey(prefab.name) == false) //반납할 key가 없다면? 
		{
			return false; //반납 실패
		}
		
		poolDic[prefab.name].Release(prefab);
		return true;
	}

	public bool IsContain<T>(T original) where T : Object
	{
		if (original is GameObject)
		{
			GameObject prefab = original as GameObject;
			return poolDic.ContainsKey(prefab.name);
		}
		else if (original is Component)
		{
			Component component = original as Component;
			return poolDic.ContainsKey(component.gameObject.name);
		}
		return false;
	}
} 