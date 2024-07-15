using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{ 
    [SerializeField] float BulletSpeed;
    void Start()
    {
        Destroy(gameObject,5);
    }

    void FixedUpdate()
    {
        transform.position -= transform.right * Time.fixedDeltaTime * BulletSpeed;
    }
}
