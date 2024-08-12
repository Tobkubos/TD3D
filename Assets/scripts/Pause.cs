using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PauseScreen;
    public CameraMovement cm;
    public void PauseON()
    {
        cm.enabled = false;
        PauseScreen.SetActive(true);
        Time.timeScale = 0.1f;
    }

    public void PauseOFF()
    {
        cm.enabled = true;
        PauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

}
