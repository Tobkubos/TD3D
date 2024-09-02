using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    public string name;
    public string desc;
    [SerializeField] float hp;
    [SerializeField] bool dodge;
    [SerializeField] float speed;
    [SerializeField] int cash;

    public Slider hpBar;

    public bool OnFire;
    public bool OnStun;

    public bool Armored;
    public bool Stunnable;

    public GameObject NaturalStun;
    public GameObject StunParent;

    public ParticleSystem ps;

    public float distanceTravelled = 0f;
    private Vector3 lastPosition;

    private int ChanceOfHit;
    private bool canBeHit = true;

    private Color originalColor;

    public void ModifySpeed(float upgrade)
    {
        GetComponent<NavMeshAgent>().speed += upgrade;
    }
    public void ModifyHp(float upgrade)
    {
        hp += upgrade;
    }
    public void ModifyCash(int upgrade)
    {
        cash += upgrade;
    }

    void Start()
    {
        GetComponent<NavMeshAgent>().speed = speed;
        originalColor = this.GetComponent<Renderer>().material.color;
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
        for (int i = 0; i < 20; i++)
        {
            Debug.Log("PALE SIE");
            yield return new WaitForSeconds(0.25f);
            DealDamageOverTime(ts.GetDamageOverTime()/20, ts);
        }
        yield return new WaitForSeconds(1);

        gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        gameObject.GetComponent<Renderer>().material.color = originalColor;
        OnFire = false;
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("end"))
		{
            GameObject.Find("manager").GetComponent<RayCastFromCamera>().lives--;
            Destroy(this.gameObject);
		}

        if (dodge)
        {
            ChanceOfHit = Random.Range(0, 100);
            if (ChanceOfHit < 40)
            {
                canBeHit = false;
            }
            else 
            {
                canBeHit = true;
            }
        }

		if (other.CompareTag("bullet") && canBeHit)
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
                        if (Stunnable)
                        {
                            OnStun = true;
                            StartCoroutine(Stun());
                        }
                        DealDamage(other.GetComponent<BulletMovement>().Elementaldamage);
                    }
                    if(OnStun)
                    {
                        Destroy(other.gameObject);
                    }
                }

                if (!Armored)
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
