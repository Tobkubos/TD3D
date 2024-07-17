using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject enemy;
	public float cooldown = 0.5f;
	private void Start()
	{
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn()
	{
		while (true)
		{
			yield return new WaitForSeconds(cooldown);
			Instantiate(enemy, transform.position, Quaternion.identity);
		}
	}
}
