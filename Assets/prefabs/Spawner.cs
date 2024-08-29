using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject cube;
    public GameObject bigChunk;
    public GameObject speeder;
    public GameObject ArmoredCone;

    [SerializeField] int wave;
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

    public IEnumerator Spawn(GameObject enemy, float interval, int number)
    {
        for (int i = 0; i < number; i++) 
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }


	public IEnumerator Spawning()
	{
		wave++;
        manager.GetComponent<RayCastFromCamera>().SetWave(wave);
        if (wave == 1)
		{
            StartCoroutine(Spawn(cube, 2f, 8));
        }

        if (wave == 2)
        {
            StartCoroutine(Spawn(cube, 1.5f, 12));
        }

        if (wave == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                StartCoroutine(Spawn(cube, 0.2f, 2));
                yield return new WaitForSeconds(2f);
            }
        }

        if (wave == 4)
        {
               StartCoroutine(Spawn(cube, 0.5f, 5));
            yield return new WaitForSeconds(5f);
            StartCoroutine(Spawn(cube, 0.5f, 5));
        }
        
        // NEW ENEMY - SPEEDER
        if (wave == 5)
        {
            StartCoroutine(Spawn(speeder, 1, 10));
        }

        if (wave == 6)
        {
            StartCoroutine(Spawn(cube, 0.8f, 20));
            StartCoroutine(Spawn(speeder, 1, 10));

        }

        if (wave == 7) 
        {
            for (int i = 0; i < 5; i++)
            {
                StartCoroutine(Spawn(speeder, 0.2f, 5));
                yield return new WaitForSeconds(3f);
            }
            StartCoroutine(Spawn(cube, 0.5f, 10));
        }

        if (wave == 8)
        {
            for (int i = 0; i < 7; i++)
            {
                StartCoroutine(Spawn(speeder, 0.2f, 7));
                StartCoroutine(Spawn(cube, 0.5f, 10));
                yield return new WaitForSeconds(10f);
            }
        }

        //NEW ENEMY - BIG CHUNK
        if (wave == 9)
        {
            StartCoroutine(Spawn(bigChunk, 5f, 10));
        }

        if (wave == 10)
        {
            StartCoroutine(Spawn(bigChunk, 2f, 3));
            yield return new WaitForSeconds(8);
            StartCoroutine(Spawn(cube, 0.2f, 30));

        }

        if (wave == 11)
        {
            StartCoroutine(Spawn(bigChunk, 4f, 10));
            StartCoroutine(Spawn(cube, 3f, 10));
            StartCoroutine(Spawn(speeder, 2f, 10));
        }
        
    }
}
