using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RayCastFromCamera : MonoBehaviour
{
	public Camera camera;
	public Grid grid;
	public GameObject _tilePrefab;
	public GameObject testcube;
	GameObject temp;
	private Vector3 savedPos;

	public TextMeshProUGUI TowerName;
	public TextMeshProUGUI TowerLevel;
	public TextMeshProUGUI TowerExperience;
	public TextMeshProUGUI TowerType;
	public TextMeshProUGUI TowerDamage;

	public GameObject[] Towers;
	public GameObject[] SelectedTowerImage;
	public int ActiveTower;
	void Start()
	{
		temp = Instantiate(_tilePrefab, new Vector3(0,0,0), Quaternion.identity);

		if (camera == null)
		{
			camera = Camera.main;
		}
	}

	void Update()
	{

		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;


		if (Physics.Raycast(ray, out hit))
		{
			Vector3 hitPoint = hit.point;
			Vector3Int gridPosition = grid.WorldToCell(hitPoint);
			Vector3 cordinate = grid.GetCellCenterWorld(gridPosition);
			cordinate.y = 0.1f;
			temp.transform.position = cordinate;

			if (hit.collider.CompareTag("chunk"))
			{
				if (Input.GetMouseButtonDown(0))
				{
					if ( ActiveTower == 0)
					{
						Instantiate(Towers[0], cordinate, Quaternion.identity);
					}

					if (ActiveTower == 1)
					{
						Instantiate(Towers[1], cordinate, Quaternion.identity);
					}

					if (ActiveTower == 2)
					{
						Instantiate(Towers[2], cordinate, Quaternion.identity);
					}

					if (ActiveTower == 3)
					{
						Instantiate(Towers[3], cordinate, Quaternion.identity);
					}
				}
			}


			if (hit.collider.CompareTag("tower"))
			{
				if (Input.GetMouseButtonDown(0))
				{
					TowerStats ts = hit.collider.gameObject.transform.Find("tower").GetComponent<TowerStats>();
					if (ts != null)
					{
						TowerName.text		 = ts.GetName();
						TowerLevel.text		 = ts.GetLevel().ToString();
						TowerExperience.text = ts.GetExperience().ToString();
						TowerType.text		 = ts.GetType();
						TowerDamage.text	 = ts.GetDamage().ToString();
					}
				}
			}
		}
	}
}
