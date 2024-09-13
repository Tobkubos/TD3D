using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTile : MonoBehaviour
{
    public int Damage;
    public int ElementalDamage;
    public int DamageOverTime;
    public float Range;
    public float Cooldown;

    private void Start()
    {
        int modifiers = Random.Range(1,3);

        int numOfModifiers = 0;
        List<int> UniqueNumbers = new List<int>();
        while (numOfModifiers < modifiers) 
        {
            int mod = Random.Range(0, 4);
            if (!UniqueNumbers.Contains(mod))
            {
                numOfModifiers++;
                UniqueNumbers.Add(mod);
                if(mod == 0)
                {
                    Damage = Random.Range(1, 3);
                }

                if (mod == 1)
                {
                    ElementalDamage = Random.Range(1, 3);
                }

                if (mod == 2)
                {
                    DamageOverTime = Random.Range(1, 3);
                }

                if (mod == 3)
                {
                    Range = Mathf.Round(Random.Range(0.2f, 1.5f) * 10f) / 10f;
                }

                if (mod == 4)
                {
                    Cooldown = Mathf.Round(Random.Range(0.1f, 0.3f) * 10f) / 10f; 
                }
            }
        }
    }
}
