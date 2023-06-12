using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
	private Chair[] chairs;
	public Chair[] Chairs { get {  return chairs; } }

	private Table table;
	public Table Table { get { return table; } }

	private void Awake()
	{
		chairs = GetComponentsInChildren<Chair>();
		table = GetComponent<Table>();
	}
}
