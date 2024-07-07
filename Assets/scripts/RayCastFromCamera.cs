using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastFromCamera : MonoBehaviour
{
	public Camera camera;
	public Grid grid;
	public GameObject _tilePrefab;
	GameObject temp;
	private Vector3 savedPos;

	void Start()
	{
		if (camera == null)
		{
			camera = Camera.main;
		}
		savedPos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
	}

	void Update()
	{

		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Input.GetMouseButtonDown(0))
		{
			ray = camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{


				Vector3 hitPoint = hit.point;

				// Convert the world position to grid coordinates
				Vector3Int gridPosition = grid.WorldToCell(hitPoint);
				Debug.Log("Koordynaty gridu: " + gridPosition);





				if (hit.collider.CompareTag("chunk"))
				{
					Debug.Log("Obiekt trafiony: " + hit.collider.name + " ma tag 'chunk'.");
					hit.collider.gameObject.GetComponent<ChunkReveal>().Reveal();
				}
			}
		}


		if (Physics.Raycast(ray, out hit))
		{
			Vector3 hitPoint = hit.point;
			Vector3Int gridPosition = grid.WorldToCell(hitPoint);

			if (hit.collider.CompareTag("chunk"))
			{
				if (savedPos != gridPosition)
				{
					if (temp != null)
					{
						Destroy(temp);
					}

					Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);
					temp = Instantiate(_tilePrefab, worldPosition, Quaternion.identity);
					savedPos = gridPosition;
				}
			}
		}
		else
		{
			if (temp != null)
			{
				Destroy(temp);
				temp = null;
				savedPos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
			}
		}
	}
}
