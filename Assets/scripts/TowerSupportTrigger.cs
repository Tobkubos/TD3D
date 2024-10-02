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

        //sprawdz czy jest null, jak support jest null to wyrzuc z listy i oblicz bonus ponownie, PRZERZUÆ TO DO SPRZEDANIA SUPPORTA!!!!!
        /*
        if (towerstats.Support == false && towerstats.SupportingTowers.Count > 0)
        {
            for (int i = towerstats.SupportingTowers.Count - 1; i >= 0; i--)
            {
                if (towerstats.SupportingTowers[i] == null)
                {
                    towerstats.SupportingTowers.RemoveAt(i);
                    towerstats.CheckSupports();
                }
            }
        }
        */


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
                    if (!towerstats.SupportingTowers.Contains(support.gameObject))
                    {
                        towerstats.SupportingTowers.Add(support.gameObject);
                        support.GetComponentInChildren<TowerStats>().TowersInRange.Clear();
                        support.GetComponentInChildren<TowerStats>().TowersInRange.Add(towerstats.TowerObject);
                    }
                }

                if (towerstats.SupportingTowers.Count > 0)
                {
                    towerstats.CheckSupports();
                }
            }
        }
    }
}
