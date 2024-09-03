using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PauseScreen;
    public CameraMovement cm;
    public GameObject NewEnemyInfo;

    private void Start()
    {
        PauseScreen.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseON();
        }
    }
    public void PauseON()
    {
        Debug.Log("PAUZA przed :" + Time.timeScale);
        Time.timeScale = 0.1f;
        cm.enabled = false;
        PauseScreen.SetActive(true);
        Debug.Log("PAUZA po :" + Time.timeScale);
    }

    public void PauseOFF()
    {
        cm.enabled = true;
        PauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void PauseOFF2()
    {
        //Debug.Log("KLIKAM");
        NewEnemyInfo.SetActive(false);
        Time.timeScale = 1.0f;
    }

}
