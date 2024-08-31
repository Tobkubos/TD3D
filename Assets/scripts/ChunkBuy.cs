using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChunkBuy : MonoBehaviour
{
    public GameObject Chunk;
    public GameObject ChunkPlane;
    public GameObject Manager;
    public TextMeshPro price;
    private GeneratorV4 generator;
    private void Start()
    {
        ChunkPlane.tag = "notBought";
        Manager = GameObject.Find("manager");
        generator = Manager.GetComponent<GeneratorV4>();
        transform.localPosition = new Vector3(Manager.GetComponent<GeneratorV4>().chunkSize / 2, 1, Manager.GetComponent<GeneratorV4>().chunkSize / 2);
        price.text = "price: " + Manager.GetComponent<RayCastFromCamera>().ChunkPrice.ToString();
    }
    private void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Manager.GetComponent<RayCastFromCamera>().HologramTower == false)
        {
            if (Manager.GetComponent<RayCastFromCamera>().money >= Manager.GetComponent<RayCastFromCamera>().ChunkPrice)
            {
                Manager.GetComponent<RayCastFromCamera>().money -= Manager.GetComponent<RayCastFromCamera>().ChunkPrice;
                Manager.GetComponent<RayCastFromCamera>().ChunkPrice += 65;

                foreach(GameObject go in generator.Chunk)
                {
                    if(go.GetComponentInChildren<ChunkBuy>() != null)
                    {
                        go.GetComponentInChildren<ChunkBuy>().price.text = "price: " + Manager.GetComponent<RayCastFromCamera>().ChunkPrice.ToString();
                    }
                }

                foreach (GameObject go in generator.EmptyChunks)
                {
                    if (go.GetComponentInChildren<ChunkBuy>() != null)
                    {
                        go.GetComponentInChildren<ChunkBuy>().price.text = "price: " + Manager.GetComponent<RayCastFromCamera>().ChunkPrice.ToString();
                    }
                }



                if (Chunk.GetComponent<ChunkReveal2>() != null)
                {
                    Chunk.GetComponent<ChunkReveal2>().Buy();
                }



                Chunk.GetComponent<ColorChanger>().ChangeCol();
                //ChunkPlane.tag = "chunk";
                Destroy(this.gameObject);
            }
        }
    }
}
