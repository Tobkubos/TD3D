using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowBossHP : MonoBehaviour
{
    public Slider Slider;
    public TextMeshPro text;
    
    void Update()
    {
        text.text = Slider.value.ToString() + " / " + Slider.maxValue.ToString();
    }
}
