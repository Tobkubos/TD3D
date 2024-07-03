using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Generator : MonoBehaviour
{
	public GameObject SurfaceTile;
	public GameObject StartTile;
	public GameObject EndTile;
	public GameObject CheckPoint;
	public GameObject Path;
	private int StartX, StartY, Rotation;
	int SizeOfPlate = 20;
	float tick = 0.05f;
	private List<string> Directions = new List<string> { "North", "East", "South", "West" };
	private List<GameObject> CheckPoints = new List<GameObject> { };
	void Start()
	{
		StartCoroutine(GenerateTiles());
	}

	private IEnumerator GenerateTiles()
	{
		for (int j = 0; j < SizeOfPlate; j++)
		{
			yield return new WaitForSeconds(tick);
			StartCoroutine(GenerateRow(j));
		}

		TempGenerate();
	}

	private IEnumerator GenerateRow(int j)
	{
		for (int i = 0; i < SizeOfPlate; i++)
		{
			yield return new WaitForSeconds(tick * 2);
			Instantiate(SurfaceTile, new Vector3(i, 0, j), Quaternion.identity);
		}
	}

	void GenerateStartEndPoint(GameObject Object)
	{
		int RandomSide = Random.Range(0, Directions.Count);
		string Side = Directions[RandomSide];
		if (Side == "North")
		{
			StartX = Random.Range(2, SizeOfPlate - 1);
			StartY = SizeOfPlate - 1;
			Rotation = 0;
		}
		if (Side == "East")
		{
			StartX = SizeOfPlate - 1;
			StartY = Random.Range(2, SizeOfPlate - 1);
			Rotation = 90;
		}
		if (Side == "South")
		{
			StartX = Random.Range(2, SizeOfPlate - 1);
			StartY = 0;
			Rotation = 180;
		}
		if (Side == "West")
		{
			StartX = 0;
			StartY = Random.Range(2, SizeOfPlate - 1);
			Rotation = 270;
		}

		Directions.Remove(Side.ToString());
		Vector3 Cords = new Vector3(StartX, 0.2f, StartY);
		GameObject temp = Instantiate(Object, Cords, Quaternion.Euler(0, Rotation, 0));
		CheckPoints.Add(temp);
	}

	void GenerateCheckPoints()
	{
		int count = 1;
		for (int i = 0; i < SizeOfPlate; i++)
		{
			int CanPlace = Random.Range(0, 10);
			if (CanPlace < 3)
			{
				int rowCord = Random.Range(1, SizeOfPlate - 1);
				GameObject temp = Instantiate(CheckPoint, new Vector3(rowCord, 0.3f, i), Quaternion.identity);
				temp.name = "CheckPoint" + count;
				count++;
				CheckPoints.Add(temp);
			}
		}
	}
	public void TempGenerate()
	{
		Destroy(GameObject.FindGameObjectWithTag("start"));
		Destroy(GameObject.FindGameObjectWithTag("end"));
		foreach (GameObject obj in CheckPoints)
		{
			if (obj != null)
			{
				Destroy(obj);
			}
		}
		GenerateStartEndPoint(StartTile);
		GenerateCheckPoints();

		float referenceX = CheckPoints[0].transform.position.x;
		float referenceZ = CheckPoints[0].transform.position.z;
		float rotationY = CheckPoints[0].transform.eulerAngles.y;

		if (rotationY == 0 || Mathf.Approximately(rotationY, 180))
		{
			CheckPoints = CheckPoints.OrderBy(go => Mathf.Abs(go.transform.position.z - referenceZ)).ToList();
		}
		else if (Mathf.Approximately(rotationY, 90) || Mathf.Approximately(rotationY, 270))
		{
			CheckPoints = CheckPoints.OrderBy(go => Mathf.Abs(go.transform.position.x - referenceX)).ToList();
		}

		// Wyœwietlenie posortowanych obiektów w konsoli
		foreach (var go in CheckPoints)
		{
			Debug.Log(go.name + " - x: " + go.transform.position.x + ", z: " + go.transform.position.z);
		}


for (int i = 1; i<CheckPoints.Count; i++)
		{
			int xPrev = (int)CheckPoints[i - 1].transform.position.x;
			int xAcc = (int)CheckPoints[i].transform.position.x;

			int yPrev = (int)CheckPoints[i - 1].transform.position.z;
			int yAcc = (int)CheckPoints[i].transform.position.z;


		}


		GenerateStartEndPoint(EndTile);
		Directions.Clear();
		Directions = new List<string> { "North", "East", "South", "West" };
	}
}
