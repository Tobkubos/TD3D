using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
	public TowerStats towerStats;
	private void OnTriggerStay(Collider collision)
	{

		if (towerStats.hologram)
		{
			return;
		}

		if (!towerStats.Support && collision.gameObject.CompareTag("enemy"))
		{
			if (!towerStats.EnemiesInRange.Contains(collision.gameObject))
			{
				towerStats.OnEnemyEnterRange(collision.gameObject);
			}
		}
		
	
		if (towerStats.Support && collision.gameObject.CompareTag("tower") && !collision.gameObject.GetComponentInChildren<TowerStats>().hologram)
		{
			// Sprawdzenie, czy wie¿a nie jest w tablicy aby nie dodawaæ jej wielokrotnie
			if (!towerStats.TowersInRange.Contains(collision.gameObject))
			{
				if (!collision.gameObject.GetComponentInChildren<TowerStats>().SupportingTowers.Contains(towerStats.TowerObject))
				{
					collision.gameObject.GetComponentInChildren<TowerStats>().OnSupportEnterRange(towerStats.TowerObject);
					collision.gameObject.GetComponentInChildren<TowerStats>().CheckSupports();
					//Debug.Log("support sprawdza");
				}
                towerStats.TowersInRange.Add(collision.gameObject);
                //Debug.Log("WIE¯A!!!");
            }
			//Debug.Log("aaaa");
		}
		

	}

    private void OnTriggerExit(Collider collision)
	{
		if (!towerStats.Support && collision.gameObject.CompareTag("enemy"))
		{
			towerStats.OnEnemyExitRange(collision.gameObject);
		}

		
		if (towerStats.Support && collision.gameObject.CompareTag("tower"))
		{
			collision.gameObject.GetComponentInChildren<TowerStats>().OnSupportExitRange(towerStats.TowerObject);
			towerStats.TowersInRange.Remove(collision.gameObject);
			//Debug.Log("NI MA!!!");
		}
		
    }
}