using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject enemy;
	private void Start()
	{
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			Instantiate(enemy, transform.position, Quaternion.identity);
		}
	}
}
