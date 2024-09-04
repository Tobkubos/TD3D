using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkReveal2 : MonoBehaviour
{
	public GameObject[] StartEnd = new GameObject[2];
	List<GameObject> CheckPoints = new List<GameObject> { };
	public int index = 0;


	List<GameObject> MonsterPathCheckPoints = new List<GameObject> { };
	List<GameObject> AllPathTiles = new List<GameObject> { };
    List<GameObject> Obstacles = new List<GameObject> { };
    public GameObject CheckPoint;
	public GameObject Path;
	public GameObject BuyButton;
	public GameObject ChunkPlane;
	public GameObject Obstacle;


	private float elevation = 0f;
	private int PD = 2;
	private int count = 1;
	private int SortBy = 1;

	private int chunkSize = 11;
	[SerializeField] int numOfCheckPoints;

    bool isStart = false;
    bool isEnd = false;

	public bool Bought = false; //do usuwania koñca
    public void SetChunkSize(int size)
	{
		chunkSize = size;
        //BuyButton.transform.localPosition = new Vector3(size / 2, elevation, size / 2);
	}
	public void Generate()
	{
		if (chunkSize != 0 && StartEnd[0] != null && StartEnd[1] != null)
		{
			//numOfCheckPoints = Random.Range(0, 3);
			while (CheckPoints.Count < numOfCheckPoints)
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
					GameObject temp = Instantiate(CheckPoint, new Vector3(x + 0.5f, elevation+0.1f, z + 0.5f), Quaternion.identity, this.gameObject.transform);
					temp.name = "CheckPoint" + count;
					count++;
					CheckPoints.Add(temp);
					//AllPathTiles.Add(temp);
				}
			}

            //POSORTUJ I ZBUDUJ
            #region
            float startX = StartEnd[0].transform.localPosition.x;
			float endX = StartEnd[1].transform.localPosition.x;

			float startZ = StartEnd[0].transform.localPosition.z;
			float endZ = StartEnd[1].transform.localPosition.z;

            //Dodatkowy kawalek drogi dla mobów na koñcu aby polacyc chunki, (jakiœ taki most)
            float eX = StartEnd[1].transform.position.x;
            float eZ = StartEnd[1].transform.position.z;

            if (Mathf.Abs(startX - endX) == chunkSize - 1)
			{
				if (startX > endX)
				{
					GameObject temp = Instantiate(Path, new Vector3(eX-1, elevation, eZ), Quaternion.identity, this.gameObject.transform);
                    AllPathTiles.Add(temp);
                    CheckPoints = CheckPoints.OrderByDescending(go => go.transform.position.x).ToList();
				}
				else
				{
                    GameObject temp = Instantiate(Path, new Vector3(eX + 1, elevation, eZ), Quaternion.identity, this.gameObject.transform);
                    AllPathTiles.Add(temp);
                    CheckPoints = CheckPoints.OrderBy(go => go.transform.position.x).ToList();
				}
				SortBy = 0;
			}
			else if (Mathf.Abs(startZ - endZ) == chunkSize - 1)
			{
				if (startZ > endZ)
				{
                    GameObject temp = Instantiate(Path, new Vector3(eX, elevation, eZ - 1), Quaternion.identity, this.gameObject.transform);
                    AllPathTiles.Add(temp);
                    CheckPoints = CheckPoints.OrderByDescending(go => go.transform.position.z).ToList();
				}
				else
				{
                    GameObject temp = Instantiate(Path, new Vector3(eX, elevation, eZ + 1), Quaternion.identity, this.gameObject.transform);
                    AllPathTiles.Add(temp);
                    CheckPoints = CheckPoints.OrderBy(go => go.transform.position.z).ToList();
				}
				SortBy = 0;
			}


			else if (startZ == 0.5f && endX == 0.5f)
			{
                // START NA DOLE, END NA LEWO
                // ZAKRÊT Z DO£U DO LEWEJ
                GameObject temp = Instantiate(Path, new Vector3(eX - 1, elevation, eZ), Quaternion.identity, this.gameObject.transform);
                AllPathTiles.Add(temp);
                CheckPoints = CheckPoints.OrderByDescending(go => go.transform.position.x).ToList();
				SortBy = 0;
			}

			else if (startX == chunkSize - 0.5f && endZ == chunkSize - 0.5f)
			{
                // START NA PRAWO, END NA GÓRZE
                // ZAKRÊT Z PRAWEJ DO GÓRY
                GameObject temp = Instantiate(Path, new Vector3(eX, elevation, eZ + 1), Quaternion.identity, this.gameObject.transform);
                AllPathTiles.Add(temp);
                CheckPoints = CheckPoints.OrderBy(go => go.transform.position.z).ToList();
				SortBy = 1;
			}

			else if (startX == 0.5f && endZ == chunkSize - 0.5f)
			{
                // START NA LEWO, END NA GÓRZE
                // ZAKRÊT Z LEWEJ DO GÓRY
                GameObject temp = Instantiate(Path, new Vector3(eX, elevation, eZ + 1), Quaternion.identity, this.gameObject.transform);
                AllPathTiles.Add(temp);
                CheckPoints = CheckPoints.OrderBy(go => go.transform.position.z).ToList();
				SortBy = 1;
			}

			else if (startZ == 0.5f && endX == chunkSize - 0.5f)
			{
                // START NA DOLE, END NA PRAWO
                // ZAKRÊT Z DO£U DO PRAWEJ
                GameObject temp = Instantiate(Path, new Vector3(eX + 1, elevation, eZ), Quaternion.identity, this.gameObject.transform);
                AllPathTiles.Add(temp);
                CheckPoints = CheckPoints.OrderBy(go => go.transform.position.x).ToList();
				SortBy = 0;
			}

			else
			{
				SortBy = 3;
			}

			//Debug.Log(StartEnd[0].name + " sX: " + startX + " sZ: " + startZ + "    " + StartEnd[1].name + " eX: " + endX + " eZ: " + endZ + "SORTBY: " + SortBy);

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

			if(StartEnd[1].transform.localPosition.x == 0.5)
			{
				StartEnd[1].transform.localPosition += new Vector3(-1, 0, 0);
			}

            if (StartEnd[1].transform.localPosition.x == chunkSize - 0.5)
            {
                StartEnd[1].transform.localPosition += new Vector3(+1, 0, 0);
            }

            if (StartEnd[1].transform.localPosition.z == chunkSize-0.5)
            {
                StartEnd[1].transform.localPosition += new Vector3(0, 0, 1);
            }
            #endregion
        }

		//wygeneruj przszkody

		int NumOfObstacles = Random.Range(5, 20);
		Vector3 chunkCord = this.gameObject.transform.position;

		while(Obstacles.Count < NumOfObstacles){
			float x = Random.Range(0, chunkSize) + 0.5f;
			float z = Random.Range(0, chunkSize) + 0.5f;
            bool isPositionOccupied = false;

            foreach (GameObject Path in AllPathTiles)
            {
                if (Path.transform.localPosition == new Vector3(x, elevation, z) || Path.transform.localPosition == new Vector3(x, -0.1f, z))
                {
                    isPositionOccupied = true;
                    break;
                }
            }

            if (!isPositionOccupied)
            {
                GameObject obst = Instantiate(Obstacle, new Vector3(chunkCord.x + x, -0.1f, chunkCord.z + z), Quaternion.identity, this.gameObject.transform);
				obst.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, Random.Range(0,360), 0);
                Obstacles.Add(obst);
                AllPathTiles.Add(obst);
            }
        }
		
    }

	public void Disappear()
	{ 
        foreach (GameObject temp in AllPathTiles)
        {
            temp.gameObject.SetActive(false);
        }

        foreach (GameObject temp in CheckPoints)
        {
            temp.gameObject.SetActive(false);
        }
        StartEnd[0].SetActive(false);
        StartEnd[1].SetActive(false);
		ChunkPlane.tag = "notBought";
    }
	public void Buy()
	{
        StartCoroutine(BuyChunk());
    }
	public IEnumerator BuyChunk()
	{
		Bought = true;
		Destroy(BuyButton);
		if (index == 0) 
		{ 
			StartEnd[0].SetActive(true); 
		}
        StartEnd[1].SetActive(true);
		StartEnd[1].transform.localScale = Vector3.zero;
        LeanTween.scale(StartEnd[1], Vector3.one, 0.3f);
        GameObject manager = GameObject.Find("manager");
		StartCoroutine(manager.GetComponent<GeneratorV4>().DeleteUnnecessaryEnds());
        foreach (GameObject temp in AllPathTiles)
        {
			yield return new WaitForSeconds(0.1f);
			temp.gameObject.transform.localScale = Vector3.zero;
            temp.gameObject.SetActive(true);
			temp.gameObject.GetComponent<SpawnAnim>().SpawnAnimation();
        }
		ChunkPlane.tag = "chunk";
    }
}
