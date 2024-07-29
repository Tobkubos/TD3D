using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class GeneratorV3 : MonoBehaviour
{
	private List<GameObject> BigChunkCheckPoints = new List<GameObject> { };
	private List<GameObject> Path = new List<GameObject> { };
	private List<GameObject> Connectors = new List<GameObject> { };
	public GameObject MonsterPath;
	public GameObject Checkpoint;
	public GameObject BigChunkCheckPoint;

	private int SizeOfMap =5 ;
	private int x,z;
	private int count = 1;
	private int connCount = 1;
	public float elevation = 0;
	public GameObject Connector;

    public int chunkSize = 5;
    public Vector3 cordinate;
    public Grid grid;
	void Start()
    {
		GenerateBigChunks();
	}
	public void Reset2()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	private void GenerateBigChunks()
	{

        //generowanie punktów zaczepenia chunków
        for (int i = 0; i < SizeOfMap; i++)
        {
            x = Random.Range(0, SizeOfMap);

            GameObject first = Instantiate(BigChunkCheckPoint, new Vector3(x * (chunkSize + 1), elevation, i * (chunkSize + 1)), Quaternion.identity);
			first.GetComponent<MeshGenerator>().SetSize(chunkSize,chunkSize);
			first.GetComponent<MeshGenerator>().GenerateMesh();
            BigChunkCheckPoints.Add(first);
        }

        BigChunkCheckPoints = BigChunkCheckPoints.OrderBy(obj => obj.transform.position.z).ToList();


        //tworzenie chunków na podstawie checkpointów
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
                for (int j = xPrev; j < xPrev + DistX; j += chunkSize+1)
                {
                    GameObject temp = Instantiate(BigChunkCheckPoint, new Vector3(j, elevation, yPrev), Quaternion.identity);
                    temp.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
                    temp.GetComponent<MeshGenerator>().GenerateMesh();
                    temp.name = "SCIEZKA" + count;
                    count++;
                    Path.Add(temp);
                }

            }
            if (DistX < 0)
            {

                //Debug.Log("dwa");
                for (int j = xPrev; j > xPrev + DistX; j -= chunkSize + 1)
                {
                    GameObject temp = Instantiate(BigChunkCheckPoint, new Vector3(j, elevation, yPrev), Quaternion.identity);
                    temp.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
                    temp.GetComponent<MeshGenerator>().GenerateMesh();
                    temp.name = "SCIEZKA" + count;
                    count++;
                    Path.Add(temp);
                }

            }
            if (DistY > 0)
            {
                //Debug.Log("trzy");

                for (int j = yPrev; j < yPrev + DistY; j += chunkSize + 1)
                {
                    GameObject temp = Instantiate(BigChunkCheckPoint, new Vector3(xAcc, elevation, j), Quaternion.identity);
                    temp.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
                    temp.GetComponent<MeshGenerator>().GenerateMesh();
                    temp.name = "SCIEZKA" + count;
                    count++;
                    Path.Add(temp);
                }

            }
            if (DistY < 0)
            {
                //Debug.Log("cztery");
                for (int j = yPrev; j > yPrev + DistY; j -= chunkSize + 1)
                {
                    //Debug.Log("cztery cztery");
                    GameObject temp = Instantiate(BigChunkCheckPoint, new Vector3(xAcc, elevation, j), Quaternion.identity);
                    temp.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
                    temp.GetComponent<MeshGenerator>().GenerateMesh();
                    temp.name = "SCIEZKA" + count;
                    count++;
                    Path.Add(temp);
                }

            }
        }
        
        GameObject t2 = Instantiate(BigChunkCheckPoint, new Vector3(BigChunkCheckPoints[BigChunkCheckPoints.Count - 1].transform.position.x, elevation, BigChunkCheckPoints[BigChunkCheckPoints.Count - 1].transform.position.z), Quaternion.identity);
        t2.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
        t2.GetComponent<MeshGenerator>().GenerateMesh();
        t2.name = "SCIEZKA" + count;
        count++;
        Path.Add(t2);
        
        foreach (GameObject go in BigChunkCheckPoints)      //USUÑ PUNKTY ZACZEPIENIA TRASY
        {
            Destroy(go);
        }
        BigChunkCheckPoints = new List<GameObject> { };     //WYZERUJ W RAZIE CZEGO

        for (int i = 1; i < Path.Count; i++)        //NA PODSTAWIE STWORZONEJ TRASY ZROB MIEJCA GDZIE CHUNKI BEDA POLACZONE ZE SOBA
        {
            GenConnections(i);
        }

        StartCoroutine(ChunkPathGeneration());

    }

    IEnumerator ChunkPathGeneration()
    {
        yield return new WaitForSeconds(2);
        for (int q = 0; q < Path.Count; q++)
        {
            Vector3 vec = Path[q].transform.position;
            Vector3Int pos = grid.WorldToCell(vec);
            //Debug.Log(Path[q].name + pos);
            Path[q].GetComponent<ChunkReveal2>().SetChunkSize(chunkSize);
            Path[q].GetComponent<ChunkReveal2>().Generate();
        }
    }

    /// ////////////

    void GenConnections(int i)  //WYGENERUJ POLACZENIA MIEDZY CHUNKAMI
    {
        //stare
        //int xPrev = (int)Path[i - 1].transform.position.x;
        //int xAcc = (int)Path[i].transform.position.x;

        //int yPrev = (int)Path[i - 1].transform.position.z;
        //int yAcc = (int)Path[i].transform.position.z;

        //nowe
        Vector3 Previous = Path[i-1].transform.position; 
        Vector3 Actual = Path[i].transform.position;

        Vector3Int PreviousGridPos = grid.WorldToCell(Previous);
        Vector3Int ActualGridPos = grid.WorldToCell(Actual);

        int xPrev = PreviousGridPos.x;
        int yPrev = PreviousGridPos.z;

        int xAcc = ActualGridPos.x;
        int yAcc = ActualGridPos.z;

        int Rand = Random.Range(1, chunkSize-1);

        if (xPrev < xAcc)
        {
            Vector3 ObjectSpawn1 = new Vector3(xPrev + chunkSize-1, elevation, yPrev + Rand);
            Vector3 ObjectSpawn2 = new Vector3(xPrev + chunkSize+1, elevation, yPrev + Rand );

            Vector3Int a1 = grid.WorldToCell(ObjectSpawn1);
            Vector3Int a2 = grid.WorldToCell(ObjectSpawn2);

            Vector3 cellCenterPosition1 = grid.GetCellCenterWorld(a1);
            Vector3 cellCenterPosition2 = grid.GetCellCenterWorld(a2);

            GameObject cn1 = Instantiate(Connector, cellCenterPosition1, Quaternion.identity);
            GameObject cn2 = Instantiate(Connector, cellCenterPosition2, Quaternion.identity);

            if (i == 1)
            {
                Vector3 ObjectSpawn3 = new Vector3(xPrev, elevation, yPrev + Rand);
                Vector3Int a3 = grid.WorldToCell(ObjectSpawn3);
                Vector3 cellCenterPosition3 = grid.GetCellCenterWorld(a3);

                GameObject cn3 = Instantiate(Connector, cellCenterPosition3, Quaternion.identity);
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
        }

        if (xPrev > xAcc)
        {
            Vector3 ObjectSpawn1 = new Vector3(xAcc + chunkSize - 1, elevation, yPrev + Rand);
            Vector3 ObjectSpawn2 = new Vector3(xAcc + chunkSize + 1, elevation, yPrev + Rand);

            Vector3Int a1 = grid.WorldToCell(ObjectSpawn1);
            Vector3Int a2 = grid.WorldToCell(ObjectSpawn2);

            Vector3 cellCenterPosition1 = grid.GetCellCenterWorld(a1);
            Vector3 cellCenterPosition2 = grid.GetCellCenterWorld(a2);

            GameObject cn1 = Instantiate(Connector, cellCenterPosition1, Quaternion.identity);
            GameObject cn2 = Instantiate(Connector, cellCenterPosition2, Quaternion.identity);
            if (i == 1)
            {

                Vector3 ObjectSpawn3 = new Vector3(xPrev + chunkSize - 1, elevation, yPrev);
                Vector3Int a3 = grid.WorldToCell(ObjectSpawn3);
                Vector3 cellCenterPosition3 = grid.GetCellCenterWorld(a3);


                GameObject cn3 = Instantiate(Connector, cellCenterPosition3, Quaternion.identity);
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

            /*
            if (i == Path.Count - 1)
            {
                GameObject end = Instantiate(Connector, new Vector3(xAcc + 4.5f, elevation, yPrev - Rand - 0.5f), Quaternion.identity);
                end.name = "END ";
                connCount++;
                Connectors.Add(end);
            }
            */
        }

        if (yPrev < yAcc)
        {

            Vector3 ObjectSpawn1 = new Vector3(xPrev + Rand, elevation, yPrev + chunkSize - 1);
            Vector3 ObjectSpawn2 = new Vector3(xPrev + Rand, elevation, yPrev + chunkSize + 1);

            Vector3Int a1 = grid.WorldToCell(ObjectSpawn1);
            Vector3Int a2 = grid.WorldToCell(ObjectSpawn2);

            Vector3 cellCenterPosition1 = grid.GetCellCenterWorld(a1);
            Vector3 cellCenterPosition2 = grid.GetCellCenterWorld(a2);

            GameObject cn1 = Instantiate(Connector, cellCenterPosition1, Quaternion.identity);
            GameObject cn2 = Instantiate(Connector, cellCenterPosition2, Quaternion.identity);

            if (i == 1)
            {

                Vector3 ObjectSpawn3 = new Vector3(xPrev + Rand, elevation, yPrev);
                Vector3Int a3 = grid.WorldToCell(ObjectSpawn3);
                Vector3 cellCenterPosition3 = grid.GetCellCenterWorld(a3);
                // new Vector3(xPrev - Rand - 0.5f, elevation, yPrev - 4.5f)

                GameObject cn3 = Instantiate(Connector, cellCenterPosition3, Quaternion.identity);
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
                Vector3 ObjectSpawn3 = new Vector3(xAcc + Rand, elevation, yAcc + chunkSize -1);
                Vector3Int a3 = grid.WorldToCell(ObjectSpawn3);
                Vector3 cellCenterPosition3 = grid.GetCellCenterWorld(a3);
                // new Vector3(xPrev - Rand - 0.5f, elevation, yAcc + 4.5f)

                GameObject end = Instantiate(Connector, cellCenterPosition3, Quaternion.identity);
                end.name = "END ";
                end.tag = "end";
                connCount++;
                Connectors.Add(end);
            }
        }
    }
}

