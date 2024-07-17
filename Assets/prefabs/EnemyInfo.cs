using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
	public int hp;
	public int speed;
	public int defence;

	public ParticleSystem ps;

	private void Update()
	{

	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("end"))
		{
			Destroy(this.gameObject);
		}

		if (other.CompareTag("bullet"))
		{
			hp -= other.GetComponent<BulletMovement>().damage;
			Destroy(other.gameObject);
			Debug.Log(hp);

			if (hp <= 0)
			{
				
				//ps.Play();
				Destroy(gameObject);
			}
		}
	}
}
