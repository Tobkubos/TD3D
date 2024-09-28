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
    [SerializeField] bool Boss;
    private float maxhp;
    public Slider hpBar;

    public bool OnFire;
    public bool OnStun;

    public bool Armored;
    public bool Stunnable;

    public bool BossAttack;
    public List<GameObject> BossTowersToAttack = new List<GameObject>();
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
    public float GetHp()
    {
        return hp;
    }
    void Start()
    {
        maxhp = hp - 1000;
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
        //Debug.Log(hp);

        if (hpBar != null)
        {
            hpBar.transform.rotation = Quaternion.Euler(55f, 45f, 0f);
			hpBar.value = hp;
        }

        
        if(Boss == true && hp < maxhp)
        {
            BossTowersToAttack.Clear();
            BossAttack = true;
            GetComponent<NavMeshAgent>().speed = 0;
            StartCoroutine(BossAttack1());
            maxhp -= 1000;
            BossAttack = false;
        }
        
    }
    IEnumerator BossAttack1()
    {
        float time = 20f;
        GetComponent<SimpleMovement>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        LeanTween.moveLocalY(this.gameObject, this.gameObject.transform.position.y + 5, 1f).setEase(LeanTweenType.easeInOutSine);
        yield return new WaitForSeconds(1.1f);
        LeanTween.moveLocalY(this.gameObject, this.gameObject.transform.position.y - 5, 1f).setEase(LeanTweenType.easeInOutSine);
        //1.1f
        yield return new WaitForSeconds(0.9f);
        foreach (GameObject tower in BossTowersToAttack) 
        {
            if(tower.GetComponentInChildren<TowerStats>().CanShoot != false)
            {
                StartCoroutine(tower.GetComponentInChildren<TowerStats>().BlockVisualizer(time));
                tower.GetComponentInChildren<TowerStats>().CanShoot = false;
            }
        }
        //2f

        GameObject.Find("GroundAttack").GetComponent<ParticleSystem>().Play();
        GetComponent<NavMeshAgent>().speed = speed;
        GetComponent<SimpleMovement>().enabled = true;
        GetComponent<NavMeshAgent>().enabled = true;


        yield return new WaitForSeconds(time);

        foreach (GameObject tower in BossTowersToAttack)
        {
            tower.GetComponentInChildren<TowerStats>().CanShoot = true;
        }
    }
    IEnumerator Fire(BulletMovement bullet, TowerStats ts)
    {
        for (int i = 0; i < 20; i++)
        {
            //Debug.Log("PALE SIE");
            yield return new WaitForSeconds(0.2f);
            DealDamageOverTime(bullet.DamageOverTime/20, ts);
        }
        yield return new WaitForSeconds(1);

        gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        gameObject.GetComponent<Renderer>().material.color = originalColor;
        OnFire = false;
    }

    private void OnTriggerEnter(Collider other)
    {
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

        if (other.CompareTag("bullet") && canBeHit && other.GetComponent<BulletMovement>().Cannon == false)
        {
            BulletMovement BulletObject = other.GetComponent<BulletMovement>();

            if (BulletObject.enemy == this.transform)
            {
                if (BulletObject.Type == "Fire") {
                    if (!OnFire)
                    {
                        if (this.gameObject != null)
                        {
                            gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                            if (Boss == false)
                            {
                                gameObject.GetComponent<Renderer>().material.color = Color.red;
                            }
                            OnFire = true;
                            StartCoroutine(Fire(BulletObject, BulletObject.ts));
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
                if (BulletObject.Type == "Nature")
                {

                    if (Armored)
                    {
                        DealDamage(BulletObject.Elementaldamage, BulletObject.ts);
                    }
                    else
                    {
                        DealDamage(BulletObject.Elementaldamage + BulletObject.damage, BulletObject.ts);
                    }

                    if (!OnStun)
                    {
                        if (Stunnable)
                        {
                            OnStun = true;
                            StartCoroutine(Stun());
                        }
                    }
                    if (OnStun)
                    {
                        Destroy(other.gameObject);
                    }
                }

                if (BulletObject.Type == "Normal")
                {
                    if (!Armored)
                    {
                        DealDamage(BulletObject.damage, BulletObject.ts);
                    }
                }
                Destroy(other.gameObject);
            }
 

			if (hp <= 0)
			{
				ps.transform.parent = null;
				ps.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
				ps.Play();
				Destroy(ps.gameObject, 2);

                //other.GetComponent<BulletMovement>().ts.GetComponent<TowerStats>().SetExperience(1);

                GameObject.Find("manager").GetComponent<RayCastFromCamera>().money += cash;


                Destroy(gameObject);
			}
		}

        if (Boss == false)
        {
            if (other.CompareTag("end"))
            {
                GameObject.Find("manager").GetComponent<RayCastFromCamera>().lives--;
                Destroy(this.gameObject);
            }
        }
	}

	public void DealDamage(float damage, TowerStats ts) {
        if (hp-damage > 0)
        {
            ts.SetExperience(damage);
        }
        else
        {
            ts.SetExperience(hp);
        }


        hp -= damage;
        if (hp <= 0)
        {
            //Debug.LogWarning("UMAR£");
            ps.transform.parent = null;
            ps.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
            ps.Play();
            Destroy(ps.gameObject, 2);
            GameObject.Find("manager").GetComponent<RayCastFromCamera>().money += cash;
            Destroy(gameObject);
        }
    }

    public void DealDamageOverTime(float damage, TowerStats ts)
    {
        if (hp - damage > 0)
        {
            ts.SetExperience(damage);
        }
        else
        {
            ts.SetExperience(hp);
        }

        hp -= damage;
        if (hp <= 0)
        {
            Debug.LogWarning("UMAR£");
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
        yield return new WaitForSeconds(2);
        LeanTween.scale(NS, Vector3.zero, 0.3f);
        Destroy(NS, 0.3f);
        GetComponent<NavMeshAgent>().speed = speed;
        OnStun = false;
    }
}
