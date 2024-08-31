using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
	public GameObject cube;
    public GameObject bigChunk;
    public GameObject speeder;
    public GameObject ArmoredCone;

    private bool AutomaticWave = false;

    [SerializeField] int wave;
    private GameObject manager;
    private void Start()
    {
        wave = 0;
    }
    public void Spawn()
	{
        manager = GameObject.Find("manager");
        
		StartCoroutine(WaveStart());
	}

    public void AutoWave()
    {
        if (AutomaticWave) {
            AutomaticWave = false;
            GameObject.Find("NEXT WAVE").GetComponent<Button>().interactable = true;
        }
        else if(!AutomaticWave){
            AutomaticWave=true;
            GameObject.Find("NEXT WAVE").GetComponent<Button>().interactable = false;
        }

        Debug.LogWarning(AutomaticWave);
    }

    public IEnumerator SpawnMonster(GameObject enemy, float interval, int number)
    {
        for (int i = 0; i < number; i++) 
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }


	public IEnumerator WaveStart()
	{
		wave++;
        manager.GetComponent<RayCastFromCamera>().SetWave(wave);
        if (wave == 1)
		{
            yield return StartCoroutine(SpawnMonster(cube, 2f, 8));
        }

        if (wave == 2)
        {
            yield return StartCoroutine(SpawnMonster(cube, 1.5f, 12));
        }

        if (wave == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                StartCoroutine(SpawnMonster(cube, 0.2f, 2));
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(2f);
        }

        if (wave == 4)
        {
               StartCoroutine(SpawnMonster(cube, 0.5f, 5));
            yield return new WaitForSeconds(5f);
            StartCoroutine(SpawnMonster(cube, 0.5f, 5));
        }
        
        // NEW ENEMY - SPEEDER
        if (wave == 5)
        {
            StartCoroutine(SpawnMonster(speeder, 1, 10));
        }

        if (wave == 6)
        {
            StartCoroutine(SpawnMonster(cube, 0.8f, 20));
            StartCoroutine(SpawnMonster(speeder, 1, 10));

        }

        if (wave == 7) 
        {
            for (int i = 0; i < 5; i++)
            {
                StartCoroutine(SpawnMonster(speeder, 0.2f, 5));
                yield return new WaitForSeconds(3f);
            }
            StartCoroutine(SpawnMonster(cube, 0.5f, 10));
        }

        if (wave == 8)
        {
            for (int i = 0; i < 7; i++)
            {
                StartCoroutine(SpawnMonster(speeder, 0.2f, 7));
                StartCoroutine(SpawnMonster(cube, 0.5f, 10));
                yield return new WaitForSeconds(10f);
            }
        }

        //NEW ENEMY - BIG CHUNK
        if (wave == 9)
        {
            StartCoroutine(SpawnMonster(bigChunk, 5f, 10));
        }

        if (wave == 10)
        {
            StartCoroutine(SpawnMonster(bigChunk, 2f, 3));
            yield return new WaitForSeconds(8);
            StartCoroutine(SpawnMonster(cube, 0.2f, 30));

        }

        if (wave == 11)
        {
            StartCoroutine(SpawnMonster(bigChunk, 4f, 10));
            StartCoroutine(SpawnMonster(cube, 3f, 10));
            StartCoroutine(SpawnMonster(speeder, 2f, 10));
        }

        if (AutomaticWave)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(WaveStart());
        }
    }
}
