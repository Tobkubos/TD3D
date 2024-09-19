using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaTrigger : MonoBehaviour
{
    public EnemyInfo ei;
    private void OnTriggerStay(Collider collision)
    {
        /*
        if (!ei.BossAttack)
        {
            return;
        }
        */

        if (collision!=null && collision.gameObject.CompareTag("tower") 
            && collision.gameObject.GetComponentInChildren<TowerStats>().hologram == false && collision.gameObject.GetComponentInChildren<TowerStats>().Support == false
            )
        {
            Debug.Log("DUPA DUPA DUPA DUPA");
            if (!ei.BossTowersToAttack.Contains(collision.gameObject))
            {
                ei.BossTowersToAttack.Add(collision.gameObject);
            }
        }
    }
}
