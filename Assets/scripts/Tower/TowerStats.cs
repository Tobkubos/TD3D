using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
	[SerializeField] string Name;
	[SerializeField] int Level;
	[SerializeField] int Experience;
    [SerializeField] int UpgradePrice;
    [SerializeField] int MaxExp;
	[SerializeField] string Type;

	[SerializeField] int Damage;
    [SerializeField] int ElementalDamage;
    [SerializeField] int DamageOverTime;
    [SerializeField] int Range;
    [SerializeField] int DPS;

	public int DamageUpgrade;
    public int ElementalUpgrade;
    public int DamageOverTimeUpgrade;
    public int SpeedUpgrade;
    public int RangeUpgrade;
    public int DPSUpgrade;


    public bool hologram;

	[SerializeField] GameObject[] Towers;

	private List<GameObject> EnemiesInRange = new List<GameObject>();

	public GameObject Bullet;
	private GameObject Turret;
	private GameObject target;
	private GameObject tower;
	private Animation ShootAnim;
	private GameObject TWR;
	public GameObject Area;
	private Quaternion rot;

	[SerializeField] ParticleSystem ExpPS;

    float Cooldown = 1f;
	float nextShoot = 0f;

	private int counter = 0;
	GameObject manager;

	void Setup(int level, Quaternion rot)
	{
		manager = GameObject.Find("manager");
		TWR = Instantiate(Towers[level], this.transform.position, rot, this.gameObject.transform);
		Transform towerTransform = TWR.transform;

		if (towerTransform != null)
		{
			tower = towerTransform.gameObject;
			Debug.Log("ZNALAZLEM TOWER");
			ShootAnim = tower.GetComponent<Animation>();

			Transform turretTransform = tower.transform.Find("turret");
			if (turretTransform != null)
			{
				Debug.Log("ZNALAZLEM TURRET");
				Turret = turretTransform.gameObject;
			}
		}
	}
	public void Upgrade()
	{
		if (manager.GetComponent<RayCastFromCamera>().money >= UpgradePrice)
		{
			manager.GetComponent<RayCastFromCamera>().money -= UpgradePrice;

				

            Level += 1;
            if (Level == 1) 
			{
				Cooldown = 0.5f;
				Damage += DamageUpgrade;
			}
            if (Level == 2)
            {
                Cooldown = 0.25f;
				Damage += 2;
            }
            if (Level == 3)
            {
                Cooldown = 0.12f;
				Damage += 3;
            }
            Area.transform.localScale += new Vector3(1,0,1);
            rot = TWR.transform.rotation;
			Destroy(TWR);
			StopAllCoroutines();
			nextShoot = 0f;
			Setup(Level, rot);
		}
	}
	private void Start()
	{
		Setup(Level, Quaternion.identity);
        DamageUpgrade = 2;
        //nextShoot = Time.time + 3;
    }
	public string GetName()
	{
		return Name;
	}
	public int GetLevel()
	{
		return Level;
	}
	public int GetExperience()
	{
		return Experience;
	}
	public void SetExperience()
	{
		Experience++;
	}
	public int GetMaxExp()
	{
		return MaxExp;
	}
	public string GetType()
	{
		return Type;
	}
	public int GetDamage()
	{
		return Damage;
	}
    public int GetElementalDamage()
    {
        return ElementalDamage;
    }
    public int GetDamageOverTime()
    {
        return DamageOverTime;
    }
    public float GetAttackSpeed()
    {
        return Cooldown;
    }
    public float GetRange()
    {
        return Area.transform.localScale.x;
    }
    public int GetDPS()
    {
        return DPS;
    }

    public int GetUpgradePrice()
	{
		return UpgradePrice;
	}

    public void OnEnemyEnterRange(GameObject enemy)
	{
		EnemiesInRange.Add(enemy);
	}

	public void OnEnemyExitRange(GameObject enemy)
	{
		EnemiesInRange.Remove(enemy);
	}
	
	private void FixedUpdate()
	{
		//Debug.Log("LEVEL" + Level + "COUNTER "  + counter  + "COOLDOWN" + Cooldown);
		if (!hologram)
		{
			if (target == null)
			{
				EnemiesInRange.Remove(target);


			}

			float dist = 0;
			if (EnemiesInRange.Count > 0)
			{
				foreach(GameObject enemy in EnemiesInRange)
				{
					float enemyDist = 0;
					if (enemy != null)
					{
						enemyDist = enemy.GetComponent<EnemyInfo>().distanceTravelled;
					}

                    if (enemyDist > dist)
                    {
						dist = enemyDist;
                        target = enemy;
                    }
                }


				//target = EnemiesInRange[0];
				if (target != null)
				{
					Vector3 direction = target.transform.position - Turret.transform.position;
					direction.y = 0;

					if (direction != Vector3.zero)
					{
						Quaternion targetRotation = Quaternion.LookRotation(direction);
						//transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
						transform.rotation = targetRotation;
						Debug.DrawRay(transform.position, direction, Color.red);

						if (Time.time > nextShoot)
						{
							nextShoot = Time.time + Cooldown;

							//
							Shoot(target.transform);
							//Animation animComp = tower.GetComponent<Animation>();
							ShootAnim.Rewind();
                            ShootAnim.Play("shooting");
                         
                            //
                        }
					}
				}
			}

			if (ExpPS != null)
			{
				//Debug.Log("Checking Experience: " + Experience + "/" + MaxExp);
				if (Experience >= MaxExp)
				{
					if (!ExpPS.isPlaying)
					{
						//Debug.Log("Playing Particle System");
						ExpPS.Play();
					}
				}
				else
				{
					if (ExpPS.isPlaying)
					{
						//Debug.Log("Stopping Particle System");
						ExpPS.Stop();
					}
				}
			}
			else
			{
				//Debug.LogWarning("ExpPS is not assigned in the Inspector");
			}
		}
	}
	private void Shoot(Transform target)
	{
		GameObject bllt = Instantiate(Bullet, Turret.transform.position, Turret.transform.rotation);
		bllt.GetComponent<BulletMovement>().damage = Damage;
		bllt.GetComponent<BulletMovement>().enemy = target;
		bllt.GetComponent<BulletMovement>().ts = this;
	}
}
