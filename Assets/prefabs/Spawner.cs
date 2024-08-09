using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject enemy1;
    public GameObject enemy2;
    public float cooldown = 0.5f;

	public void Spawn()
	{
		StartCoroutine(Spawning());
	}
	public IEnumerator Spawning()
	{
		while (true)
		{
			yield return new WaitForSeconds(cooldown);
			Instantiate(enemy1, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(cooldown);
            Instantiate(enemy2, transform.position, Quaternion.identity);

        }
    }
}
