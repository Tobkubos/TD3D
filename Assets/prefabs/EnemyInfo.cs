using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
	[SerializeField] float hp;
	public int speed;
	public int defence;
	public int cash;
    public Slider hpBar;
    public bool OnFire;
    public bool OnStun;
    public GameObject NaturalStun;
    public GameObject StunParent;

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

    IEnumerator Fire(TowerStats ts)
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("PALE SIE");
            yield return new WaitForSeconds(1f);
            DealDamageOverTime(1f, ts);
        }
        yield return new WaitForSeconds(1);

        gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        OnFire = false;
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("end"))
		{
            GameObject.Find("manager").GetComponent<RayCastFromCamera>().lives--;
            Destroy(this.gameObject);
		}

		if (other.CompareTag("bullet"))
		{
			if (other.GetComponent<BulletMovement>().enemy == this.transform)
			{
                if (other.GetComponent<BulletMovement>().Type == "Fire") {
                    if (!OnFire)
                    {
                        if (this.gameObject != null)
                        {
                            gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                            gameObject.GetComponent<Renderer>().material.color = Color.red;
                            OnFire = true;
                            StartCoroutine(Fire(other.GetComponent<BulletMovement>().ts));
                        }
                    }

                    if (OnFire)
                    {
                        Destroy(other.gameObject);
                    }
                    //
                    foreach (Transform child in other.transform)
                    {
                        Vector3 originalScale = child.localScale;
                        child.parent = null;
                        child.localScale = originalScale;
                        if (child.GetComponent<ParticleSystem>())
                        {
                            child.GetComponent<ParticleSystem>().Stop();
                        }
                        else
                        {
                            LeanTween.scale(child.gameObject, Vector3.zero, 0.3f);
                        }
                        Destroy(child.gameObject, 2f);
                    }
                }
                //
                if (other.GetComponent<BulletMovement>().Type == "Nature")
                {
                    if (!OnStun)
                    {
                        OnStun = true;
                        DealDamage(other.GetComponent<BulletMovement>().damage);
                        StartCoroutine(Stun());
                    }
                    if(OnStun)
                    {
                        Destroy(other.gameObject);
                    }
                }

                if (other.GetComponent<BulletMovement>().Type == "Normal")
                {
                    hp -= other.GetComponent<BulletMovement>().damage;
                }
                Destroy(other.gameObject);
            }

			if (hp <= 0)
			{
				ps.transform.parent = null;
				ps.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
				ps.Play();
				Destroy(ps.gameObject, 2);

                other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().SetExperience();

                GameObject.Find("manager").GetComponent<RayCastFromCamera>().money += cash;


                Destroy(gameObject);
			}
		}
	}

	public bool DealDamage(float damage) {
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

    public void DealDamageOverTime(float damage, TowerStats ts)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Debug.Log("Spali³em sie");
            ts.SetExperience();

            ps.transform.parent = null;
            ps.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
            ps.Play();
            Destroy(ps.gameObject, 2);
            GameObject.Find("manager").GetComponent<RayCastFromCamera>().money += cash;
            Destroy(gameObject);
        }
    }

    public IEnumerator Stun()
    {
        GameObject NS = Instantiate(NaturalStun, transform.position, Quaternion.Euler(transform.rotation.x, Random.Range(0,360), transform.rotation.z), StunParent.transform);
        this.gameObject.GetComponent<NavMeshAgent>().speed = 0;
        yield return new WaitForSeconds(5);
        LeanTween.scale(NS, Vector3.zero, 0.3f);
        Destroy(NS, 0.3f);
        GetComponent<NavMeshAgent>().speed = speed;
        OnStun = false;
    }
}
