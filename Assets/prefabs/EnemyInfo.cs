using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
	[SerializeField] int hp;
	public int speed;
	public int defence;
	public int cash;
    public Slider hpBar;

    public ParticleSystem ps;

    public float distanceTravelled = 0f;
    private Vector3 lastPosition;

    void Start()
    {
		hpBar.maxValue = hp;
        lastPosition = transform.position;
    }

    void Update()
    {
        // distance till start
        distanceTravelled += Vector3.Distance(lastPosition, transform.position);
        lastPosition = transform.position;


        if (hpBar != null)
        {
            hpBar.transform.rotation = Quaternion.Euler(55f, 45f, 0f);
			hpBar.value = hp;
        }

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
				ps.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
				ps.Play();
				Destroy(ps.gameObject, 2);

                /*
                if (other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().GetExperience() < other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().GetMaxExp())
                {
                    other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().SetExperience(); //give experience
                }
                */
                other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().SetExperience();

                GameObject.Find("manager").GetComponent<RayCastFromCamera>().money += cash;


                Destroy(gameObject);
			}
		}
	}

	public bool DealDamage(int damage) {
        hp -= damage;
        if (hp <= 0)
        {
            ps.transform.parent = null;
            ps.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
            ps.Play();
            Destroy(ps.gameObject, 2);
            GameObject.Find("manager").GetComponent<RayCastFromCamera>().money += cash;
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
