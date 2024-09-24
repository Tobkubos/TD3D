using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSupportTrigger : MonoBehaviour
{
    public TowerStats towerstats;
    [SerializeField] Collider[] Objects;
    public LayerMask mask;
    void Update()
    {
        if (towerstats.hologram)
        {
            Objects = null;
            Objects = Physics.OverlapBox(this.transform.position, new Vector3(1f, 1, 1f), Quaternion.identity, mask);

            foreach (Collider c in Objects)
            {
                Debug.Log(c.gameObject);
            }


            if (towerstats.Support == false)
            {
                towerstats.SupportingTowers.Clear();

                foreach (Collider support in Objects)
                {
                    towerstats.SupportingTowers.Add(support.gameObject);
                    support.GetComponentInChildren<TowerStats>().TowersInRange.Clear();
                    support.GetComponentInChildren<TowerStats>().TowersInRange.Add(towerstats.TowerObject);
                }

                if (towerstats.SupportingTowers.Count > 0)
                {
                    towerstats.CheckSupports();
                }
            }
        }
    }
}
