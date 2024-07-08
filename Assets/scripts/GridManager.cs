using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	public Grid grid;
	private Dictionary<Vector3Int, GameObject> tileDictionary;

	void Start()
	{
		tileDictionary = new Dictionary<Vector3Int, GameObject>();
	}

	public bool IsTileOccupied(Vector3Int gridPosition)
	{
		return tileDictionary.ContainsKey(gridPosition);
	}

	public void OccupyTile(Vector3Int gridPosition, GameObject obj)
	{
		if (!tileDictionary.ContainsKey(gridPosition))
		{
			tileDictionary[gridPosition] = obj;
		}
	}

	public void FreeTile(Vector3Int gridPosition)
	{
		if (tileDictionary.ContainsKey(gridPosition))
		{
			tileDictionary.Remove(gridPosition);
		}
	}

	public GameObject GetObjectOnTile(Vector3Int gridPosition)
	{
		if (tileDictionary.ContainsKey(gridPosition))
		{
			return tileDictionary[gridPosition];
		}
		return null;
	}
}