using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
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
	int PD = 2;
	private List<string> Directions = new List<string> { "North", "East", "South", "West" };
	private List<GameObject> CheckPoints = new List<GameObject> { };
	private List<GameObject> Paths = new List<GameObject> { };
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
		/*
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
				i++;
				CheckPoints.Add(temp);
			}
		}
		*/
		int count = 1;
		while(CheckPoints.Count < 5)
		{
			int x = Random.Range(2, SizeOfPlate - 2);
			int y = Random.Range(2, SizeOfPlate - 2);
			bool canPlace = true;
			foreach(GameObject go in CheckPoints)
			{
				int goX = (int)go.transform.position.x;
				int goY = (int)go.transform.position.z;
				if ( (goX < x + PD && goX > x - PD) || (goY < y + PD && goY > y - PD)) 
				{ 
					canPlace = false;
				}
			}
			if(canPlace)
			{
				GameObject temp = Instantiate(CheckPoint, new Vector3(x, 0.3f, y), Quaternion.identity);
				temp.name = "CheckPoint" + count;
				count++;
				CheckPoints.Add(temp);
			}
		}
	}
	public void TempGenerate()
	{
		foreach (GameObject obj in CheckPoints)
		{
			if (obj != null)
			{
				Destroy(obj);
			}
		}

		foreach (GameObject obj in Paths)
		{
			if (obj != null)
			{
				Destroy(obj);
			}
		}
		CheckPoints = new List<GameObject> { };
		Paths = new List<GameObject> { };

		GenerateStartEndPoint(StartTile);
		GenerateCheckPoints();


		Debug.Log(CheckPoints.Count);
		float referenceX = CheckPoints[0].transform.position.x;
		float referenceZ = CheckPoints[0].transform.position.z;
		float rotationY =  CheckPoints[0].transform.eulerAngles.y;

		if (rotationY == 0 || Mathf.Approximately(rotationY, 180))
		{
			CheckPoints = CheckPoints.OrderBy(go => Mathf.Abs(go.transform.position.z - referenceZ)).ToList();
		}
		else if (Mathf.Approximately(rotationY, 90) || Mathf.Approximately(rotationY, 270))
		{
			CheckPoints = CheckPoints.OrderBy(go => Mathf.Abs(go.transform.position.x - referenceX)).ToList();
		}

		// Wyœwietlenie
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

			Debug.Log(xPrev +" "+ xAcc + " " + yPrev + " " + yAcc);

			int DistX = xAcc - xPrev;
			int DistY = yAcc - yPrev;
			Debug.Log(DistX+" dist "+DistY);
			if (DistY > 0)
			{
				Debug.Log("trzy");
				for (int j = yPrev; j < yPrev + DistY; j++)
				{
					GameObject temp = Instantiate(Path, new Vector3(xAcc, 0.1f, j), Quaternion.identity);
					Paths.Add(temp);
				}
			}
			if (DistY < 0)
			{
				Debug.Log("cztery");
				for (int j = yPrev; j > yPrev + DistY; j--)
				{
					Debug.Log("cztery cztery");
					GameObject temp = Instantiate(Path, new Vector3(xAcc, 0.1f, j), Quaternion.identity);
					Paths.Add(temp);
				}
			}

			if (DistX > 0)
			{
				Debug.Log("jeden");
				for (int j = xPrev; j < xPrev + DistX; j++)
				{
					GameObject temp = Instantiate(Path, new Vector3(j, 0.1f, yPrev), Quaternion.identity);
					Paths.Add(temp);
				}
			}
			if (DistX < 0)
			{
				Debug.Log("dwa");
				for (int j = xPrev; j > xPrev + DistX; j--)
				{
					GameObject temp = Instantiate(Path, new Vector3(j, 0.1f, yPrev), Quaternion.identity);
					Paths.Add(temp);
				}
			}

		}

		GenerateStartEndPoint(EndTile);
		Directions.Clear();
		Directions = new List<string> { "North", "East", "South", "West" };

	}
}
