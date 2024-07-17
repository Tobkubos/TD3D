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
			// Oblicz kierunek w stron� enemy
			Vector3 direction = (enemy.position - transform.position).normalized;

			// Przesu� pocisk w stron� enemy
			transform.position += direction * BulletSpeed * Time.deltaTime;

			// Obr�� pocisk w stron� enemy
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
