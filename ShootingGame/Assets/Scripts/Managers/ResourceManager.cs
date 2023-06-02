using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
	{
		if(pooling)
		{
			return GameManager.Pool.Get(original, position, rotation);
		}
		else
		{
			return Object.Instantiate(original, position, rotation);
		}
	}

	public void Destroy(GameObject obj)
	{
		if (GameManager.Pool.IsContain(obj))
		{
			//GameManager.Pool.Release가 true면 pooling한 오브젝트임. -> 반납함
			if (GameManager.Pool.Release(obj))
				return;
		}
	

		//Release가 false면 pooling이 불가능하기 때문에 그냥 삭제함
		GameObject.Destroy(obj);
	}
}
