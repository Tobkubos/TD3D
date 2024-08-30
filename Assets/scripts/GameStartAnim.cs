using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartAnim : MonoBehaviour
{
    public Canvas canvas;
    public GameObject trans;
    private void Start()
    {
        LeanTween.value(canvas.GetComponent<RectTransform>().rect.width,0 , 0.4f).setEase(LeanTweenType.easeInOutSine).setOnUpdate((float value) =>
        {
            RectTransform transRect = trans.GetComponent<RectTransform>();
            transRect.sizeDelta = new Vector2(value, transRect.sizeDelta.y);
        });
    }
}
