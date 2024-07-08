using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    public GameObject _tilePrefab;
    void Start()
    {
        var worldPosition  = _grid.GetCellCenterWorld(new Vector3Int(0, 0, 1));
        worldPosition.y = 0;
		Instantiate(_tilePrefab, worldPosition, Quaternion.identity);
		var worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(1, 0, 4));
		Instantiate(_tilePrefab, worldPosition2, Quaternion.identity);
    }
}
