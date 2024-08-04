using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBuy : MonoBehaviour
{
    public GameObject Chunk;
    public GameObject Manager;

    private void Start()
    {
        Manager = GameObject.Find("manager");
    }
    private void OnMouseUpAsButton()
    {
        if (Manager.GetComponent<RayCastFromCamera>().money > 100)
        {
            Manager.GetComponent<RayCastFromCamera>().money -= 100;

            if (Chunk.GetComponent<ChunkReveal2>() != null)
            {
                Chunk.GetComponent<ChunkReveal2>().Buy();
            }


            Chunk.GetComponent<ColorChanger>().ChangeCol();
            Destroy(this.gameObject);
        }
    }
}
