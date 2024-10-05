using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemySetupParameters", menuName = "EnemySetupParameters")]
public class EnemySetupParameters : ScriptableObject
{
    public Param[] param;
}



[System.Serializable]
public class Param 
{
    public float hp;
    public float speed;
    public int cash;
}

