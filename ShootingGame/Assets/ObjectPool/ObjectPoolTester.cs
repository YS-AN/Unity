using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesingPattern
{
	public class ObjectPoolTester : MonoBehaviour
	{
		private ObjectPool objPool;

		private void Awake()
		{
			objPool = GetComponent<ObjectPool>();
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.Space))
			{
				Poolable poolable = objPool.Get();

				poolable.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)); //랜덥 위치에 배치
			}
		}
	}
}
