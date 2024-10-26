using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPScounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float fps;
    void Start()
    {
        StartCoroutine(FPSUpdate());
    }

    private void Update()
    {
        if (fps<60)
        {
            Time.timeScale = 1.0f;
        }
    }

    IEnumerator FPSUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            fps = 1f / Time.unscaledDeltaTime;
            fpsText.text = "FPS: " + Mathf.RoundToInt(fps);
        }
    }
}
