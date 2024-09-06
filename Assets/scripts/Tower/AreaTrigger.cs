using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
	public TowerStats towerStats;

	private void OnTriggerEnter(Collider collision)
	{
		if (!towerStats.Support && collision.gameObject.CompareTag("enemy"))
		{
			towerStats.OnEnemyEnterRange(collision.gameObject);
		}

		if (towerStats.Support && collision.gameObject.CompareTag("tower"))
		{
			collision.gameObject.GetComponentInChildren<TowerStats>().SupportingTowers.Add(towerStats.gameObject);
			towerStats.TowersInRange.Add(collision.gameObject);
			Debug.Log("WIE¯A!!!");
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
			collision.gameObject.GetComponentInChildren<TowerStats>().SupportingTowers.Remove(towerStats.gameObject);
			towerStats.TowersInRange.Remove(collision.gameObject);
			Debug.Log("NI MA!!!");
		}
	}
}