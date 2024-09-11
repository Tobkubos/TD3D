using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTileTrigger : MonoBehaviour
{
    public TowerStats towerStats;
    private void OnTriggerStay(Collider collision)
    {
        if (towerStats.hologram)
        {
            return;
        }

        if (!towerStats.Support && collision.gameObject.CompareTag("bonusTile"))
        {
            if (towerStats.BonusTile != collision.gameObject)
            {
                towerStats.BonusTile = collision.gameObject;
                towerStats.CheckSupports();
            }
        }
    }
}
