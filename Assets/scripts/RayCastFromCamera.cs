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
	public Slider ExpSlider;

	public TextMeshProUGUI TowerType;
	public TextMeshProUGUI TowerDamage;
	public Vector3 cordinate;

	public GameObject[] Towers;
	public GameObject[] HoloTowers;
	public GameObject[] SelectedTowerImage;
	Transform TowerArea;
	public int ActiveTower = -1;
	public bool Hologram = false;
	public GameObject towerHolo = null;

	TowerStats ts;
	void Start()
	{
		temp = Instantiate(_tilePrefab, new Vector3(0,0,0), Quaternion.identity);
		TowerStats ts = null;
		if (camera == null)
		{
			camera = Camera.main;
		}
	}


	void Update()
	{

		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;


		if (Input.GetMouseButtonDown(0))
		{
			
			if (TowerArea != null)
			{
				TowerArea.GetComponent<MeshRenderer>().material = InvisibleMaterial;
			}

			if(ts != null)
			{
				ts = null;
			}
		}


		if (Physics.Raycast(ray, out hit))
		{

			Vector3 hitPoint = hit.point;
			Vector3Int gridPosition = grid.WorldToCell(hitPoint);
			cordinate = grid.GetCellCenterWorld(gridPosition);
			cordinate.y = 0f;
			
			PlaceHoloTower(cordinate);
			


            if (hit.collider.CompareTag("chunk"))
			{
                temp.transform.position = cordinate;
                Debug.Log(gridPosition);
                if (Input.GetMouseButtonDown(0))
				{
					PlaceTower(cordinate);
				}
			}


			if (hit.collider.CompareTag("tower"))
			{
				if (Input.GetMouseButtonDown(0))
				{
					ts = hit.collider.gameObject.transform.Find("towerInfo").GetComponent<TowerStats>();
					if (ts != null)
					{
						TowerName.text = ts.GetName();
						TowerLevel.text = ts.GetLevel().ToString();

						ExpSlider.minValue = 0;
						ExpSlider.maxValue = ts.GetMaxExp();
						ExpSlider.value = ts.GetExperience();

						TowerType.text = ts.GetType();
						TowerDamage.text = ts.GetDamage().ToString();
					}
					TowerArea = hit.collider.gameObject.transform.Find("area");
					TowerArea.GetComponent<MeshRenderer>().material = HoloMaterial;
				}
			}
		}

		if (ts != null)
		{
			TowerName.text = ts.GetName();
			TowerLevel.text = ts.GetLevel().ToString();

			ExpSlider.minValue = 0;
			ExpSlider.maxValue = ts.GetMaxExp();
			ExpSlider.value = ts.GetExperience();

			TowerType.text = ts.GetType();
			TowerDamage.text = ts.GetDamage().ToString();
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
		if (ActiveTower == 0)
		{
			ResetSelectedTower();
			GameObject tower = Instantiate(Towers[0], cordinate, Quaternion.identity);

		}

		if (ActiveTower == 1)
		{
			ResetSelectedTower();
            GameObject tower = Instantiate(Towers[1], cordinate, Quaternion.identity);
        }

		if (ActiveTower == 2)
		{
			ResetSelectedTower();
            GameObject tower = Instantiate(Towers[2], cordinate, Quaternion.identity);
		}

		if (ActiveTower == 3)
		{
			ResetSelectedTower();
            GameObject tower = Instantiate(Towers[3], cordinate, Quaternion.identity);
		}
	}
	public void PlaceHoloTower(Vector3 cordinate)
	{
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
		if (towerHolo != null)
		{
			towerHolo.transform.position = cordinate;
		}
	}
}
