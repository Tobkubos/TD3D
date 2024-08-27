using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject enemy1;
    public GameObject enemy2;
    public float cooldown = 0.5f;
    private int wave;
    private GameObject manager;
    private void Start()
    {
        wave = 0;
    }
    public void Spawn()
	{
        manager = GameObject.Find("manager");
        
		StartCoroutine(Spawning());
	}
	public IEnumerator Spawning()
	{
		wave++;
        manager.GetComponent<RayCastFromCamera>().SetWave(wave);
        if (wave == 1)
		{
			for (int i = 0; i < 10; i++)
			{
                yield return new WaitForSeconds(1);
                Instantiate(enemy1, transform.position, Quaternion.identity);
			}
		}

        if (wave == 2)
        {
            for (int i = 0; i < 20; i++)
            {
                yield return new WaitForSeconds(0.8f);
                Instantiate(enemy1, transform.position, Quaternion.identity);
            }
        }

        if (wave == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    yield return new WaitForSeconds(0.2f);
                    Instantiate(enemy1, transform.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(2f);
            }
        }


        if (wave == 4)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(enemy2, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(2f);
            }
        }
        // NEW ENEMY - SPEEDER
    }
}
