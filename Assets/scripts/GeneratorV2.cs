using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneratorV2 : MonoBehaviour
{
	private List<GameObject> BigChunkCheckPoints = new List<GameObject> { };
	private List<GameObject> Path = new List<GameObject> { };
	private List<GameObject> Connectors = new List<GameObject> { };
	public GameObject MonsterPath;
	public GameObject Checkpoint;
	public GameObject BigChunkCheckPoint;
	private int SizeOfMap = 3;
	private int x,z;
	private int count = 1;
	private int connCount = 1;
	public int elevation = -5;
	public GameObject Connector;
	void Start()
    {
		StartCoroutine(GenerateBigChunks());
	}
	public void Reset2()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Reset()
	{
		foreach(GameObject go in BigChunkCheckPoints)
		{
			Destroy(go);
		}
		BigChunkCheckPoints = new List<GameObject> { };

		foreach (GameObject go in Path)
		{
			Destroy(go);
		}
		Path = new List<GameObject> { };

		foreach (GameObject go in Connectors)
		{
			Destroy(go);
		}
		Connectors = new List<GameObject> { };

		count = 0;
		connCount = 0;
		StartCoroutine(GenerateBigChunks());
	}
	private IEnumerator GenerateBigChunks() { 

		for(int i =0; i<SizeOfMap; i++)
		{
			x = Random.Range(0, SizeOfMap);
			GameObject first = Instantiate(BigChunkCheckPoint, new Vector3(x * 10, 0.3f, i * 10), Quaternion.identity);
			BigChunkCheckPoints.Add(first);
		}
		
		BigChunkCheckPoints = BigChunkCheckPoints.OrderBy(obj => obj.transform.position.z).ToList();

		for (int i = 1; i < BigChunkCheckPoints.Count; i++)
		{
			int xPrev = (int)BigChunkCheckPoints[i - 1].transform.position.x;
			int xAcc = (int)BigChunkCheckPoints[i].transform.position.x;

			int yPrev = (int)BigChunkCheckPoints[i - 1].transform.position.z;
			int yAcc = (int)BigChunkCheckPoints[i].transform.position.z;

			int DistX = xAcc - xPrev;
			int DistY = yAcc - yPrev;

			if (DistX > 0)
			{
				//Debug.Log("jeden");
				for (int j = xPrev; j < xPrev + DistX; j += 10)
				{
					GameObject temp = Instantiate(BigChunkCheckPoint, new Vector3(j, elevation, yPrev), Quaternion.identity);
					temp.name = "SCIEZKA" + count;
					count++;
					Path.Add(temp);
				}

			}
			if (DistX < 0)
			{

				//Debug.Log("dwa");
				for (int j = xPrev; j > xPrev + DistX; j -= 10)
				{
					GameObject temp = Instantiate(BigChunkCheckPoint, new Vector3(j, elevation, yPrev), Quaternion.identity);
					temp.name = "SCIEZKA" + count;
					count++;
					Path.Add(temp);
				}

			}
			if (DistY > 0)
			{
				//Debug.Log("trzy");

				for (int j = yPrev; j < yPrev + DistY; j += 10)
				{
					GameObject temp = Instantiate(BigChunkCheckPoint, new Vector3(xAcc, elevation, j), Quaternion.identity);
					temp.name = "SCIEZKA" + count;
					count++;
					Path.Add(temp);
				}

			}
			if (DistY < 0)
			{
				//Debug.Log("cztery");
				for (int j = yPrev; j > yPrev + DistY; j -= 10)
				{
					//Debug.Log("cztery cztery");
					GameObject temp = Instantiate(BigChunkCheckPoint, new Vector3(xAcc, elevation, j), Quaternion.identity);
					temp.name = "SCIEZKA" + count;
					count++;
					Path.Add(temp);
				}

			}
		}
		GameObject t2 = Instantiate(BigChunkCheckPoint, new Vector3(BigChunkCheckPoints[BigChunkCheckPoints.Count-1].transform.position.x, elevation, BigChunkCheckPoints[BigChunkCheckPoints.Count - 1].transform.position.z), Quaternion.identity);
		t2.name = "SCIEZKA" + count;
		count++;
		Path.Add(t2);

		
		foreach (var go in BigChunkCheckPoints)
		{
			Debug.Log(go.name + " - x: " + go.transform.position.x + ", z: " + go.transform.position.z);
		}
		
		
		foreach (var go in Path)
		{
			Debug.Log(go.name + " - x: " + go.transform.position.x + ", z: " + go.transform.position.z);
		}
		

		foreach (GameObject go in BigChunkCheckPoints)      //USUÑ PUNKTY ZACZEPIENIA TRASY
		{
			Destroy(go);
		}

		BigChunkCheckPoints = new List<GameObject> { };     //WYZERUJ W RAZIE CZEGO TABLICE


		for (int i = 1; i < Path.Count; i++)        //NA PODSTAWIE STWORZONEJ TRASY ZROB MIEJCA GDZIE CHUNKI BEDA POLACZONE ZE SOBA
		{
			GenConnections(i);
		}

		yield return new WaitForSeconds(0.3f);
		for (int q = 0; q < Path.Count; q++)
		{
			Path[q].GetComponent<ChunkReveal>().Generate();
		}
	
	}
	void GenConnections(int i)  //WYGENERUJ POLACZENIA MIEDZY CHUNKAMI
	{
		int xPrev = (int)Path[i - 1].transform.position.x;
		int xAcc = (int)Path[i].transform.position.x;

		int yPrev = (int)Path[i - 1].transform.position.z;
		int yAcc = (int)Path[i].transform.position.z;

		int Rand = Random.Range(-3,3);

		if(xPrev < xAcc)
		{
			GameObject cn1 = Instantiate(Connector, new Vector3(xPrev + 4.5f, elevation, yPrev -Rand - 0.5f),Quaternion.identity);
		    GameObject cn2 = Instantiate(Connector, new Vector3(xPrev + 5.5f, elevation, yPrev - Rand - 0.5f), Quaternion.identity);
			if(i == 1)
			{
				GameObject cn3 = Instantiate(Connector, new Vector3(xPrev - 4.5f, elevation, yPrev - Rand - 0.5f), Quaternion.identity);
				cn3.name = "CONNECTOR " + connCount;
				cn3.tag = "start";
				connCount++;
				Connectors.Add(cn3);
			}
			cn1.name = "CONNECTOR " + connCount;
			cn1.tag = "end";
			connCount++;
			cn2.name = "CONNECTOR " + connCount;
			cn2.tag = "start";
			connCount++;
			Connectors.Add(cn1);
			Connectors.Add(cn2);

			if (i == Path.Count-1)
			{
				GameObject end = Instantiate(Connector, new Vector3(xAcc - 4.5f, elevation, yPrev - Rand - 0.5f), Quaternion.identity);
				end.name = "END ";
				connCount++;
				Connectors.Add(end);
			}

		}

		if (xPrev > xAcc)
		{
			GameObject cn1 = Instantiate(Connector, new Vector3(xAcc + 4.5f, elevation, yPrev - Rand - 0.5f), Quaternion.identity);
			GameObject cn2 = Instantiate(Connector, new Vector3(xAcc + 5.5f, elevation, yPrev - Rand - 0.5f), Quaternion.identity);
			if (i == 1)
			{
				GameObject cn3 = Instantiate(Connector, new Vector3(xPrev + 4.5f, elevation, yPrev - Rand - 0.5f), Quaternion.identity);
				cn3.name = "CONNECTOR " + connCount;
				cn3.tag = "start";
				connCount++;
				Connectors.Add(cn3);
			}
			cn2.name = "CONNECTOR " + connCount;
			cn2.tag = "end";
			connCount++;
			cn1.name = "CONNECTOR " + connCount;
			cn1.tag = "start";
			connCount++;

			Connectors.Add(cn2);
			Connectors.Add(cn1);

			if (i == Path.Count - 1)
			{
				GameObject end = Instantiate(Connector, new Vector3(xAcc + 4.5f, elevation, yPrev - Rand - 0.5f), Quaternion.identity);
				end.name = "END ";
				connCount++;
				Connectors.Add(end);
			}
		}

		if (yPrev < yAcc)
		{
			GameObject cn1 = Instantiate(Connector, new Vector3(xPrev - Rand - 0.5f, elevation, yPrev + 4.5f), Quaternion.identity);
			GameObject cn2 = Instantiate(Connector, new Vector3(xPrev - Rand - 0.5f, elevation, yPrev + 5.5f), Quaternion.identity);
			if (i == 1)
			{
				GameObject cn3 = Instantiate(Connector, new Vector3(xPrev - Rand - 0.5f, elevation, yPrev - 4.5f), Quaternion.identity);
				cn3.name = "CONNECTOR " + connCount;
				cn3.tag = "start";
				connCount++;
				Connectors.Add(cn3);
			}
			cn1.name = "CONNECTOR " + connCount;
			cn1.tag = "end";
			connCount++;
			cn2.name = "CONNECTOR " + connCount;
			cn2.tag = "start";
			connCount++;
			Connectors.Add(cn1);
			Connectors.Add(cn2);
			if (i == Path.Count - 1)
			{
				GameObject end = Instantiate(Connector, new Vector3(xPrev - Rand - 0.5f, elevation, yAcc + 4.5f), Quaternion.identity);
				end.name = "END ";
				end.tag = "end";
				connCount++;
				Connectors.Add(end);
			}
		}
	}
}
