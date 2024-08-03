using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnim : MonoBehaviour
{
    public void SpawnAnimation()
    {
        this.gameObject.transform.localScale= Vector3.zero;
        LeanTween.scale(this.gameObject, new Vector3(1, 0.1f, 1),0.3f);
    }
}
