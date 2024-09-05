using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
	public TowerStats towerStats;

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("enemy"))
		{
			towerStats.OnEnemyEnterRange(collision.gameObject);
		}
	}

	private void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.CompareTag("enemy"))
		{
			towerStats.OnEnemyExitRange(collision.gameObject);
		}
	}
}