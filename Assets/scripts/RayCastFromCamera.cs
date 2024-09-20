using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

    public GameObject DamageSupportInfo;
    public Slider DamageSupportSlider;
    public TextMeshProUGUI TowerDamageSupport;

    public GameObject ElementalDamageSupportInfo;
    public Slider ElementalDamageSupportSlider;
    public TextMeshProUGUI TowerElementalDamageSupport;

    public GameObject DamageOverTimeSupportInfo;
    public Slider DamageOverTimeSupportSlider;
    public TextMeshProUGUI TowerDamageOverTimeSupport;

    public GameObject SpeedSupportInfo;
    public Slider SpeedSupportSlider;
    public TextMeshProUGUI TowerSpeedSupport;

    public GameObject RangeSupportInfo;
    public Slider RangeSupportSlider;
    public TextMeshProUGUI TowerRangeSupport;

    public TextMeshProUGUI TowerDescription;

    public TextMeshProUGUI TowerUpgradePrice;
	public Button TowerUpgrade;
    public TextMeshProUGUI TowerUpgradeText;

    public TextMeshProUGUI TowerSellIncome;
    public Button TowerSell;

	public GameObject supportsBonus;
	public TextMeshProUGUI damageFromSupports;
    public TextMeshProUGUI elementalDamageFromSupports;
    public TextMeshProUGUI damageOverTimeFromSupports;
    public TextMeshProUGUI speedFromSupports;
	public TextMeshProUGUI rangeFromSupports;


    //
    public TextMeshProUGUI CursorInfotext;
    public GameObject CursorInfo;
    private BonusTile BonusTile;
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

    UnityEngine.Color col = new UnityEngine.Color(1, 0.9565783f, 0.3632075f);
    UnityEngine.Color upgradecol = new UnityEngine.Color(1, 0.4903689f, 0.2216981f);
    public TowerStats ts = null;
	void Start()
	{
        CursorInfo.SetActive(false);
        TowerStatsCanva.SetActive(false);
		temp = Instantiate(_tilePrefab, new Vector3(0.5f, 0, 0.5f), Quaternion.identity);
		if (camera == null)
		{
			camera = Camera.main;
		}
	}

	void ShowBonusTileInfo(BonusTile BonusTile)
	{
        CursorInfotext.text = "";
        CursorInfo.SetActive(true);
        if (BonusTile.Damage != 0)
        {
            CursorInfotext.text += "Physical Damage +" + BonusTile.Damage.ToString() + "\n";
        }
        if (BonusTile.ElementalDamage != 0)
        {
            CursorInfotext.text += "Elemental Damage +" + BonusTile.ElementalDamage.ToString() + "\n";
        }

        if (BonusTile.DamageOverTime != 0)
        {
            CursorInfotext.text += "Damage Over Time +" + BonusTile.DamageOverTime.ToString() + "\n";
        }

        if (BonusTile.Range != 0)
        {
            CursorInfotext.text += "Range +" + BonusTile.Range.ToString() + "\n";
        }

        if (BonusTile.Cooldown != 0)
        {
            CursorInfotext.text += "Cooldown +" + BonusTile.Cooldown.ToString() + "\n";
        }
    }

	void Update()
	{
		TotalCash.text = "CASH:   " + money.ToString();
		TotalLives.text = lives.ToString();

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
            if(ts.BonusTile != null)
            {
                BonusTile = ts.BonusTile.GetComponent<BonusTile>();
            }
        }
		else
		{
			TowerStatsCanva.SetActive(false);
		}


        if (BonusTile != null)
        {
            ShowBonusTileInfo(BonusTile);
        }
        else
        {
            CursorInfo.SetActive(false);
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
                //klocek poza plansza --> klocek na plansze --> aktywacja
				if (!temp.active)
				{
					temp.SetActive(true);
				}

                //wieza poza plansza --> wieza na plansze --> aktywacja
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

				if (hit.collider.CompareTag("bonusTile"))
				{
                    BonusTile = hit.collider.GetComponent<BonusTile>();
                }
                else
                {
                    BonusTile = null;
                }


				if (hit.collider.CompareTag("chunk") || hit.collider.CompareTag("bonusTile"))
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
							if (!ts.Support)
							{
								ts.CheckSupports();
							}
							if (ts.Support) 
							{
								foreach (GameObject tower in ts.TowersInRange) 
								{
									tower.GetComponentInChildren<TowerStats>().CheckSupports();
								}
							}
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


	public void ShowSlider(String ParameterName, Slider ParameterSlider, Slider UpgradeSlider, Slider AllSlider, TextMeshProUGUI ParameterTextValue, TextMeshProUGUI ParameterTextValueFromSupports,int maxValue, int Parameter, int FinalParameter, int UpgradeParameter, int ParameterFromSupports)
	{
        ParameterSlider.maxValue = maxValue;
        UpgradeSlider.value = 0;
        UpgradeSlider.maxValue = maxValue;

        if (ParameterFromSupports == 0)
        {
            ParameterTextValue.text = Parameter.ToString();
            ParameterTextValueFromSupports.gameObject.SetActive(false);
            AllSlider.value = 0;
        }
        if (ParameterFromSupports != 0)
        {
            supportsBonus.SetActive(true);
            ParameterTextValue.text = FinalParameter.ToString();
            ParameterTextValueFromSupports.gameObject.SetActive(true);
            ParameterTextValueFromSupports.text = ParameterName + "+" + ParameterFromSupports.ToString();
            AllSlider.value = FinalParameter;
            AllSlider.maxValue = maxValue;
        }

        ParameterSlider.value = Parameter;
		if(FinalParameter > 0)
		{
			AllSlider.value = FinalParameter;
            AllSlider.maxValue = maxValue;
            ParameterTextValue.text = FinalParameter.ToString();
        }

        if (Visualize && UpgradeParameter > 0)
        {
            if (ParameterFromSupports == 0)
            {
                ParameterTextValue.text = FinalParameter.ToString() + " + " + UpgradeParameter;
            }
            else
            {
                ParameterTextValue.text = FinalParameter.ToString() + " + " + UpgradeParameter;
            }
            UpgradeSlider.value = Parameter + UpgradeParameter;
            AllSlider.value = FinalParameter + UpgradeParameter;
        }
    }
    public void ShowSlider(String ParameterName, Slider ParameterSlider, Slider UpgradeSlider, Slider AllSlider, TextMeshProUGUI ParameterTextValue, TextMeshProUGUI ParameterTextValueFromSupports, int maxValue, float Parameter, float FinalParameter, float UpgradeParameter, float ParameterFromSupports)
    {
        ParameterSlider.maxValue = maxValue;
        UpgradeSlider.value = 0;
        UpgradeSlider.maxValue = maxValue;

        if (ParameterFromSupports == 0)
        {
            ParameterTextValue.text = Parameter.ToString();
            ParameterTextValueFromSupports.gameObject.SetActive(false);
            AllSlider.value = 0;
        }
        if (ParameterFromSupports != 0)
        {
            supportsBonus.SetActive(true);
            ParameterTextValue.text = FinalParameter.ToString();
            ParameterTextValueFromSupports.gameObject.SetActive(true);
            ParameterTextValueFromSupports.text = ParameterName + "+" + ParameterFromSupports.ToString();
            AllSlider.value = FinalParameter;
            AllSlider.maxValue = maxValue;
        }

        ParameterSlider.value = Parameter;
        if (FinalParameter > 0)
        {
            AllSlider.value = FinalParameter;
            AllSlider.maxValue = maxValue;
            ParameterTextValue.text = FinalParameter.ToString();
        }
        if (Visualize && UpgradeParameter > 0)
        {
            if (ParameterFromSupports == 0)
            {
                ParameterTextValue.text = FinalParameter.ToString() + " + " + UpgradeParameter;
            }
            else
            {
                ParameterTextValue.text = FinalParameter.ToString() + " + " + UpgradeParameter;
            }
            UpgradeSlider.value = Parameter + UpgradeParameter;
            AllSlider.value = FinalParameter + UpgradeParameter;
        }
    }
    public void ShowSpeed(String ParameterName, Slider ParameterSlider, Slider UpgradeSlider, Slider AllSlider, TextMeshProUGUI ParameterTextValue, TextMeshProUGUI ParameterTextValueFromSupports, int maxValue, float Parameter, float FinalParameter, float UpgradeParameter, float ParameterFromSupports)
    {
        ParameterSlider.maxValue = maxValue;
        UpgradeSlider.value = 0;
        UpgradeSlider.maxValue = maxValue;

        if (ParameterFromSupports == 0)
        {
            ParameterTextValue.text = Parameter.ToString();
            ParameterTextValueFromSupports.gameObject.SetActive(false);
            AllSlider.value = 0;
        }
        if (ParameterFromSupports != 0)
        {
            supportsBonus.SetActive(true);
            ParameterTextValue.text = FinalParameter.ToString();
            ParameterTextValueFromSupports.gameObject.SetActive(true);
            ParameterTextValueFromSupports.text = ParameterName + "-" + ParameterFromSupports.ToString();
            AllSlider.value = maxValue - FinalParameter;
            AllSlider.maxValue = maxValue;
        }

        ParameterSlider.value = maxValue - Parameter;
        if (FinalParameter > 0)
        {
            AllSlider.value = maxValue - FinalParameter;
            AllSlider.maxValue = maxValue;
            ParameterTextValue.text = FinalParameter.ToString();
        }
        if (Visualize && UpgradeParameter > 0)
        {
            if (ParameterFromSupports == 0)
            {
                ParameterTextValue.text = FinalParameter.ToString() + " - " + UpgradeParameter;
            }
            else
            {
                ParameterTextValue.text = FinalParameter.ToString() + " - " + UpgradeParameter;
            }
            UpgradeSlider.value = maxValue - Parameter + UpgradeParameter;
            AllSlider.value = maxValue - FinalParameter + UpgradeParameter;
        }
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
        else
        {
            TowerLevel.text = "";
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

        /*
		if(ts.DamageFromSupports == 0)
		{
			damageFromSupports.gameObject.SetActive(false);
		}
		else
		{
			damageFromSupports.gameObject.SetActive(true);
		}
        */

		//damage

		damageFromSupports.gameObject.SetActive(false);
        elementalDamageFromSupports.gameObject.SetActive(false);
        damageOverTimeFromSupports.gameObject.SetActive(false);
        speedFromSupports.gameObject.SetActive(false);
        rangeFromSupports.gameObject.SetActive(false);
		supportsBonus.SetActive(false);

        if (ts.GetDamage() != 0)
		{
			DamageInfo.SetActive(true);
			Slider AllSlider = DamageSlider.transform.Find("All Damage Slider").GetComponent<Slider>();
			Slider UpgradeSlider = DamageSlider.transform.Find("damage upgrade Slider").GetComponent<Slider>();
            ShowSlider("Damage: ", DamageSlider, UpgradeSlider, AllSlider, TowerDamage, damageFromSupports, 30, ts.GetDamage(), ts.FinalDamage, ts.DamageUpgrade, ts.DamageFromSupports);
		}
		else 
		{
			DamageInfo.SetActive(false);
		}

        if (ts.GetElementalDamage() != 0)
        {
            ElementalDamageInfo.SetActive(true);
            Slider AllSlider = ElementalDamageSlider.transform.Find("All ElementalDamage Slider").GetComponent<Slider>();
            Slider UpgradeSlider = ElementalDamageSlider.transform.Find("elemental damage upgrade Slider").GetComponent<Slider>();
            ShowSlider("Elemental Damage: ", ElementalDamageSlider, UpgradeSlider, AllSlider, TowerElementalDamage, elementalDamageFromSupports, 30, ts.GetElementalDamage(), ts.FinalElementalDamage, ts.ElementalUpgrade, ts.ElementalDamageFromSupports);
        }
        else
        {
            ElementalDamageInfo.SetActive(false);
        }

        if (ts.GetDamageOverTime() != 0)
        {
            DamageOverTimeInfo.SetActive(true);
            Slider AllSlider = DamageOverTimeSlider.transform.Find("All DamageOverTime Slider").GetComponent<Slider>();
            Slider UpgradeSlider = DamageOverTimeSlider.transform.Find("damage over time upgrade Slider").GetComponent<Slider>();

            ShowSlider("Damage Over Time: ", DamageOverTimeSlider, UpgradeSlider, AllSlider, TowerDamageOverTime, damageOverTimeFromSupports, 30, ts.GetDamageOverTime(), ts.FinalDamageOverTime, ts.DamageOverTimeUpgrade, ts.DamageOverTimeFromSupports);
        }
        else
        {
            DamageOverTimeInfo.SetActive(false);
        }

        if (ts.GetAttackSpeed() != 0)
        {
            SpeedInfo.SetActive(true);
            Slider AllSlider = SpeedSlider.transform.Find("All Speed Slider").GetComponent<Slider>();
            Slider UpgradeSlider = SpeedSlider.transform.Find("attack speed upgrade Slider").GetComponent<Slider>();

            ShowSpeed("Cooldown: ", SpeedSlider, UpgradeSlider, AllSlider, TowerSpeed, speedFromSupports, 4, ts.GetAttackSpeed(), ts.FinalCooldown, ts.SpeedUpgrade, ts.CooldownFromSupports);
        }
        else
        {
            SpeedInfo.SetActive(false);
        }

        if (ts.GetRange() != 0 && ts.Support == false)
        {
            RangeInfo.SetActive(true);
            Slider AllSlider = RangeSlider.transform.Find("All Range Slider").GetComponent<Slider>();
            Slider UpgradeSlider = RangeSlider.transform.Find("range upgrade Slider").GetComponent<Slider>();

            ShowSlider("Range: ", RangeSlider, UpgradeSlider, AllSlider, TowerRange, rangeFromSupports, 15, ts.GetRange(), ts.FinalRange, ts.RangeUpgrade, ts.RangeFromSupports);
        }
        else
        {
            RangeInfo.SetActive(false);
        }

        //SUPPORT DAMAGE
        if (ts.DamageSupport != 0)
		{
			DamageSupportInfo.SetActive(true);
			DamageSupportSlider.value = ts.DamageSupport;
            DamageSupportSlider.maxValue = 3;
            TowerDamageSupport.text = ts.DamageSupport.ToString();
		}
		else
		{
			DamageSupportInfo.SetActive(false);
		}

        //SUPPORT ELEMENTAL DAMAGE
        if (ts.ElementalDamageSupport != 0)
        {
            ElementalDamageSupportInfo.SetActive(true);
            ElementalDamageSupportSlider.value = ts.ElementalDamageSupport;
            ElementalDamageSupportSlider.maxValue = 3;
            TowerElementalDamageSupport.text = ts.ElementalDamageSupport.ToString();
        }
        else
        {
            ElementalDamageSupportInfo.SetActive(false);
        }

        //SUPPORT DAMAGE OVER TIME
        if (ts.DamageOverTimeSupport != 0)
        {
            DamageOverTimeSupportInfo.SetActive(true);
            DamageOverTimeSupportSlider.value = ts.DamageOverTimeSupport;
            DamageOverTimeSupportSlider.maxValue = 3;
            TowerDamageOverTimeSupport.text = ts.DamageOverTimeSupport.ToString();
        }
        else
        {
            DamageOverTimeSupportInfo.SetActive(false);
        }

        //RANGE DAMAGE
        if (ts.RangeSupport != 0)
        {
            RangeSupportInfo.SetActive(true);
            RangeSupportSlider.value = ts.RangeSupport;
            RangeSupportSlider.maxValue = 1;
            TowerRangeSupport.text = ts.RangeSupport.ToString();
        }
        else
        {
            RangeSupportInfo.SetActive(false);
        }

        //SPEED DAMAGE
        if (ts.CooldownSupport != 0)
        {
            SpeedSupportInfo.SetActive(true);
            SpeedSupportSlider.value = ts.CooldownSupport;
            SpeedSupportSlider.maxValue = 0.2f;
            TowerSpeedSupport.text = ts.CooldownSupport.ToString();
        }
        else
        {
            SpeedSupportInfo.SetActive(false);
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
            TowerUpgrade.gameObject.SetActive(true);
            TowerUpgrade.onClick.AddListener(ts.Upgrade);
			TowerUpgradePrice.text = ts.GetUpgradePrice().ToString();
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE";
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = UnityEngine.Color.black;
            TowerUpgrade.interactable = true;
        }
		if(ts.GetLevel() >= 3)
		{
            ColorBlock cb = TowerUpgrade.colors;
            cb.disabledColor = UnityEngine.Color.yellow;
            TowerUpgrade.colors = cb;

			TowerUpgrade.interactable = false;
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MAX";
            TowerUpgrade.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = UnityEngine.Color.white;
			ExpSlider.maxValue = 1;
            ExpSlider.value = 1;
            TowerExperience.text = "MAX";

            TowerUpgrade.gameObject.SetActive(false);
        }

		if (ts.Support)
		{
            TowerUpgrade.gameObject.SetActive(false);
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
