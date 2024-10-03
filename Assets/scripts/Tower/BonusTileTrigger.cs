using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTileTrigger : MonoBehaviour
{
    public TowerStats towerStats;
    public LayerMask mask;
    [SerializeField] Collider[] Objects;
    private bool bonusChecked;

    private void Update()
    {
        if (towerStats.hologram)
        {
            CheckBonusTile();
        }
        if (bonusChecked == false)
        {
            bonusChecked = true;
            CheckBonusTile();
        }
    }


    private void CheckBonusTile()
    {
        Objects = null;
        Objects = Physics.OverlapBox(this.transform.position, new Vector3(0.2f, 1, 0.2f), Quaternion.identity, mask);
        //Debug.Log(area[0]);

        if (Objects.Length != 0)
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
