using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeRoad : MonoBehaviour
{
	[SerializeField] private float moveSpeed;

	private void Start()
	{
		Destroy(this.gameObject, 5f);
	}

	private void Update()
	{
		transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
	}
}
