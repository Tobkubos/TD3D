using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelector : MonoBehaviour
{
	[SerializeField] int TowerIndex = 0;
	public RayCastFromCamera rcfc;

	public void SelectTower()
	{
		rcfc.ActiveTower = TowerIndex;

		foreach(GameObject go in rcfc.SelectedTowerImage)
		{
			go.SetActive(false);
		}
		rcfc.SelectedTowerImage[TowerIndex].SetActive(true);
		if(rcfc.towerHolo != null)
		{
			rcfc.Hologram = false;
			Destroy(rcfc.towerHolo);
			rcfc.PlaceHoloTower(rcfc.cordinate);
		}

	}
}
