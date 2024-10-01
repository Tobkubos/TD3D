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

    //support
    public TextMeshProUGUI price5;
    public TextMeshProUGUI price6;
    public TextMeshProUGUI price7;
    public TextMeshProUGUI price8;
    public TextMeshProUGUI price9;
    public TextMeshProUGUI price10;

    void Start()
    {
        rcfc = GetComponent<RayCastFromCamera>();
        price1.text = rcfc.TowersStartupSetup[0].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price2.text = rcfc.TowersStartupSetup[1].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price3.text = rcfc.TowersStartupSetup[2].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price4.text = rcfc.TowersStartupSetup[3].GetComponentInChildren<TowerSetupParams>().Price.ToString();

        price5.text = rcfc.TowersStartupSetup[4].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price6.text = rcfc.TowersStartupSetup[5].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price7.text = rcfc.TowersStartupSetup[6].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price8.text = rcfc.TowersStartupSetup[7].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price9.text = rcfc.TowersStartupSetup[8].GetComponentInChildren<TowerSetupParams>().Price.ToString();
        price10.text = rcfc.TowersStartupSetup[9].GetComponentInChildren<TowerSetupParams>().Price.ToString();
    }
}
