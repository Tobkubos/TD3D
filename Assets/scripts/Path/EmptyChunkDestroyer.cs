using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyChunkDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
