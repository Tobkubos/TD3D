using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{ 
    [SerializeField] float BulletSpeed;
    public int damage;
    void Start()
    {
        Destroy(gameObject,2);
    }

    void FixedUpdate()
    {
        transform.position -= transform.right * Time.fixedDeltaTime * BulletSpeed;
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("enemy"))
        {
            other.GetComponent<EnemyInfo>().hp -= damage;
            Debug.Log(other.GetComponent<EnemyInfo>().hp);
        }
	}
}
