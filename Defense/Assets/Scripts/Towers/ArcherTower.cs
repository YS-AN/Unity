using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : Tower
{
	private void Awake()
	{
		Data = GameManager.Resource.Load<TowerData>("Data/ArcherTowerData");
	}

}
