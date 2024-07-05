using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkReveal : MonoBehaviour
{
	private List<GameObject> AllChunkObjects = new List<GameObject> { };
	List<GameObject> StartEnd = new List<GameObject> { };
	List<GameObject> CheckPoints = new List<GameObject> { };
	public GameObject CheckPoint;
	private int PD = 1;
	private int count = 1;

	private void OnTriggerEnter(Collider other)
	{
		AllChunkObjects.Add(other.gameObject);
		//other.gameObject.SetActive(false);
		if (other.CompareTag("start"))
		{
			//Debug.Log(other);
			StartEnd.Add(other.gameObject);
			Destroy(other.GetComponent<Rigidbody>());
		}

		if (other.CompareTag("end"))
		{
			StartEnd.Add(other.gameObject);
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
		for(int i = 0; i<CheckPoints.Count; i++)
		{
			Debug.Log(CheckPoints[i]);
		}
		Debug.Log("================");


		while(CheckPoints.Count < 3) {

			int x = Random.Range(-5, 5) + (int)this.gameObject.transform.position.x;
			int z = Random.Range(-5, 5) + (int)this.gameObject.transform.position.z;
			bool canPlace = true;
			foreach (GameObject go in CheckPoints)
			{
				int goX = (int)go.transform.position.x;
				int goZ = (int)go.transform.position.z;
				if ((goX < x + PD && goX > x - PD) || (goZ < z + PD && goZ > z - PD))
				{
					canPlace = false;
				}
			}
			if (canPlace)
			{
				GameObject temp = Instantiate(CheckPoint, new Vector3(x + 0.5f, -5, z + 0.5f), Quaternion.identity);
				temp.name = "CheckPoint" + count;
				count++;
				CheckPoints.Add(temp);
			}
		}
	}
}
