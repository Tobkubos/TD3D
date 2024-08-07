using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

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
	public bool Hologram = false;
	public GameObject towerHolo = null;
	private bool inspect = false;


	TowerStats ts = null;
	void Start()
	{
		temp = Instantiate(_tilePrefab, new Vector3(0.5f,0,0.5f), Quaternion.identity);
		if (camera == null)
		{
			camera = Camera.main;
		}
	}


	void Update()
	{
		TotalCash.text = money.ToString();


		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
		{

			Vector3 hitPoint = hit.point;
			Vector3Int gridPosition = grid.WorldToCell(hitPoint);
			cordinate = grid.GetCellCenterWorld(gridPosition);
			cordinate.y = 0f;
            temp.transform.position = cordinate;

            PlaceHoloTower(cordinate);

            if (hit.collider.CompareTag("chunk"))
            {
                if (Input.GetMouseButtonDown(0))
				{
					PlaceTower(cordinate);
					if (inspect) {
                        if (TowerArea != null)
                        {
                            TowerArea.GetComponent<MeshRenderer>().material = InvisibleMaterial;
                        }

                        if (ts != null)
                        {
                            ts = null;
                        }
                        inspect = false;
                        TowerStatsCanva.SetActive(false);
                    }
				}
                if (Input.GetMouseButtonDown(1))
                {
					ResetSelectedTower();
				}
			}


			if (hit.collider.CompareTag("tower"))
			{
                if (Input.GetMouseButtonDown(0) && !inspect)
				{
                    TowerStatsCanva.SetActive(true);
                    ts = hit.collider.gameObject.transform.Find("towerInfo").GetComponent<TowerStats>();

					TowerArea = hit.collider.gameObject.transform.Find("area");
					TowerArea.GetComponent<MeshRenderer>().material = HoloMaterial;
					inspect = true;
                }

				else if(Input.GetMouseButtonDown(0) && inspect)
                {
                    if (TowerArea != null)
                    {
                        TowerArea.GetComponent<MeshRenderer>().material = InvisibleMaterial;
                    }

                    if (ts != null)
                    {
                        ts = null;
                    }
					inspect = false;
					TowerStatsCanva.SetActive(false);
                }
				
            }


            if (ts != null)
            {
                TowerUpgrade.onClick.RemoveAllListeners();
                TowerStatsCanva.SetActive(true);
                TowerName.text = ts.GetName();
				TowerLevel.text = ts.GetLevel().ToString();
				Debug.Log(ts.GetLevel());


                ExpSlider.minValue = 0;
                ExpSlider.maxValue = ts.GetMaxExp();
                ExpSlider.value = ts.GetExperience();

                TowerType.text = ts.GetType();
                TowerDamage.text = ts.GetDamage().ToString();

                TowerUpgrade.onClick.AddListener(ts.Upgrade);
				TowerUpgradePrice.text = ts.GetUpgradePrice().ToString();
            }
        }
	}

	void ResetSelectedTower()
	{
		Hologram = false;
		ActiveTower = -1;
		Destroy(towerHolo);
		foreach (GameObject go in SelectedTowerImage)
		{
			go.SetActive(false);
		}
	}

	public void PlaceTower(Vector3 cordinate)
	{
		if (ActiveTower == 0 && money >= Tower1Price0)
		{
			money -= Tower1Price0;
			ResetSelectedTower();
			GameObject tower = Instantiate(Towers[0], cordinate, Quaternion.identity);
		}

		if (ActiveTower == 1 && money >= Tower1Price1)
		{
            money -= Tower1Price1;
            ResetSelectedTower();
            GameObject tower = Instantiate(Towers[1], cordinate, Quaternion.identity);
        }

		if (ActiveTower == 2 && money >= Tower1Price2)
		{
            money -= Tower1Price2;
            ResetSelectedTower();
            GameObject tower = Instantiate(Towers[2], cordinate, Quaternion.identity);
		}

		if (ActiveTower == 3 && money >= Tower1Price3)
		{
            money -= Tower1Price3;
            ResetSelectedTower();
            GameObject tower = Instantiate(Towers[3], cordinate, Quaternion.identity);
		}
	}
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
}
