using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkReveal2 : MonoBehaviour
{
	private List<GameObject> AllChunkObjects = new List<GameObject> { };
	GameObject[] StartEnd = new GameObject[2];
	List<GameObject> CheckPoints = new List<GameObject> { };
	List<GameObject> MonsterPathCheckPoints = new List<GameObject> { };
	List<GameObject> AllPathTiles = new List<GameObject> { };
	public GameObject CheckPoint;
	public GameObject Path;
	private float elevation = 0.1f;
	private int PD = 2;
	private int count = 1;
	private int SortBy = 1;

	private int chunkSize = 11;


    bool isStart = false;
    bool isEnd = false;

	public void SetChunkSize(int size)
	{
		chunkSize = size;
	}

    IEnumerator OnTriggerEnter(Collider other)
	{
		yield return new WaitForSeconds(1);
		AllChunkObjects.Add(other.gameObject);
		//other.gameObject.SetActive(false);
		if (other.CompareTag("start") && !isStart)
		{
			//Debug.Log(other);
            other.transform.SetParent(this.transform);
            StartEnd[0] = (other.gameObject);
			Destroy(other.GetComponent<Rigidbody>());
			isStart = true;
		}

		if (other.CompareTag("end") && !isEnd)
		{
			//Debug.Log(other);
            other.transform.SetParent(this.transform);
            StartEnd[1] = (other.gameObject);
			Destroy(other.GetComponent<Rigidbody>());
			isEnd = true;
		}
	}
	/*
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
	*/
	public void Generate()
	{
		if (chunkSize != 0)
		{
			while (CheckPoints.Count < 2)
			{

				float x = Random.Range(2, chunkSize - 2) + this.gameObject.transform.position.x;
				float z = Random.Range(2, chunkSize - 2) + this.gameObject.transform.position.z;



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
					GameObject temp = Instantiate(CheckPoint, new Vector3(x + 0.5f, elevation, z + 0.5f), Quaternion.identity, this.gameObject.transform);
					temp.name = "CheckPoint" + count;
					count++;
					CheckPoints.Add(temp);
				}
			}
			//POSORTUJ
			float startX = StartEnd[0].transform.localPosition.x;
			float endX = StartEnd[1].transform.localPosition.x;

			float startZ = StartEnd[0].transform.localPosition.z;
			float endZ = StartEnd[1].transform.localPosition.z;

            //Dodatkowa droga na koñcu aby polacyc chunki
            float eX = StartEnd[1].transform.position.x;
            float eZ = StartEnd[1].transform.position.z;

            if (Mathf.Abs(startX - endX) == chunkSize - 1)
			{
				if (startX > endX)
				{
					Instantiate(Path, new Vector3(eX-1, elevation, eZ), Quaternion.identity, this.gameObject.transform);
					CheckPoints = CheckPoints.OrderByDescending(go => go.transform.position.x).ToList();
				}
				else
				{
                    Instantiate(Path, new Vector3(eX + 1, elevation, eZ), Quaternion.identity, this.gameObject.transform);
                    CheckPoints = CheckPoints.OrderBy(go => go.transform.position.x).ToList();
				}
				SortBy = 0;
			}
			else if (Mathf.Abs(startZ - endZ) == chunkSize - 1)
			{
				if (startZ > endZ)
				{
                    Instantiate(Path, new Vector3(eX, elevation, eZ - 1), Quaternion.identity, this.gameObject.transform);
                    CheckPoints = CheckPoints.OrderByDescending(go => go.transform.position.z).ToList();
				}
				else
				{
                    Instantiate(Path, new Vector3(eX, elevation, eZ + 1), Quaternion.identity, this.gameObject.transform);
                    CheckPoints = CheckPoints.OrderBy(go => go.transform.position.z).ToList();
				}
				SortBy = 0;
			}


			else if (startZ == 0.5f && endX == 0.5f)
			{
                // START NA DOLE, END NA LEWO
                // ZAKRÊT Z DO£U DO LEWEJ
                Instantiate(Path, new Vector3(eX - 1, elevation, eZ), Quaternion.identity, this.gameObject.transform);
                CheckPoints = CheckPoints.OrderByDescending(go => go.transform.position.x).ToList();
				SortBy = 0;
			}

			else if (startX == chunkSize - 0.5f && endZ == chunkSize - 0.5f)
			{
                // START NA PRAWO, END NA GÓRZE
                // ZAKRÊT Z PRAWEJ DO GÓRY
                Instantiate(Path, new Vector3(eX, elevation, eZ + 1), Quaternion.identity, this.gameObject.transform);
                CheckPoints = CheckPoints.OrderBy(go => go.transform.position.z).ToList();
				SortBy = 1;
			}

			else if (startX == 0.5f && endZ == chunkSize - 0.5f)
			{
                // START NA LEWO, END NA GÓRZE
                // ZAKRÊT Z LEWEJ DO GÓRY
                Instantiate(Path, new Vector3(eX, elevation, eZ + 1), Quaternion.identity, this.gameObject.transform);
                CheckPoints = CheckPoints.OrderBy(go => go.transform.position.z).ToList();
				SortBy = 1;
			}

			else if (startZ == 0.5f && endX == chunkSize - 0.5f)
			{
                // START NA DOLE, END NA PRAWO
                // ZAKRÊT Z DO£U DO PRAWEJ
                Instantiate(Path, new Vector3(eX + 1, elevation, eZ), Quaternion.identity, this.gameObject.transform);
                CheckPoints = CheckPoints.OrderBy(go => go.transform.position.x).ToList();
				SortBy = 0;
			}

			else
			{
				SortBy = 3;
			}

			Debug.Log(StartEnd[0].name + " sX: " + startX + " sZ: " + startZ + "    " + StartEnd[1].name + " eX: " + endX + " eZ: " + endZ + "SORTBY: " + SortBy);

			//Debug.Log(this.gameObject.name);
			//Debug.Log(this.gameObject.name + " " + SortBy);
			for (int i = 0; i < CheckPoints.Count; i++)
			{
				//Debug.Log(CheckPoints[i].name);
			}
			//Debug.Log("+++++++++++++++");

			//SCAL
			MonsterPathCheckPoints.Add(StartEnd[0]);
			for (int i = 0; i < CheckPoints.Count; i++)
			{
				MonsterPathCheckPoints.Add(CheckPoints[i]);
			}
			MonsterPathCheckPoints.Add(StartEnd[1]);

			//GENERATOR DROGI MOBÓW
			for (int i = 1; i < MonsterPathCheckPoints.Count; i++)
			{
				float xPrev = MonsterPathCheckPoints[i - 1].transform.position.x;
				float xAct = MonsterPathCheckPoints[i].transform.position.x;

				float zPrev = MonsterPathCheckPoints[i - 1].transform.position.z;
				float zAct = MonsterPathCheckPoints[i].transform.position.z;

				float DistX = xAct - xPrev;
				float DistY = zAct - zPrev;


				if (SortBy == 0)
				{
					//zbuduj po X
					if (xAct != xPrev)
					{
						if (xAct > xPrev)
						{
							for (float j = xPrev; j <= xAct; j++)
							{
								GameObject temp = Instantiate(Path, new Vector3(j, elevation, zPrev), Quaternion.identity, this.gameObject.transform);
								AllPathTiles.Add(temp);
							}
						}
						else
						{
							for (float j = xPrev; j >= xAct; j--)
							{
								GameObject temp = Instantiate(Path, new Vector3(j, elevation, zPrev), Quaternion.identity, this.gameObject.transform);
								AllPathTiles.Add(temp);
							}
						}
					}

					//zbuduj po Z
					if (zAct != zPrev)
					{
						if (zAct > zPrev)
						{
							for (float j = zPrev; j <= zAct; j++)
							{
								GameObject temp = Instantiate(Path, new Vector3(xAct, elevation, j), Quaternion.identity, this.gameObject.transform);
								AllPathTiles.Add(temp);
							}
						}
						else
						{
							for (float j = zPrev; j >= zAct; j--)
							{
								GameObject temp = Instantiate(Path, new Vector3(xAct, elevation, j), Quaternion.identity, this.gameObject.transform);
								AllPathTiles.Add(temp);
							}
						}
					}
				}


				if (SortBy == 1)
				{
					//zbuduj po Z potem X
					if (zAct != zPrev)
					{
						if (zAct > zPrev)
						{
							for (float j = zPrev; j <= zAct; j++)
							{
								GameObject temp = Instantiate(Path, new Vector3(xPrev, elevation, j), Quaternion.identity, this.gameObject.transform);
								AllPathTiles.Add(temp);
							}
						}
						else
						{
							for (float j = zPrev; j >= zAct; j--)
							{
								GameObject temp = Instantiate(Path, new Vector3(xPrev, elevation, j), Quaternion.identity, this.gameObject.transform);
								AllPathTiles.Add(temp);
							}
						}
					}

					//zbuduj po X potem Z
					if (xAct != xPrev)
					{
						if (xAct > xPrev)
						{
							for (float j = xPrev; j <= xAct; j++)
							{
								GameObject temp = Instantiate(Path, new Vector3(j, elevation, zAct), Quaternion.identity, this.gameObject.transform);
								AllPathTiles.Add(temp);
							}
						}
						else
						{
							for (float j = xPrev; j >= xAct; j--)
							{
								GameObject temp = Instantiate(Path, new Vector3(j, elevation, zAct), Quaternion.identity, this.gameObject.transform);
								AllPathTiles.Add(temp);
							}
						}
					}
				}
			}
		}
	}
}
