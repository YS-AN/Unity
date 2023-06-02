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
			//GameManager.Pool.Release�� true�� pooling�� ������Ʈ��. -> �ݳ���
			if (GameManager.Pool.Release(obj))
				return;
		}
	

		//Release�� false�� pooling�� �Ұ����ϱ� ������ �׳� ������
		GameObject.Destroy(obj);
	}
}
