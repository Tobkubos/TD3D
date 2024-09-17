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

        if (collision.gameObject.CompareTag("tower") && collision.gameObject.GetComponent<TowerStats>().hologram == false && collision.gameObject.GetComponent<TowerStats>().Support == false)
        {
            Debug.Log("DUPA DUPA DUPA DUPA");
            if (!ei.BossTowersToAttack.Contains(collision.gameObject))
            {
                ei.BossTowersToAttack.Add(collision.gameObject);
            }
        }
    }
}
