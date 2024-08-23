using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{ 
    [SerializeField] float BulletSpeed;
    public int damage;
	public string Type;
	public Transform enemy;
	public TowerStats ts;
	private Vector3 LastDir;
    void Start()
    {
        Destroy(gameObject,2);
    }

    void FixedUpdate()
    {
		if (enemy != null)
		{
			Vector3 direction = (enemy.position - transform.position).normalized;

            transform.position += direction * BulletSpeed * Time.deltaTime;
            //transform.position = Vector3.Lerp(transform.position, transform.position + direction * BulletSpeed * Time.deltaTime, 0.5f);

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
