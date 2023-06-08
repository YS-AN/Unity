using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildInGameUI : InGameUI
{
	public TowerPlace towerPlace;

	protected override void Awake()
	{
		base.Awake();

		buttons["Blocker"].onClick.AddListener(() => { GameManager.UI.CloseInGameUI(this); });

		buttons["BtnArchor"].onClick.AddListener(() => { BuildArcherTower(); });
		buttons["BtnCanon"].onClick.AddListener(() => { BuildCanonTower(); });
		buttons["BtnMage"].onClick.AddListener(() => { BuildMageTower(); });
	}

	public void BuildArcherTower()
	{
		BuildTower("Data/ArcherTowerData");
	}

	public void BuildCanonTower()
	{
		BuildTower("Data/CanonTowerData");
	}

	public void BuildMageTower()
	{
		BuildTower("Data/MageTowerData");
	}

	private void BuildTower(string path)
	{
		TowerData towerData = GameManager.Resource.Load<TowerData>(path);
		towerPlace.BuildTower(towerData);

		GameManager.UI.CloseInGameUI(this);
	}
}

