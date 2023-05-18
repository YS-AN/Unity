using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleCam : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObj;

	private void LateUpdate()
	{
        this.gameObject.transform.rotation = targetObj.transform.rotation;
    }
}
