using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PriceSetup : MonoBehaviour
{
    private RayCastFromCamera rcfc;

    public TextMeshProUGUI price1;
    public TextMeshProUGUI price2;
    public TextMeshProUGUI price3;
    public TextMeshProUGUI price4;


    void Start()
    {
        rcfc = GetComponent<RayCastFromCamera>();
        price1.text = rcfc.TowersStartupSetup[0].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price2.text = rcfc.TowersStartupSetup[1].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price3.text = rcfc.TowersStartupSetup[2].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price4.text = rcfc.TowersStartupSetup[3].GetComponentInChildren<TowerSetupParams>().Price.ToString();
    }
}
