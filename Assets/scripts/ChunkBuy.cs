using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBuy : MonoBehaviour
{
    public GameObject Chunk;
    public GameObject ChunkPlane;
    public GameObject Manager;

    private void Start()
    {
        ChunkPlane.tag = "notBought";
        Manager = GameObject.Find("manager");
        transform.localPosition = new Vector3(Manager.GetComponent<GeneratorV4>().chunkSize / 2, 1, Manager.GetComponent<GeneratorV4>().chunkSize / 2);
    }
    private void OnMouseUpAsButton()
    {
        if (Manager.GetComponent<RayCastFromCamera>().money > 100)
        {
            Manager.GetComponent<RayCastFromCamera>().money -= 100;
            ChunkPlane.tag = "chunk";
            if (Chunk.GetComponent<ChunkReveal2>() != null)
            {
                Chunk.GetComponent<ChunkReveal2>().Buy();
            }


            Chunk.GetComponent<ColorChanger>().ChangeCol();
            Destroy(this.gameObject);
        }
    }
}
