using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RayCastFromCamera : MonoBehaviour
{
	public int money;
	[SerializeField] int Tower1Price0;
	[SerializeField] int Tower1Price1;
	[SerializeField] int Tower1Price2;
	[SerializeField] int Tower1Price3;
	private int price;

	public Camera camera;
	public Grid grid;
	public GameObject _tilePrefab;
	public GameObject testcube;
	public Material HoloMaterial;
	public Material InvisibleMaterial;
    public Material RedMaterial;
    GameObject temp;

	public TextMeshProUGUI TotalCash;
	public TextMeshProUGUI CurrentWave;
	public GameObject TowerStatsCanva;

	//tower details
	public TextMeshProUGUI TowerName;
	public TextMeshProUGUI TowerLevel;
	public TextMeshProUGUI TowerType;

	public Slider ExpSlider;
	public TextMeshProUGUI TowerExperience;

    public Slider DamageSlider;
    public TextMeshProUGUI TowerDamage;

    public Slider ElementalDamageSlider;
    public TextMeshProUGUI TowerElementalDamage;

    public Slider DamageOverTimeSlider;
    public TextMeshProUGUI TowerDamageOverTime;

    public Slider SpeedSlider;
    public TextMeshProUGUI TowerSpeed;

    public Slider RangeSlider;
    public TextMeshProUGUI TowerRange;

    public Slider DPSSlider;
    public TextMeshProUGUI TowerDPS;

    public TextMeshProUGUI TowerUpgradePrice;
	public Button TowerUpgrade;

    public TextMeshProUGUI TowerSellIncome;
    public Button TowerSell;
	//end

	public Vector3 cordinate;

	public GameObject[] Towers;
	public GameObject[] HoloTowers;
	public GameObject[] SelectedTowerImage;
	Transform TowerArea;
	public int ActiveTower = -1;
	public bool HologramTower = false;
	//public GameObject towerHolo = null;
	public bool Visualize = false;
	GameObject tower;

    Color col = new Color(1, 0.9565783f, 0.3632075f);
    Color upgradecol = new Color(1, 0.4903689f, 0.2216981f);
    public TowerStats ts = null;
	void Start()
	{
		TowerStatsCanva.SetActive(false);
		temp = Instantiate(_tilePrefab, new Vector3(0.5f, 0, 0.5f), Quaternion.identity);
		if (camera == null)
		{
			camera = Camera.main;
		}
	}


	void Update()
	{
		TotalCash.text = "CASH:   "+money.ToString();
        if (ts != null)
        {
            ShowTowerInfo();
        }
        if (!EventSystem.current.IsPointerOverGameObject())
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				Vector3 hitPoint = hit.point;
				Vector3Int gridPosition = grid.WorldToCell(hitPoint);
				cordinate = grid.GetCellCenterWorld(gridPosition);
				cordinate.y = 0f;
				temp.transform.position = cordinate;  //Przesuwaj klocek (kursor)

				PlaceTower(cordinate);  //postaw hologram jezeli klikniesz guzik

				if (tower != null)  //jezeli jest wieza to przesuwaj tam gdzie kursor i pokaz obszar
				{
					tower.transform.position = cordinate;
					TowerArea = tower.transform.Find("area");
					TowerArea.GetComponent<MeshRenderer>().material = HoloMaterial;
				}

				if (hit.collider.CompareTag("chunk"))
				{
					//POSTAWIENIE WIEZY PO NACISNIECIU LPM
					if (Input.GetMouseButtonDown(0))
					{
						if (tower != null)
						{
                            money -= price;
                            tower.transform.position = cordinate;
							TowerArea.GetComponent<MeshRenderer>().material = InvisibleMaterial;
							tower.GetComponentInChildren<TowerStats>().hologram = false;
							tower.transform.Find("Particle Build").GetComponent<ParticleSystem>().Play();
							tower.layer = LayerMask.NameToLayer("Default");
							tower = null;
							HologramTower = false;
						}

						//jezeli klikasz lpmw mape a wieza jest zaznaczona wyczysc dane oraz obszar
						TowerAreaInvisible();
						if (ts != null)
						{
							ts = null;
							TowerStatsCanva.SetActive(false);
						}
					}
				}
				else
				{
					if (tower != null)
					{
                        TowerArea.GetComponent<MeshRenderer>().material = RedMaterial;
                    }

                 }
                    //klikasz prawym = resetujesz wybor wiezy (hologramu)
                    if (Input.GetMouseButtonDown(1))
				{
					ResetSelectedTower();
					TowerStatsCanva.SetActive(false);
				}

				if (hit.collider.CompareTag("tower") && !HologramTower)
				{
					if (Input.GetMouseButtonDown(0))
					{
						TowerAreaInvisible();

						TowerStatsCanva.SetActive(true);
						ts = hit.collider.gameObject.transform.Find("towerInfo").GetComponent<TowerStats>();

						TowerArea = hit.collider.gameObject.transform.Find("area");
						TowerArea.GetComponent<MeshRenderer>().material = HoloMaterial;
					}
				}
			}
		}
	}

	public void SetWave(int wave)
	{
		CurrentWave.text = wave.ToString();
		LeanTween.cancel(CurrentWave.gameObject);
		CurrentWave.transform.localScale = Vector3.one;
		float s = 1.2f;
		float t = 0.2f;
		LeanTween.scale(CurrentWave.gameObject, new Vector3(s, s, s), t).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(CurrentWave.gameObject, new Vector3(1, 1, 1), t).setEase(LeanTweenType.easeInOutSine).setDelay(t);
    }
    public void ShowInfo()
	{
		//name / tier
        TowerName.text = ts.GetName();
        TowerLevel.text = "TIER " + (ts.GetLevel() + 1).ToString();



        //exp
        ExpSlider.minValue = 0;
        ExpSlider.maxValue = ts.GetMaxExp();
        ExpSlider.value = ts.GetExperience();
        TowerExperience.text = ts.GetExperience() + " / " + ts.GetMaxExp();



		//type
        TowerType.text = "type: " + ts.GetType();



        //damage
        TowerDamage.text = ts.GetDamage().ToString();
        DamageSlider.value = ts.GetDamage();
        DamageSlider.maxValue = 100;
        DamageSlider.transform.Find("damage upgrade Slider").GetComponent<Slider>().value = 0;
        DamageSlider.transform.Find("damage upgrade Slider").GetComponent<Slider>().maxValue = 100;
        if (Visualize && ts.DamageUpgrade > 0)
		{
            TowerDamage.text = ts.GetDamage() + " + " + ts.DamageUpgrade;
			DamageSlider.transform.Find("damage upgrade Slider").GetComponent<Slider>().value = ts.GetDamage() + ts.DamageUpgrade;
        }



        //elemental damage
        TowerElementalDamage.text = ts.GetElementalDamage().ToString();
        ElementalDamageSlider.value = ts.GetElementalDamage();
        ElementalDamageSlider.maxValue = 100;
        ElementalDamageSlider.transform.Find("elemental damage upgrade Slider").GetComponent<Slider>().value = 0;
        ElementalDamageSlider.transform.Find("elemental damage upgrade Slider").GetComponent<Slider>().maxValue = 100;
        if (Visualize && ts.ElementalUpgrade > 0)
        {
			TowerElementalDamage.text = ts.GetElementalDamage() + " + " + ts.ElementalUpgrade;
            ElementalDamageSlider.transform.Find("elemental damage upgrade Slider").GetComponent<Slider>().value = ts.GetElementalDamage() + ts.ElementalUpgrade;
        }



        //damage over time
        TowerDamageOverTime.text = ts.GetDamageOverTime().ToString();
        DamageOverTimeSlider.value = ts.GetDamageOverTime();
        DamageOverTimeSlider.maxValue = 100;
        DamageOverTimeSlider.transform.Find("damage over time upgrade Slider").GetComponent<Slider>().value = 0;
        DamageOverTimeSlider.transform.Find("damage over time upgrade Slider").GetComponent<Slider>().maxValue = 100;
        if (Visualize && ts.DamageOverTimeUpgrade > 0)
        {
            TowerDamageOverTime.text = ts.GetDamageOverTime() + " + " + ts.DamageOverTimeUpgrade;
            DamageOverTimeSlider.transform.Find("damage over time upgrade Slider").GetComponent<Slider>().value = ts.GetDamageOverTime() + ts.DamageOverTimeUpgrade;
        }



        //attack speed
        TowerSpeed.text = ts.GetAttackSpeed().ToString();
        SpeedSlider.value = 3 - ts.GetAttackSpeed();
        SpeedSlider.maxValue = 3;
        SpeedSlider.transform.Find("attack speed upgrade Slider").GetComponent<Slider>().value = 0;
        SpeedSlider.transform.Find("attack speed upgrade Slider").GetComponent<Slider>().maxValue = 3;
        if (Visualize && ts.SpeedUpgrade > 0)
        {
            TowerSpeed.text = ts.GetAttackSpeed() + " - " + ts.SpeedUpgrade;
            SpeedSlider.transform.Find("attack speed upgrade Slider").GetComponent<Slider>().value = 3 - ts.GetAttackSpeed() + ts.SpeedUpgrade;
        }



        //range
        TowerRange.text = ts.GetRange().ToString();
        RangeSlider.value = ts.GetRange();
        RangeSlider.maxValue = 15;
        RangeSlider.transform.Find("range upgrade Slider").GetComponent<Slider>().value = 0;
        RangeSlider.transform.Find("range upgrade Slider").GetComponent<Slider>().maxValue = 15;
        if (Visualize && ts.RangeUpgrade > 0)
        {
            TowerRange.text = ts.GetRange() + " + " + ts.RangeUpgrade;
            RangeSlider.transform.Find("range upgrade Slider").GetComponent<Slider>().value = ts.GetRange() + ts.RangeUpgrade;
        }
    }


	void ShowTowerInfo()
	{
        TowerUpgrade.onClick.RemoveAllListeners();
		TowerSell.onClick.RemoveAllListeners();
		TowerSell.onClick.AddListener(ts.Sell);
        TowerStatsCanva.SetActive(true);
			
		ShowInfo();


        TowerSellIncome.text = "for " + ts.GetSellIncome().ToString();
		if (ts.GetLevel() < 3)
		{
			TowerUpgrade.onClick.AddListener(ts.Upgrade);
			TowerUpgradePrice.text = ts.GetUpgradePrice().ToString();
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE";
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            TowerUpgrade.interactable = true;
        }
		else
		{
            ColorBlock cb = TowerUpgrade.colors;
            cb.disabledColor = Color.yellow;
            TowerUpgrade.colors = cb;

			TowerUpgrade.interactable = false;
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MAX";
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
            
    }

	void ResetSelectedTower()
	{
		ActiveTower = -1;
		HologramTower = false;
		Destroy(tower);
		foreach (GameObject go in SelectedTowerImage)
		{
			go.SetActive(false);
		}
	}
	void TowerAreaInvisible()
	{
		if (TowerArea != null)
		{
			TowerArea.GetComponent<MeshRenderer>().material = InvisibleMaterial;
		}
	}
	public void PlaceTower(Vector3 cordinate)
	{
		if (ActiveTower == 0 && money >= Towers[0].transform.Find("tower stats").GetComponent<TowerStats>().GetUpgradePrice())
		{
			/*
			money -= Towers[0].transform.Find("tower stats").GetComponent<TowerStats>().GetUpgradePrice();
			ResetSelectedTower();
			tower = Instantiate(Towers[0], cordinate, Quaternion.identity);
			tower.layer = LayerMask.NameToLayer("Ignore Raycast");
			*/
		}

		if (ActiveTower == 1 && money >= Towers[1].GetComponentInChildren<TowerStats>().GetUpgradePrice())
		{
			//money -= Tower1Price1;
			//price = Tower1Price1;
			price = Towers[1].GetComponentInChildren<TowerStats>().GetUpgradePrice();
			ResetSelectedTower();
			TowerAreaInvisible();
            tower = Instantiate(Towers[1], cordinate, Quaternion.identity);
            ts = tower.GetComponentInChildren<TowerStats>();
            tower.GetComponentInChildren<TowerStats>().hologram = true;
			HologramTower = true;
			tower.layer = LayerMask.NameToLayer("Ignore Raycast");
		}

		if (ActiveTower == 2 && money >= Tower1Price2)
		{
			money -= Tower1Price2;
			ResetSelectedTower();
			tower = Instantiate(Towers[2], cordinate, Quaternion.identity);
		}

		if (ActiveTower == 3 && money >= Tower1Price3)
		{
			money -= Tower1Price3;
			ResetSelectedTower();
			tower = Instantiate(Towers[3], cordinate, Quaternion.identity);
		}
	}
}
