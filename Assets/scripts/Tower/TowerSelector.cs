using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelector : MonoBehaviour
{
	[SerializeField] int TowerIndex = 0;
	public RayCastFromCamera rcfc;

    public void SelectTower()
	{
		rcfc.ActiveTower = TowerIndex;
	}
}
