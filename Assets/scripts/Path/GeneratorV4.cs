using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class GeneratorV4 : MonoBehaviour
{
	private List<GameObject> ChunkCheckPoints = new List<GameObject> { };    
	private List<GameObject> Chunk = new List<GameObject> { };                   
	private List<GameObject> Connectors = new List<GameObject> { };

    private int[][] map;

	public GameObject ChunkCheckPoint; //obiekt chunku z mesh generatorem
    public GameObject EmptyChunk;      //obiekt chunku z mesh generatorem
    public GameObject Spawner;         //miejsce spawnu potworów
	public GameObject Connector;       //przejœcie
    public Grid grid;                  //siatka gry


    public int SizeOfMap = 4;          //wielkoœæ mapy w chunkach
    public int chunkSize = 5;          //wielkoœæ chunku

	private int x,z;           
	private int count = 0;
	private int connCount = 1;
	private float elevation = 0;
    private Vector3 cordinate;


    private int index = 0;
	void Start()
    {
        for (int i = 0; i < SizeOfMap; i++)
        {
            for (int j = 0; j < SizeOfMap; j++)
            {
                GameObject EmptyCh = Instantiate(EmptyChunk, new Vector3(i * (chunkSize + 1), elevation, j * (chunkSize + 1)), Quaternion.identity);
                EmptyCh.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
                EmptyCh.GetComponent<MeshGenerator>().GenerateMesh();
            }
        }
      GenerateChunks();
	}
	public void Reset2()
	{
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(Chunk[index].GetComponent<ChunkReveal2>().BuyChunk());
        index++;
    }
	private void GenerateChunks()
	{

        //generowanie punktów zaczepenia chunków
        for (int i = 0; i < SizeOfMap; i++)
        {
            x = Random.Range(0, SizeOfMap);
            GameObject first = Instantiate(ChunkCheckPoint, new Vector3(x * (chunkSize + 1), elevation, i * (chunkSize + 1)), Quaternion.identity);
			first.GetComponent<MeshGenerator>().SetSize(chunkSize,chunkSize);
			first.GetComponent<MeshGenerator>().GenerateMesh();
            ChunkCheckPoints.Add(first);
        }

        ChunkCheckPoints = ChunkCheckPoints.OrderBy(obj => obj.transform.position.z).ToList();


        //tworzenie chunków na podstawie checkpointów
        for (int i = 1; i < ChunkCheckPoints.Count; i++)
        {
            int xPrev = (int)ChunkCheckPoints[i - 1].transform.position.x;
            int zPrev = (int)ChunkCheckPoints[i - 1].transform.position.z;
            
            int xAct = (int)ChunkCheckPoints[i].transform.position.x;
            int zAct = (int)ChunkCheckPoints[i].transform.position.z;

            int DistX = xAct - xPrev;
            int DistY = zAct - zPrev;

            if (DistX > 0)
            {
                for (int j = xPrev; j < xPrev + DistX; j += chunkSize+1)
                {
                    GameObject temp = Instantiate(ChunkCheckPoint, new Vector3(j, elevation, zPrev), Quaternion.identity);
                    temp.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
                    temp.GetComponent<MeshGenerator>().GenerateMesh();
                    temp.GetComponent<ChunkReveal2>().index = count;
                    temp.name = "SCIEZKA" + count;
                    count++;
                    Chunk.Add(temp);
                }

            }
            if (DistX < 0)
            {
                for (int j = xPrev; j > xPrev + DistX; j -= chunkSize + 1)
                {
                    GameObject temp = Instantiate(ChunkCheckPoint, new Vector3(j, elevation, zPrev), Quaternion.identity);
                    temp.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
                    temp.GetComponent<MeshGenerator>().GenerateMesh();
                    temp.name = "SCIEZKA" + count;
                    count++;
                    Chunk.Add(temp);
                }

            }
            if (DistY > 0)
            {
                for (int j = zPrev; j < zPrev + DistY; j += chunkSize + 1)
                {
                    GameObject temp = Instantiate(ChunkCheckPoint, new Vector3(xAct, elevation, j), Quaternion.identity);
                    temp.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
                    temp.GetComponent<MeshGenerator>().GenerateMesh();
                    temp.name = "SCIEZKA" + count;
                    count++;
                    Chunk.Add(temp);
                }

            }
            if (DistY < 0)
            {
                for (int j = zPrev; j > zPrev + DistY; j -= chunkSize + 1)
                {
                    GameObject temp = Instantiate(ChunkCheckPoint, new Vector3(xAct, elevation, j), Quaternion.identity);
                    temp.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
                    temp.GetComponent<MeshGenerator>().GenerateMesh();
                    temp.name = "SCIEZKA" + count;
                    count++;
                    Chunk.Add(temp);
                }

            }
        }
        
        //OSTATNI CHUNK
        GameObject lastChunk = Instantiate(ChunkCheckPoint, new Vector3(ChunkCheckPoints[ChunkCheckPoints.Count - 1].transform.position.x, elevation, ChunkCheckPoints[ChunkCheckPoints.Count - 1].transform.position.z), Quaternion.identity);
        lastChunk.GetComponent<MeshGenerator>().SetSize(chunkSize, chunkSize);
        lastChunk.GetComponent<MeshGenerator>().GenerateMesh();
        lastChunk.name = "SCIEZKA" + count;
        count++;
        Chunk.Add(lastChunk);
        
        foreach (GameObject go in ChunkCheckPoints)      //USUÑ PUNKTY ZACZEPIENIA TRASY
        {
            Destroy(go);
        }
        ChunkCheckPoints = new List<GameObject> { };     //WYZERUJ W RAZIE CZEGO

        for(int i = 0;  i < Chunk.Count; i++)
        {
            if (i+1 < Chunk.Count)
            {
                GenConnections(i + 1);
            }
            Chunk[i].GetComponent<ChunkReveal2>().SetChunkSize(chunkSize);
            Chunk[i].GetComponent<ChunkReveal2>().Generate();
        }
        
        gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();

        for (int i = 0; i < Chunk.Count; i++)
        {
            Chunk[i].GetComponent<ChunkReveal2>().Disappear();
        }


        Connectors[0].GetComponent<Spawner>().enemy.GetComponent<SimpleMovement>().end = Connectors[Connectors.Count-1];
        Connectors[0].GetComponent<Spawner>().enabled = true;
    }

    /// ////////////
    void GenConnections(int i)  //WYGENERUJ POLACZENIA MIEDZY CHUNKAMI
    {

            Vector3 Previous = Chunk[i - 1].transform.position;
            Vector3 Actual = Chunk[i].transform.position;

            Vector3Int PreviousGridPos = grid.WorldToCell(Previous);
            Vector3Int ActualGridPos = grid.WorldToCell(Actual);

            int xPrev = PreviousGridPos.x;
            int zPrev = PreviousGridPos.z;

            int xAct = ActualGridPos.x;
            int zAct = ActualGridPos.z;

            int Rand = Random.Range(1, chunkSize - 1);

            if (xPrev < xAct)
            {
                Vector3 ObjectSpawn1 = new Vector3(xPrev + chunkSize - 1, elevation, zPrev + Rand);
                Vector3 ObjectSpawn2 = new Vector3(xPrev + chunkSize + 1, elevation, zPrev + Rand);

                Vector3Int a1 = grid.WorldToCell(ObjectSpawn1);
                Vector3Int a2 = grid.WorldToCell(ObjectSpawn2);

                Vector3 cellCenterPosition1 = grid.GetCellCenterWorld(a1);
                Vector3 cellCenterPosition2 = grid.GetCellCenterWorld(a2);

                GameObject cn1 = Instantiate(Connector, cellCenterPosition1, Quaternion.identity, Chunk[i-1].transform);
                Chunk[i-1].GetComponent<ChunkReveal2>().StartEnd[1] = cn1;
                cn1.tag = "end";
                GameObject cn2 = Instantiate(Connector, cellCenterPosition2, Quaternion.identity, Chunk[i].transform);
                Chunk[i].GetComponent<ChunkReveal2>().StartEnd[0] = cn2;
                cn2.tag = "start";

                if (i == 1)
                {
                    Vector3 ObjectSpawn3 = new Vector3(xPrev, elevation, zPrev + Rand);
                    Vector3Int a3 = grid.WorldToCell(ObjectSpawn3);
                    Vector3 cellCenterPosition3 = grid.GetCellCenterWorld(a3);

                    GameObject cn3 = Instantiate(Spawner, cellCenterPosition3, Quaternion.identity, Chunk[i - 1].transform);
                    Chunk[i - 1].GetComponent<ChunkReveal2>().StartEnd[0] = cn3;
                    cn3.name = "CONNECTOR " + connCount;
                    cn3.tag = "start";
                    connCount++;
                    Connectors.Add(cn3);
                }

                cn1.name = "CONNECTOR " + connCount;

                connCount++;
                cn2.name = "CONNECTOR " + connCount;
                connCount++;
                Connectors.Add(cn1);
                Connectors.Add(cn2);
            }

            if (xPrev > xAct)
            {
                Vector3 ObjectSpawn1 = new Vector3(xAct + chunkSize - 1, elevation, zPrev + Rand);
                Vector3 ObjectSpawn2 = new Vector3(xAct + chunkSize + 1, elevation, zPrev + Rand);

                Vector3Int a1 = grid.WorldToCell(ObjectSpawn1);
                Vector3Int a2 = grid.WorldToCell(ObjectSpawn2);

                Vector3 cellCenterPosition1 = grid.GetCellCenterWorld(a1);
                Vector3 cellCenterPosition2 = grid.GetCellCenterWorld(a2);

                GameObject cn1 = Instantiate(Connector, cellCenterPosition1, Quaternion.identity, Chunk[i].transform);
                Chunk[i].GetComponent<ChunkReveal2>().StartEnd[0] = cn1;
                cn1.tag = "start";
                GameObject cn2 = Instantiate(Connector, cellCenterPosition2, Quaternion.identity, Chunk[i - 1].transform);
                Chunk[i - 1].GetComponent<ChunkReveal2>().StartEnd[1] = cn2;
                cn2.tag = "end";

                if (i == 1)
                {

                    Vector3 ObjectSpawn3 = new Vector3(xPrev + chunkSize - 1, elevation, zPrev);
                    Vector3Int a3 = grid.WorldToCell(ObjectSpawn3);
                    Vector3 cellCenterPosition3 = grid.GetCellCenterWorld(a3);

                    GameObject cn3 = Instantiate(Spawner, cellCenterPosition3, Quaternion.identity, Chunk[i - 1].transform);
                    Chunk[i - 1].GetComponent<ChunkReveal2>().StartEnd[0] = cn3;
                    cn3.name = "CONNECTOR " + connCount;
                    cn3.tag = "start";
                    connCount++;
                    Connectors.Add(cn3);
                }
                cn2.name = "CONNECTOR " + connCount;
                connCount++;
                cn1.name = "CONNECTOR " + connCount;
                connCount++;

                Connectors.Add(cn2);
                Connectors.Add(cn1);
            }

            if (zPrev < zAct)
            {

                Vector3 ObjectSpawn1 = new Vector3(xPrev + Rand, elevation, zPrev + chunkSize - 1);
                Vector3 ObjectSpawn2 = new Vector3(xPrev + Rand, elevation, zPrev + chunkSize + 1);

                Vector3Int a1 = grid.WorldToCell(ObjectSpawn1);
                Vector3Int a2 = grid.WorldToCell(ObjectSpawn2);

                Vector3 cellCenterPosition1 = grid.GetCellCenterWorld(a1);
                Vector3 cellCenterPosition2 = grid.GetCellCenterWorld(a2);

                GameObject cn1 = Instantiate(Connector, cellCenterPosition1, Quaternion.identity, Chunk[i - 1].transform);
                Chunk[i - 1].GetComponent<ChunkReveal2>().StartEnd[1] = cn1;
                cn1.tag = "end";
                GameObject cn2 = Instantiate(Connector, cellCenterPosition2, Quaternion.identity, Chunk[i].transform);
                Chunk[i].GetComponent<ChunkReveal2>().StartEnd[0] = cn2;
                cn2.tag = "start";

                if (i == 1)
                {

                    Vector3 ObjectSpawn3 = new Vector3(xPrev + Rand, elevation, zPrev);
                    Vector3Int a3 = grid.WorldToCell(ObjectSpawn3);
                    Vector3 cellCenterPosition3 = grid.GetCellCenterWorld(a3);

                    GameObject cn3 = Instantiate(Spawner, cellCenterPosition3, Quaternion.identity, Chunk[i - 1].transform);
                    Chunk[i - 1].GetComponent<ChunkReveal2>().StartEnd[0] = cn3;
                    cn3.name = "CONNECTOR " + connCount;
                    cn3.tag = "start";
                    connCount++;
                    Connectors.Add(cn3);
                }
                cn1.name = "CONNECTOR " + connCount;
                connCount++;
                cn2.name = "CONNECTOR " + connCount;
                connCount++;
                Connectors.Add(cn1);
                Connectors.Add(cn2);
                if (i == Chunk.Count - 1)
                {
                    Vector3 ObjectSpawn3 = new Vector3(xAct + Rand, elevation, zAct + chunkSize - 1);
                    Vector3Int a3 = grid.WorldToCell(ObjectSpawn3);
                    Vector3 cellCenterPosition3 = grid.GetCellCenterWorld(a3);

                    GameObject end = Instantiate(Connector, cellCenterPosition3, Quaternion.identity, Chunk[i].transform);
                    Chunk[i].GetComponent<ChunkReveal2>().StartEnd[1] = end;
                    end.name = "END ";
                    end.tag = "end";
                    connCount++;
                    Connectors.Add(end);
                }
            }
        
    }
}

