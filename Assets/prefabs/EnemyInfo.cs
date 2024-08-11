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

    public float distanceTravelled = 0f;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        // distance till start
        distanceTravelled += Vector3.Distance(lastPosition, transform.position);
        lastPosition = transform.position;
    }


    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("end"))
		{
			Destroy(this.gameObject);
		}

		if (other.CompareTag("bullet"))
		{
			if (other.GetComponent<BulletMovement>().enemy == this.transform)
			{
				hp -= other.GetComponent<BulletMovement>().damage;

			Destroy(other.gameObject);
			}

			//Debug.Log(hp);

			if (hp <= 0)
			{
				ps.transform.parent = null;
				ps.Play();
				Destroy(ps.gameObject, 2);

                if (other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().GetExperience() < other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().GetMaxExp())
                {
                    other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().SetExperience(); //give experience
                }

				GameObject.Find("manager").GetComponent<RayCastFromCamera>().money += cash;


                Destroy(gameObject);
			}
		}
	}
}
