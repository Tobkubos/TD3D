using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Modifiers : MonoBehaviour
{
    public ModifiersInfo modsInfo;
    public TextMeshProUGUI template;

    private void Start()
    {
        modsInfo.mod.Clear();
    }

    public void ShowMods()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (ModifiersInfo.Mod m in modsInfo.mod)
        {
            TextMeshProUGUI ModT = Instantiate(template, transform);
            ModT.text = m.modText;
        }
    }
}
