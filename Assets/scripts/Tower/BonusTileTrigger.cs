using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTileTrigger : MonoBehaviour
{
    public TowerStats towerStats;
    public LayerMask mask;
    [SerializeField] Collider[] Objects;
    /*
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
    */

    private void Update()
    {
        if (towerStats.hologram)
        {
            Objects = null;
            Objects = Physics.OverlapBox(this.transform.position, new Vector3(0.2f, 1, 0.2f), Quaternion.identity, mask);
            //Debug.Log(area[0]);

            if(Objects.Length!=0)
            {
                if (!towerStats.Support)
                {
                    towerStats.BonusTile = Objects[0].gameObject;
                    towerStats.CheckSupports();
                }

            }
            else
            {
                towerStats.BonusTile = null;
                towerStats.CheckSupports();
            }
        }
    }
}
