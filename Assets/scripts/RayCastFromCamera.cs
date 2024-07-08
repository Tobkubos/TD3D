using System.Collections;
using System.Collections.Generic;
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
			if (hit.collider.CompareTag("chunk"))
			{
				Vector3 hitPoint = hit.point;
				Vector3Int gridPosition = grid.WorldToCell(hitPoint);
				Vector3 cordinate = grid.GetCellCenterWorld(gridPosition);
				cordinate.y = 0.1f;
				temp.transform.position = cordinate;

				if (Input.GetMouseButtonDown(0))
				{
					Instantiate(testcube, cordinate, Quaternion.identity);
				}
			}
		}
	}
}
