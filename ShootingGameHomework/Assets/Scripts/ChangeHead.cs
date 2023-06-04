using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHead : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> Hats;

	[SerializeField]
	private Transform Head;

	private void Start()
	{
		var hat = Hats[Random.Range(0, Hats.Count)];
		hat.transform.parent = Head;
	}
}
