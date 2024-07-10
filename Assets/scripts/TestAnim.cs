using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    void Start()
    {
        Vector3 scale = transform.localScale;
        this.gameObject.transform.localScale= Vector3.zero;
        LeanTween.scale(this.gameObject, scale,0.3f);
    }
}
