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

    List<GameObject> Enemies = new List<GameObject>();

    public GameObject Tier1Boss;

    private bool AutomaticWave = false;

    [SerializeField] int wave;
    private GameObject manager;
    bool isWaveActive;

    //enemy details and modifiers
    public GameObject NewEnemyInfo;
    public TextMeshProUGUI EnemyName;
    public TextMeshProUGUI EnemyDescription;

    EnemyInfo cubeInfo;

    //
    public Modifiers Modifiers;
    private void Start()
    {
        Modifiers = GameObject.Find("modifiers").GetComponent<Modifiers>();
        NewEnemyInfo = GameObject.Find("New Enemy Info");
        //Debug.Log(NewEnemyInfo);
        EnemyName = NewEnemyInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        EnemyDescription = NewEnemyInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        NewEnemyInfo.SetActive(false);
        manager = GameObject.Find("manager");

        cubeInfo = cube.GetComponent<EnemyInfo>();

        wave = 0;
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
        if (AutomaticWave)
        {
            AutomaticWave = false;

            GameObject.Find("NEXT WAVE").transform.Find("Auto wave info").gameObject.SetActive(false);
        }
        else if (!AutomaticWave)
        {
            AutomaticWave = true;
            GameObject.Find("NEXT WAVE").transform.Find("Auto wave info").gameObject.SetActive(true);
        }

        if (isWaveActive)
        {
            GameObject.Find("NEXT WAVE").GetComponent<Button>().interactable = false;
        }
        else {
            GameObject.Find("NEXT WAVE").GetComponent<Button>().interactable = true;
        }

        //Debug.LogWarning(AutomaticWave);
    }

    public IEnumerator SpawnMonster(GameObject enemy, float interval, int number, int level)
    {
        for (int i = 0; i < number; i++) 
        {
            GameObject monster = Instantiate(enemy, transform.position, Quaternion.identity);
            monster.GetComponent<EnemyInfo>().Init(level);
            Enemies.Add(monster);
            yield return new WaitForSeconds(interval);
        }
    }

	public IEnumerator WaveStart()
	{
        isWaveActive = true;
        GameObject.Find("NEXT WAVE").GetComponent<Button>().interactable = false;
        if (wave < 20)
        {
            wave++;
            manager.GetComponent<RayCastFromCamera>().SetWave(wave);
            if (wave == 1)
            {
                ShowNewEnemyInfo(cube.GetComponent<EnemyInfo>().name, cube.GetComponent<EnemyInfo>().desc);
                yield return StartCoroutine(SpawnMonster(cube, 2f, 8, 0));
            }

            
            if (wave == 2)
            {
                //Modifiers.modsInfo.SetModifier(cubeInfo.setupParameters.param[1].hp - cubeInfo.setupParameters.param[0].hp, "cube HP");
                //Modifiers.modsInfo.SetModifier(cubeInfo.setupParameters.param[1].speed - cubeInfo.setupParameters.param[0].speed, "cube SPEED");
                //Modifiers.modsInfo.SetModifier(cubeInfo.setupParameters.param[1].cash - cubeInfo.setupParameters.param[0].cash, "cube CASH");
                //Modifiers.ShowMods();
                yield return StartCoroutine(SpawnMonster(cube, 1.5f, 12, 0));
            }

            
            if (wave == 3)
            {
                for (int i = 0; i < 4; i++)
                {
                    yield return StartCoroutine(SpawnMonster(cube, 0.2f, 2, 0));
                    yield return new WaitForSeconds(2f);
                }
                yield return new WaitForSeconds(2f);
            }

            if (wave == 4)
            {
                StartCoroutine(SpawnMonster(cube, 0.5f, 5, 0));
                yield return new WaitForSeconds(5f);
                yield return StartCoroutine(SpawnMonster(cube, 0.5f, 5, 0));
            }

            // NEW ENEMY - SPEEDER
            if (wave == 5)
            {
                ShowNewEnemyInfo(speeder.GetComponent<EnemyInfo>().name, speeder.GetComponent<EnemyInfo>().desc);
                yield return StartCoroutine(SpawnMonster(speeder, 1, 10, 0));
            }

            if (wave == 6)
            {
                StartCoroutine(SpawnMonster(cube, 0.8f, 20, 0));
                yield return StartCoroutine(SpawnMonster(speeder, 1, 10, 0));

            }

            if (wave == 7)
            {
                for (int i = 0; i < 5; i++)
                {
                    StartCoroutine(SpawnMonster(speeder, 0.2f, 5, 0));
                    yield return new WaitForSeconds(3f);
                }
                yield return StartCoroutine(SpawnMonster(cube, 0.5f, 10, 0));
            }

            if (wave == 8)
            {
                for (int i = 0; i < 7; i++)
                {
                    StartCoroutine(SpawnMonster(speeder, 0.2f, 7, 0));
                    StartCoroutine(SpawnMonster(cube, 0.5f, 10, 0));
                    yield return new WaitForSeconds(10f);
                }
            }

            //NEW ENEMY - BIG CHUNK
            if (wave == 9)
            {
                ShowNewEnemyInfo(bigChunk.GetComponent<EnemyInfo>().name, bigChunk.GetComponent<EnemyInfo>().desc);
                yield return StartCoroutine(SpawnMonster(bigChunk, 5f, 6, 0));
            }

            if (wave == 10)
            {
                StartCoroutine(SpawnMonster(bigChunk, 2f, 3, 0));
                yield return new WaitForSeconds(8);
                yield return StartCoroutine(SpawnMonster(cube, 0.2f, 30, 0));

            }

            if (wave == 11)
            {
                StartCoroutine(SpawnMonster(bigChunk, 4f, 10, 0));
                StartCoroutine(SpawnMonster(cube, 3f, 10, 0));
                yield return StartCoroutine(SpawnMonster(speeder, 2f, 10, 0));
            }

            if (wave == 12)
            {
                for (int i = 0; i < 6; i++)
                {
                    StartCoroutine(SpawnMonster(bigChunk, 1f, 3, 0));
                    StartCoroutine(SpawnMonster(cube, 0.3f, 13, 0));
                    yield return StartCoroutine(SpawnMonster(speeder, 0.2f, 10, 0));
                    yield return new WaitForSeconds(3f);
                }
            }

            //NEW ENEMY - ARMORED CONE
            if (wave == 13)
            {
                ShowNewEnemyInfo(ArmoredCone.GetComponent<EnemyInfo>().name, ArmoredCone.GetComponent<EnemyInfo>().desc);
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(SpawnMonster(cube, 0.3f, 13, 0));
                    StartCoroutine(SpawnMonster(ArmoredCone, 0.4f, 8, 0));
                    yield return new WaitForSeconds(3f);
                }
            }

            if (wave == 14)
            {
                StartCoroutine(SpawnMonster(cube, 0.5f, 50, 0));
                yield return StartCoroutine(SpawnMonster(ArmoredCone, 0.4f, 10, 0));
            }

            if (wave == 15)
            {
                StartCoroutine(SpawnMonster(bigChunk, 1f, 10, 0));
                yield return StartCoroutine(SpawnMonster(ArmoredCone, 0.4f, 20, 0));
            }

            if (wave == 16)
            {
                StartCoroutine(SpawnMonster(bigChunk, 1, 20, 0));
                StartCoroutine(SpawnMonster(speeder, 0.7f, 30, 0));
                yield return StartCoroutine(SpawnMonster(ArmoredCone, 0.2f, 20, 0));
            }

            if (wave == 17)
            {
                StartCoroutine(SpawnMonster(cube, 0.4f, 45, 0));
                StartCoroutine(SpawnMonster(bigChunk, 1f, 15, 0));
                StartCoroutine(SpawnMonster(speeder, 0.7f, 25, 0));
                yield return StartCoroutine(SpawnMonster(ArmoredCone, 0.25f, 25, 0));
            }

            if (wave == 18)
            {
                StartCoroutine(SpawnMonster(cube, 0.4f, 50, 0));
                StartCoroutine(SpawnMonster(bigChunk, 1f, 20, 0));
                StartCoroutine(SpawnMonster(speeder, 0.4f, 35, 0));
                yield return StartCoroutine(SpawnMonster(ArmoredCone, 0.3f, 30, 0));
            }

            if (wave == 19)
            {
                StartCoroutine(SpawnMonster(cube, 0.4f, 80, 0));
                StartCoroutine(SpawnMonster(bigChunk, 0.8f, 40, 0));
                StartCoroutine(SpawnMonster(speeder, 0.4f, 55, 0));
                yield return StartCoroutine(SpawnMonster(ArmoredCone, 0.2f, 40, 0));
            }

            if (wave == 20)
            {
                ShowNewEnemyInfo(Tier1Boss.GetComponent<EnemyInfo>().name, Tier1Boss.GetComponent<EnemyInfo>().desc);
                StartCoroutine(SpawnMonster(Tier1Boss, 0.3f, 1,0));
            }
            

            CheckAndStartNextWave();




        }
    }

    private void CheckAndStartNextWave()
    {
        StartCoroutine(CheckAndStartNextWaveCoroutine());
    }

    private IEnumerator CheckAndStartNextWaveCoroutine()
    {
        while (Enemies.Count > 0)
        {
            Enemies.RemoveAll(enemy => enemy == null);  // Remove destroyed enemies
            yield return new WaitForSeconds(1f);
        }
        if (AutomaticWave)
        {
            //yield return new WaitForSeconds(2f);
            //StartCoroutine(WaveStart());
        StartCoroutine(WaveStart());
        }
        else
        {
            isWaveActive = false;
            GameObject.Find("NEXT WAVE").GetComponent<Button>().interactable = true;
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (Enemies.Contains(enemy))
        {
            Enemies.Remove(enemy);
        }
    }
}
