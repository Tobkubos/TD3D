using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RayCastFromCamera : MonoBehaviour
{
	public int money;
	public int lives;
	private int price;
	public int ChunkPrice;

	public Camera camera;
	public Grid grid;
	public GameObject _tilePrefab;
	public GameObject testcube;
	public Material HoloMaterial;
	public Material InvisibleMaterial;
    public Material RedMaterial;
    GameObject temp;

	public TextMeshProUGUI TotalCash;
	public TextMeshProUGUI TotalLives;
	public TextMeshProUGUI CurrentWave;
	public GameObject TowerStatsCanva;

	//tower details
	public TextMeshProUGUI TowerName;
	public TextMeshProUGUI TowerLevel;
	public TextMeshProUGUI TowerType;

	public Slider ExpSlider;
	public TextMeshProUGUI TowerExperience;
	public TextMeshProUGUI TowerExperienceGainInfo;

	public GameObject DamageInfo;
    public Slider DamageSlider;
    public TextMeshProUGUI TowerDamage;

	public GameObject ElementalDamageInfo;
    public Slider ElementalDamageSlider;
    public TextMeshProUGUI TowerElementalDamage;

	public GameObject DamageOverTimeInfo;
    public Slider DamageOverTimeSlider;
    public TextMeshProUGUI TowerDamageOverTime;

	public GameObject SpeedInfo;
    public Slider SpeedSlider;
    public TextMeshProUGUI TowerSpeed;

	public GameObject RangeInfo;
    public Slider RangeSlider;
    public TextMeshProUGUI TowerRange;

    public GameObject RangeSupportInfo;
    public Slider RangeSupportSlider;
    public TextMeshProUGUI TowerRangeSupport;

    public TextMeshProUGUI TowerDescription;

    public TextMeshProUGUI TowerUpgradePrice;
	public Button TowerUpgrade;

    public TextMeshProUGUI TowerSellIncome;
    public Button TowerSell;
    //

    public Vector3 cordinate;

	public GameObject[] Towers;
    public GameObject[] TowersStartupSetup;
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
		TotalCash.text = "CASH:   " + money.ToString();
		TotalLives.text = lives.ToString();

		if (ts != null)
		{
			//Debug.Log(ts.GetName());
		}
		//Debug.Log(ActiveTower);
		//GAME OVER
		if (lives <= 0)
		{
			SceneManager.LoadScene(0);
		}


		if (ActiveTower != -1)
		{
			ts = TowersStartupSetup[ActiveTower].GetComponentInChildren<TowerStats>();
		}


        if (ts != null)
        {
            ShowTowerInfo();
        }
		else
		{
			TowerStatsCanva.SetActive(false);
		}
		


        if (!EventSystem.current.IsPointerOverGameObject())
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;


            if (Input.GetMouseButtonDown(1))
            {
                TowerAreaInvisible();
                ResetSelectedTower();
                ts = null;
            }

			if (Physics.Raycast(ray, out hit))
			{
				if (!temp.active)
				{
					temp.SetActive(true);
				}

				if ( tower!= null && !tower.active)
				{
					tower.SetActive(true);
				}
				Vector3 hitPoint = hit.point;
				Vector3Int gridPosition = grid.WorldToCell(hitPoint);
				cordinate = grid.GetCellCenterWorld(gridPosition);
				cordinate.y = 0f;
				temp.transform.position = cordinate;  //Przesuwaj klocek (kursor)

				PlaceTower(cordinate, ActiveTower);  //postaw hologram jezeli klikniesz guzik

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
							ts = tower.GetComponentInChildren<TowerStats>();
							tower.transform.Find("Particle Build").GetComponent<ParticleSystem>().Play();
							tower.layer = LayerMask.NameToLayer("Default");
							tower = null;
							HologramTower = false;
						}

						//jezeli klikasz lpm w mape a wieza jest zaznaczona wyczysc dane oraz obszar
						TowerAreaInvisible();

						if (ts != null)
						{
							ts = null;
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


				if (hit.collider.CompareTag("tower") && !HologramTower)
				{
					if (Input.GetMouseButtonDown(0))
					{
						TowerAreaInvisible();
						ActiveTower = -1;

						//TowerStatsCanva.SetActive(true);
						ts = hit.collider.gameObject.transform.Find("towerInfo").GetComponent<TowerStats>();

						TowerArea = hit.collider.gameObject.transform.Find("area");
						TowerArea.GetComponent<MeshRenderer>().material = HoloMaterial;
					}
				}
			}
			else
			{
				temp.SetActive(false);
				if (tower != null)
				{
					tower.SetActive(false);
				}
			}
		}
	}
    string FormatExperience(float value)
    {
        if (Mathf.Abs(value - Mathf.Floor(value)) < Mathf.Epsilon)
        {
            return value.ToString("F0");
        }
        else
        {
            return value.ToString("F1");
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
		int maxValue = 30; 
        TowerStatsCanva.SetActive(true);
        //name / tier
        TowerName.text = ts.GetName();

		if (!ts.Support)
		{
			TowerLevel.text = "TIER " + (ts.GetLevel() + 1).ToString();
		}


		//exp
		if (ts.Support) 
		{
            ExpSlider.gameObject.SetActive(false);
			TowerExperience.gameObject.SetActive(false);
			TowerExperienceGainInfo.gameObject.SetActive(false);
        }
		else
		{
            ExpSlider.gameObject.SetActive(true);
            TowerExperience.gameObject.SetActive(true);
			TowerExperienceGainInfo.gameObject.SetActive(true);
            ExpSlider.minValue = 0;
			ExpSlider.maxValue = ts.GetMaxExp();
			ExpSlider.value = ts.GetExperience();

			TowerExperience.text = FormatExperience(ts.GetExperience()) + " / " + ts.GetMaxExp();
		}


		//type
        TowerType.text = "type: " + ts.GetType();



		//damage
		if (ts.GetDamage() != 0)
		{
            DamageInfo.SetActive(true);
            TowerDamage.text = ts.GetDamage().ToString();
			DamageSlider.value = ts.GetDamage();
			DamageSlider.maxValue = maxValue;
			DamageSlider.transform.Find("damage upgrade Slider").GetComponent<Slider>().value = 0;
			DamageSlider.transform.Find("damage upgrade Slider").GetComponent<Slider>().maxValue = maxValue;
			if (Visualize && ts.DamageUpgrade > 0)
			{
				TowerDamage.text = ts.GetDamage() + " + " + ts.DamageUpgrade;
				DamageSlider.transform.Find("damage upgrade Slider").GetComponent<Slider>().value = ts.GetDamage() + ts.DamageUpgrade;
			}
		}
		else{
			DamageInfo.SetActive(false);
		}

		//elemental damage
		if (ts.GetElementalDamage() != 0)
		{
			ElementalDamageInfo.SetActive(true);
			TowerElementalDamage.text = ts.GetElementalDamage().ToString();
			ElementalDamageSlider.value = ts.GetElementalDamage();
			ElementalDamageSlider.maxValue = maxValue;
			ElementalDamageSlider.transform.Find("elemental damage upgrade Slider").GetComponent<Slider>().value = 0;
			ElementalDamageSlider.transform.Find("elemental damage upgrade Slider").GetComponent<Slider>().maxValue = maxValue;
			if (Visualize && ts.ElementalUpgrade > 0)
			{
				TowerElementalDamage.text = ts.GetElementalDamage() + " + " + ts.ElementalUpgrade;
				ElementalDamageSlider.transform.Find("elemental damage upgrade Slider").GetComponent<Slider>().value = ts.GetElementalDamage() + ts.ElementalUpgrade;
			}
		}
		else
		{
			ElementalDamageInfo.SetActive(false);
		}


		//damage over time
		if (ts.GetDamageOverTime() != 0)
		{
			DamageOverTimeInfo.SetActive(true);
			TowerDamageOverTime.text = ts.GetDamageOverTime().ToString();
			DamageOverTimeSlider.value = ts.GetDamageOverTime();
			DamageOverTimeSlider.maxValue = maxValue;
			DamageOverTimeSlider.transform.Find("damage over time upgrade Slider").GetComponent<Slider>().value = 0;
			DamageOverTimeSlider.transform.Find("damage over time upgrade Slider").GetComponent<Slider>().maxValue = maxValue;
			if (Visualize && ts.DamageOverTimeUpgrade > 0)
			{
				TowerDamageOverTime.text = ts.GetDamageOverTime() + " + " + ts.DamageOverTimeUpgrade;
				DamageOverTimeSlider.transform.Find("damage over time upgrade Slider").GetComponent<Slider>().value = ts.GetDamageOverTime() + ts.DamageOverTimeUpgrade;
			}
		}
		else
		{
			DamageOverTimeInfo.SetActive(false);
		}


		//attack speed
		if (ts.GetAttackSpeed() != 0)
		{
			SpeedInfo.SetActive(true);
			TowerSpeed.text = ts.GetAttackSpeed().ToString();
			SpeedSlider.value = 4 - ts.GetAttackSpeed();
			SpeedSlider.maxValue = 4;
			SpeedSlider.transform.Find("attack speed upgrade Slider").GetComponent<Slider>().value = 0;
			SpeedSlider.transform.Find("attack speed upgrade Slider").GetComponent<Slider>().maxValue = 4;
			if (Visualize && ts.SpeedUpgrade > 0)
			{
				TowerSpeed.text = ts.GetAttackSpeed() + " - " + ts.SpeedUpgrade;
				SpeedSlider.transform.Find("attack speed upgrade Slider").GetComponent<Slider>().value = 4 - ts.GetAttackSpeed() + ts.SpeedUpgrade;
			}
		}
		else
		{
			SpeedInfo.SetActive(false);
		}


		//range
		if (ts.GetRange() != 0)
		{
			RangeInfo.SetActive(true);
			TowerRange.text = ts.GetRange().ToString();
			RangeSlider.value = ts.GetRange();
			RangeSlider.maxValue = 10;
			RangeSlider.transform.Find("range upgrade Slider").GetComponent<Slider>().value = 0;
			RangeSlider.transform.Find("range upgrade Slider").GetComponent<Slider>().maxValue = 10;
			if (Visualize && ts.RangeUpgrade > 0)
			{
				TowerRange.text = ts.GetRange() + " + " + ts.RangeUpgrade;
				RangeSlider.transform.Find("range upgrade Slider").GetComponent<Slider>().value = ts.GetRange() + ts.RangeUpgrade;
			}
		}
		else
		{
			RangeInfo.SetActive(false);
		}

		TowerDescription.text = ts.Description;


    }


	void ShowTowerInfo()
	{
        TowerUpgrade.onClick.RemoveAllListeners();
		TowerSell.onClick.RemoveAllListeners();

		
			
		ShowInfo();


		if (!ts.hologram)
		{
			TowerSell.onClick.AddListener(ts.Sell);
		}

        TowerSellIncome.text = "for " + ts.GetSellIncome().ToString();
		if (!ts.Support && ts.GetLevel() < 3)
		{
			TowerUpgrade.onClick.AddListener(ts.Upgrade);
			TowerUpgradePrice.text = ts.GetUpgradePrice().ToString();
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE";
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            TowerUpgrade.interactable = true;
        }
		if(ts.GetLevel() >= 3)
		{
            ColorBlock cb = TowerUpgrade.colors;
            cb.disabledColor = Color.yellow;
            TowerUpgrade.colors = cb;

			TowerUpgrade.interactable = false;
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MAX";
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
			ExpSlider.maxValue = 1;
            ExpSlider.value = 1;
            TowerExperience.text = "MAX";
        }

		if (ts.Support)
		{

		}
            
    }

	void ResetSelectedTower()
	{
	    ActiveTower = -1;
		ts = null;
		HologramTower = false;
		Destroy(tower);
	}
	void TowerAreaInvisible()
	{
		if (TowerArea != null)
		{
			TowerArea.GetComponent<MeshRenderer>().material = InvisibleMaterial;
		}
	}
	public void PlaceTower(Vector3 cordinate, int towerindex)
	{
		if (ActiveTower!=-1 && money >= Towers[towerindex].GetComponentInChildren<TowerSetupParams>().Price)
		{
            price = Towers[towerindex].GetComponentInChildren<TowerSetupParams>().Price;
            ResetSelectedTower();
            TowerAreaInvisible();
            tower = Instantiate(Towers[towerindex], cordinate, Quaternion.identity);
            ts = tower.GetComponentInChildren<TowerStats>();
            tower.GetComponentInChildren<TowerStats>().hologram = true;
            HologramTower = true;
            tower.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
	}
}
