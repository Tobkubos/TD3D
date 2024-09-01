using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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


    //enemy details and modifiers
    public GameObject NewEnemyInfo;
    public TextMeshProUGUI EnemyName;
    public TextMeshProUGUI EnemyDescription;

    public TextMeshProUGUI Modifiers;
    //

    private void Start()
    {
        NewEnemyInfo = GameObject.Find("New Enemy Info");
        Debug.Log(NewEnemyInfo);
        EnemyName = NewEnemyInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        EnemyDescription = NewEnemyInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        NewEnemyInfo.SetActive(false);
        manager = GameObject.Find("manager");

        wave = 0;
        Debug.Log("DUPA");
    }
    void ShowNewEnemyInfo(string name, string desc)
    {
        NewEnemyInfo.SetActive(true);
        Time.timeScale = 0.1f;
        EnemyName.text = name;
        EnemyDescription.text = desc;
    }

    public void Spawn()
	{
		StartCoroutine(WaveStart());
	}

    public void AutoWave()
    {
        if (AutomaticWave) {
            AutomaticWave = false;
            GameObject.Find("NEXT WAVE").GetComponent<Button>().interactable = true;
            GameObject.Find("NEXT WAVE").transform.Find("Auto wave info").gameObject.SetActive(false);
        }
        else if(!AutomaticWave){
            AutomaticWave=true;
            GameObject.Find("NEXT WAVE").transform.Find("Auto wave info").gameObject.SetActive(true);
            if (wave != 0)
            {
                GameObject.Find("NEXT WAVE").GetComponent<Button>().interactable = false;
            }
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
        GameObject.Find("NEXT WAVE").GetComponent<Button>().interactable = false;
        wave++;
        manager.GetComponent<RayCastFromCamera>().SetWave(wave);
        if (wave == 1)
		{
            ShowNewEnemyInfo(cube.GetComponent<EnemyInfo>().name, cube.GetComponent<EnemyInfo>().desc);
            yield return StartCoroutine(SpawnMonster(cube, 2f, 8));
            //cube.GetComponent<EnemyInfo>().ModifyHp(10);
        }


        if (wave == 2)
        {
            yield return StartCoroutine(SpawnMonster(cube, 1.5f, 12));
        }

        if (wave == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                yield return StartCoroutine(SpawnMonster(cube, 0.2f, 2));
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(2f);
        }

        if (wave == 4)
        {
            StartCoroutine(SpawnMonster(cube, 0.5f, 5));
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(SpawnMonster(cube, 0.5f, 5));
        }
        
        // NEW ENEMY - SPEEDER
        if (wave == 5)
        {
            ShowNewEnemyInfo(speeder.GetComponent<EnemyInfo>().name, speeder.GetComponent<EnemyInfo>().desc);
            yield return StartCoroutine(SpawnMonster(speeder, 1, 10));
        }

        if (wave == 6)
        {
            StartCoroutine(SpawnMonster(cube, 0.8f, 20));
            yield return StartCoroutine(SpawnMonster(speeder, 1, 10));

        }

        if (wave == 7) 
        {
            for (int i = 0; i < 5; i++)
            {
                StartCoroutine(SpawnMonster(speeder, 0.2f, 5));
                yield return new WaitForSeconds(3f);
            }
            yield return StartCoroutine(SpawnMonster(cube, 0.5f, 10));
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
            ShowNewEnemyInfo(bigChunk.GetComponent<EnemyInfo>().name, bigChunk.GetComponent<EnemyInfo>().desc);
            yield return StartCoroutine(SpawnMonster(bigChunk, 5f, 10));
        }

        if (wave == 10)
        {
            StartCoroutine(SpawnMonster(bigChunk, 2f, 3));
            yield return new WaitForSeconds(8);
            yield return StartCoroutine(SpawnMonster(cube, 0.2f, 30));

        }

        if (wave == 11)
        {
            StartCoroutine(SpawnMonster(bigChunk, 4f, 10));
            StartCoroutine(SpawnMonster(cube, 3f, 10));
            yield return StartCoroutine(SpawnMonster(speeder, 2f, 10));
        }

        if (wave == 12)
        {
            for (int i = 0; i < 6; i++)
            {
                StartCoroutine(SpawnMonster(bigChunk, 1f, 3));
                StartCoroutine(SpawnMonster(cube, 0.1f, 13));
                yield return StartCoroutine(SpawnMonster(speeder, 0.2f, 10));
                yield return new WaitForSeconds(3f);
            }
        }

        //NEW ENEMY - ARMORED CONE
        if (wave == 13)
        {
            for (int i = 0; i < 6; i++)
            {
                StartCoroutine(SpawnMonster(bigChunk, 1f, 3));
                StartCoroutine(SpawnMonster(cube, 0.1f, 13));
                yield return StartCoroutine(SpawnMonster(speeder, 0.2f, 10));
                yield return new WaitForSeconds(3f);
            }
        }

        if (AutomaticWave)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(WaveStart());
        }
        else
        {
            GameObject.Find("NEXT WAVE").GetComponent<Button>().interactable = true;
        }
    }
}
