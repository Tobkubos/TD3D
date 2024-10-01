using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject Controls;
    public Canvas canvas;
    public GameObject trans;
    private void Start()
    {
        Time.timeScale = 1.0f;
        if (Controls != null)
        {
            Controls.transform.localScale = Vector3.zero;
            Controls.SetActive(false);
        }
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameAnim());
    }
    public IEnumerator StartGameAnim()
    {
        LeanTween.value(0, canvas.GetComponent<RectTransform>().rect.width, 0.4f).setEase(LeanTweenType.easeInOutSine).setOnUpdate((float value) => {
            RectTransform transRect = trans.GetComponent<RectTransform>();
            transRect.sizeDelta = new Vector2(value, transRect.sizeDelta.y);
        });
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    public void ShowControls()
    {
        Controls.SetActive(true);
        LeanTween.scale(Controls, Vector3.one, 0.2f);
    }
}
