using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
	[SerializeField] int hp;
	public int speed;
	public int defence;
	public int cash;

	public ParticleSystem ps;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("end"))
		{
			Destroy(this.gameObject);
		}

		if (other.CompareTag("bullet"))
		{
			hp -= other.GetComponent<BulletMovement>().damage;

			if (other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().GetExperience() < other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().GetMaxExp())
			{
				other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().SetExperience(); //give experience
			}


			Destroy(other.gameObject);
			Debug.Log(hp);

			if (hp <= 0)
			{
				ps.transform.parent = null;
				ps.Play();
				Destroy(ps.gameObject, 2);



				Destroy(gameObject);
			}
		}
	}
}
