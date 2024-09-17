using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTrigger : MonoBehaviour
{
    public GameObject enemyObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("end"))
        {
            GameObject.Find("manager").GetComponent<RayCastFromCamera>().lives--;
            Destroy(enemyObject);
        }
    }
}
