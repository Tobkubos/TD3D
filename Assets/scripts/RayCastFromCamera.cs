using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
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


	public Camera camera;
	public Grid grid;
	public GameObject _tilePrefab;
	public GameObject testcube;
	public Material HoloMaterial;
	public Material InvisibleMaterial;
    public Material RedMaterial;
    GameObject temp;

	public TextMeshProUGUI TowerName;
	public TextMeshProUGUI TowerLevel;
	public TextMeshProUGUI TowerExperience;
	public TextMeshProUGUI TotalCash;

	public Slider ExpSlider;
	public Button TowerUpgrade;
	public GameObject TowerStatsCanva;

	public TextMeshProUGUI TowerType;
	public TextMeshProUGUI TowerDamage;
	public TextMeshProUGUI TowerUpgradePrice;
	public Vector3 cordinate;

	public GameObject[] Towers;
	public GameObject[] HoloTowers;
	public GameObject[] SelectedTowerImage;
	Transform TowerArea;
	public int ActiveTower = -1;
	public bool HologramTower = false;
	//public GameObject towerHolo = null;
	private bool inspect = false;
	GameObject tower;


	TowerStats ts = null;
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
	void ShowTowerInfo()
	{
            TowerUpgrade.onClick.RemoveAllListeners();
            TowerStatsCanva.SetActive(true);
            TowerName.text = ts.GetName();
            TowerLevel.text = ts.GetLevel().ToString();
            //Debug.Log(ts.GetLevel());


            ExpSlider.minValue = 0;
            ExpSlider.maxValue = ts.GetMaxExp();
            ExpSlider.value = ts.GetExperience();

            TowerType.text = ts.GetType();
            TowerDamage.text = ts.GetDamage().ToString();

            TowerUpgrade.onClick.AddListener(ts.Upgrade);
            TowerUpgradePrice.text = ts.GetUpgradePrice().ToString();
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
		if (ActiveTower == 0 && money >= Tower1Price0)
		{
			money -= Tower1Price0;
			ResetSelectedTower();
			tower = Instantiate(Towers[0], cordinate, Quaternion.identity);
			tower.layer = LayerMask.NameToLayer("Ignore Raycast");
		}

		if (ActiveTower == 1 && money >= Tower1Price1)
		{
			money -= Tower1Price1;
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
		/*
		public void PlaceHoloTower(Vector3 cordinate)
		{
			if (towerHolo != null)
			{
				towerHolo.transform.position = cordinate;
			}

			if (ActiveTower == 0 && !Hologram)
			{
				Hologram = true;
				towerHolo = Instantiate(HoloTowers[0], cordinate, Quaternion.identity);
				towerHolo.transform.position = cordinate;
			}

			if (ActiveTower == 1 && !Hologram)
			{
				Hologram = true;
				towerHolo = Instantiate(HoloTowers[1], cordinate, Quaternion.identity);
				towerHolo.transform.position = cordinate;
			}

			if (ActiveTower == 2 && !Hologram)
			{
				Hologram = true;
				towerHolo = Instantiate(HoloTowers[2], cordinate, Quaternion.identity);
				towerHolo.transform.position = cordinate;
			}

			if (ActiveTower == 3 && !Hologram)
			{
				Hologram = true;
				towerHolo = Instantiate(HoloTowers[3], cordinate, Quaternion.identity);
				towerHolo.transform.position = cordinate;
			}
		}
		*/
