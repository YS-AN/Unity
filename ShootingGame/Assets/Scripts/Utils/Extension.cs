using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extension
{
	public static bool IsValid(this GameObject obj)
	{
		return obj != null && obj.activeInHierarchy; 
	}

	public static bool IsValid(this Component comp)
	{
		return comp != null && comp.gameObject.activeInHierarchy;
	}
}
