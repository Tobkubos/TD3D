using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{ 
    [SerializeField] float BulletSpeed;
    public int damage;
	public Transform enemy;
	private Vector3 LastDir;
    void Start()
    {
        Destroy(gameObject,2);
    }

    void FixedUpdate()
    {
        //transform.position -= transform.right * Time.fixedDeltaTime * BulletSpeed;

		if (enemy != null)
		{
			// Oblicz kierunek w stronê enemy
			Vector3 direction = (enemy.position - transform.position).normalized;

			// Przesuñ pocisk w stronê enemy
			transform.position += direction * BulletSpeed * Time.deltaTime;

			// Obróæ pocisk w stronê enemy
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

			LastDir = direction;
		}
		else
		{
			transform.position += LastDir * BulletSpeed * Time.deltaTime;
		}
	}
}
