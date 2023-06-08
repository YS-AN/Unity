using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTower : Tower
{
	private void Awake()
	{
		Data = GameManager.Resource.Load<TowerData>("Data/CanonTowerData");
	}

}
