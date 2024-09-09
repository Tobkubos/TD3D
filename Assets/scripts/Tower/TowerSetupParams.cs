using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSetupParams: MonoBehaviour
{
    [Header("Tower")]
    public int Damage;
    public int ElementalDamage;
    public int DamageOverTime;
    public float Speed;
    public float Range;
    public int MaxExp;
    public int Price;

    [Header("Support")]
    public int DamageSupport;
    public int ElementalDamageSupport;
    public int DamageOverTimeSupport;
    public float SpeedSupport;
    public float RangeSupport;

}
