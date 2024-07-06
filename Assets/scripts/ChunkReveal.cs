using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkReveal : MonoBehaviour
{
	private List<GameObject> AllChunkObjects = new List<GameObject> { };
	GameObject[] StartEnd = new GameObject[2];
	List<GameObject> CheckPoints = new List<GameObject> { };
	List<GameObject> MonsterPath = new List<GameObject> { };
	List<GameObject> AllPathTiles = new List<GameObject> { };
	public GameObject CheckPoint;
	public GameObject Path;
	private float elevation = 0.1f;
	private int PD = 2;
	private int count = 1;
	private int SortBy = 1;

	private void OnTriggerEnter(Collider other)
	{
		AllChunkObjects.Add(other.gameObject);
		//other.gameObject.SetActive(false);
		if (other.CompareTag("start"))
		{
			//Debug.Log(other);
			StartEnd[0] = (other.gameObject);
			Destroy(other.GetComponent<Rigidbody>());
		}

		if (other.CompareTag("end"))
		{
			StartEnd[1] = (other.gameObject);
			Destroy(other.GetComponent<Rigidbody>());
		}
	}
	public void Reveal()
	{
		foreach(GameObject ob in AllChunkObjects)
		{
			Rigidbody rb = ob.GetComponent<Rigidbody>();
			if (rb != null)
			{
				Destroy(rb);
			}
			ob.SetActive(true);
		}
	}

	public void Generate()
	{
		while(CheckPoints.Count < 2) {

			float x = Random.Range(-3, 3) + this.gameObject.transform.position.x;
			float z = Random.Range(-3, 3) + this.gameObject.transform.position.z;

			bool canPlace = true;
			foreach (GameObject go in CheckPoints)
			{
				float goX = go.transform.position.x;
				float goZ = go.transform.position.z;
				if ((goX < x + PD && goX > x - PD) || (goZ < z + PD && goZ > z - PD))
				{
					canPlace = false;
				}
			}
			if (canPlace)
			{
				GameObject temp = Instantiate(CheckPoint, new Vector3(x + 0.5f, elevation, z + 0.5f), Quaternion.identity);
				temp.name = "CheckPoint" + count;
				count++;
				CheckPoints.Add(temp);
			}
		}
		//POSORTUJ
		float startX = StartEnd[0].transform.position.x;
		float endX = StartEnd[1].transform.position.x;

		float startZ = StartEnd[0].transform.position.z;
		float endZ = StartEnd[1].transform.position.z;
		
		if (Mathf.Abs( startX - endX )== 9)
		{
			if (startX > endX)
			{
				CheckPoints = CheckPoints.OrderByDescending(go => go.transform.position.x).ToList();
			}
			else
			{
				CheckPoints = CheckPoints.OrderBy(go => go.transform.position.x).ToList();
			}
			SortBy = 0;
		}
		else if(Mathf.Abs(startZ - endZ) == 9)
		{
			if (startZ > endZ)
			{
				CheckPoints = CheckPoints.OrderByDescending(go => go.transform.position.z).ToList();
			}
			else
			{
				CheckPoints = CheckPoints.OrderBy(go => go.transform.position.z).ToList();
			}
			SortBy = 0;
		}

		
		else if(startX > endX && startZ < endZ && endX %10f == 5.5f)
		{
			CheckPoints = CheckPoints.OrderByDescending(go => go.transform.position.x).ToList();
			SortBy = 0;
		}
		else if (startX > endX && startZ < endZ && endZ % 10f == 4.5f)
		{
			CheckPoints = CheckPoints.OrderBy(go => go.transform.position.z).ToList();
			SortBy = 1;
		}
		else if (startX < endX && startZ < endZ && endZ % 10f == 4.5f)
		{
			CheckPoints = CheckPoints.OrderBy(go => go.transform.position.z).ToList();
			SortBy = 1;
		}
		else if (startX < endX && startZ < endZ && endX % 10f == 4.5f)
		{
			CheckPoints = CheckPoints.OrderBy(go => go.transform.position.x).ToList();
			SortBy = 0;
		}
		else
		{
			SortBy = 3;
		}



		Debug.Log(this.gameObject.name);
		Debug.Log(SortBy);
		for (int i = 0; i<CheckPoints.Count; i++)
		{
			Debug.Log(CheckPoints[i].name);
		}
		Debug.Log("+++++++++++++++");

		//SCAL
		MonsterPath.Add(StartEnd[0]);
		for(int i = 0; i<CheckPoints.Count; i++)
		{
			MonsterPath.Add(CheckPoints[i]);
		}
		MonsterPath.Add(StartEnd[1]);

		//GENERATOR DROGI MOBÓW
		for (int i = 1; i < MonsterPath.Count; i++)
		{
			float xPrev = MonsterPath[i - 1].transform.position.x;
			float xAcc = MonsterPath[i].transform.position.x;

			float yPrev = MonsterPath[i - 1].transform.position.z;
			float yAcc = MonsterPath[i].transform.position.z;

			float DistX = xAcc - xPrev;
			float DistY = yAcc - yPrev;


			if(SortBy == 0)
			{
				if (xAcc != xPrev)
				{
					if (xAcc > xPrev)
					{
						for (float j = xPrev; j <= xAcc; j++)
						{
							GameObject temp = Instantiate(Path, new Vector3(j, elevation, yPrev), Quaternion.identity);
							AllPathTiles.Add(temp);
						}
					}
					else
					{
						for (float j = xPrev; j >= xAcc; j--)
						{
							GameObject temp = Instantiate(Path, new Vector3(j, elevation, yPrev), Quaternion.identity);
							AllPathTiles.Add(temp);
						}
					}
				}

				// Now handle the Y-axis
				if (yAcc != yPrev)
				{
					if (yAcc > yPrev)
					{
						for (float j = yPrev; j <= yAcc; j++)
						{
							GameObject temp = Instantiate(Path, new Vector3(xAcc, elevation, j), Quaternion.identity);
							AllPathTiles.Add(temp);
						}
					}
					else
					{
						for (float j = yPrev; j >= yAcc; j--)
						{
							GameObject temp = Instantiate(Path, new Vector3(xAcc, elevation, j), Quaternion.identity);
							AllPathTiles.Add(temp);
						}
					}
				}
			}
		

			if (SortBy == 1)
			{
				// First handle the Y-axis
				if (yAcc != yPrev)
				{
					if (yAcc > yPrev)
					{
						for (float j = yPrev; j <= yAcc; j++)
						{
							GameObject temp = Instantiate(Path, new Vector3(xPrev, elevation, j), Quaternion.identity);
							AllPathTiles.Add(temp);
						}
					}
					else
					{
						for (float j = yPrev; j >= yAcc; j--)
						{
							GameObject temp = Instantiate(Path, new Vector3(xPrev, elevation, j), Quaternion.identity);
							AllPathTiles.Add(temp);
						}
					}
				}

				// Now handle the X-axis
				if (xAcc != xPrev)
				{
					if (xAcc > xPrev)
					{
						for (float j = xPrev; j <= xAcc; j++)
						{
							GameObject temp = Instantiate(Path, new Vector3(j, elevation, yAcc), Quaternion.identity);
							AllPathTiles.Add(temp);
						}
					}
					else
					{
						for (float j = xPrev; j >= xAcc; j--)
						{
							GameObject temp = Instantiate(Path, new Vector3(j, elevation, yAcc), Quaternion.identity);
							AllPathTiles.Add(temp);
						}
					}
				}
			}
		}
	}
}
