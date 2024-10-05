using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Modifiers", menuName = "mods")]
public class ModifiersInfo : ScriptableObject
{

    public List<Mod> mod = new List<Mod>();

    public void SetModifier(float value, string modName)
    {

        foreach(Mod m in mod)
        {
            if(m.name == modName)
            {
                m.value = value;
                m.SetText();
                return;
            }
        }

        Mod newMod = new Mod();
        newMod.value = value;
        newMod.name = modName;
        newMod.SetText();
        mod.Add(newMod);
    }


    [System.Serializable]
    public class Mod
    {
        public string name;
        public float value;
        public string modText;

        public void SetText()
        {
            modText = name.ToString() + " +" + value.ToString();
        }
    }
}
