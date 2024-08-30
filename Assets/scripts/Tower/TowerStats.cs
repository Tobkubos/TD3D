using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class TowerStats : MonoBehaviour
{
	[SerializeField] string Name;
	public string Description;
	int Level;
	int Experience;
	int UpgradePrice;
    private int SellPrice;
	int MaxExp;
	[SerializeField] string Type;
	int Damage;
	int ElementalDamage;
	float DamageOverTime;
	float Range;

	public int DamageUpgrade;
    public int ElementalUpgrade;
    public int DamageOverTimeUpgrade;
    public float SpeedUpgrade;
    public float RangeUpgrade;
	public bool canUpgrade = false;

	public TowerSetupParams towerSetupParams;
	public TowerUpgradeParams[] levels;

    public bool hologram;

	[SerializeField] GameObject[] Towers;

	private List<GameObject> EnemiesInRange = new List<GameObject>();
    private List<GameObject> TopEnemies = new List<GameObject>();

    public GameObject Bullet;
	private GameObject Turret;
	private GameObject target;
	private GameObject tower;
	private Animation ShootAnim;
	private GameObject TWR;
	public GameObject Area;
	private Quaternion rot;
	public GameObject TowerObject;
	[SerializeField] ParticleSystem ExpPS;
	public LineRenderer lineRenderer;
	float Cooldown = 1f;
	float nextShoot = 0f;

	private int counter = 0;
	GameObject manager;

	public GameObject TestProj;
	Vector3[] positions = null;

    void Setup(int level, Quaternion rot)
	{
		manager = GameObject.Find("manager");
		TWR = Instantiate(Towers[level], this.transform.position, rot, this.gameObject.transform);
		Transform towerTransform = TWR.transform;

		if (towerTransform != null)
		{
			tower = towerTransform.gameObject;
			//Debug.Log("ZNALAZLEM TOWER");
			ShootAnim = tower.GetComponent<Animation>();

			Transform turretTransform = tower.transform.Find("turret");
			if (turretTransform != null)
			{
				//Debug.Log("ZNALAZLEM TURRET");
				Turret = turretTransform.gameObject;
			}
		}
	}
	public void Upgrade()
	{
		if (manager.GetComponent<RayCastFromCamera>().money >= UpgradePrice && canUpgrade)
		{
			manager.GetComponent<RayCastFromCamera>().money -= UpgradePrice;
			Experience = 0;


            Level += 1;
			Damage += DamageUpgrade;
			ElementalDamage += ElementalUpgrade;
			DamageOverTime += DamageOverTimeUpgrade;
			Cooldown -= SpeedUpgrade;
			Area.transform.localScale += new Vector3(RangeUpgrade,0, RangeUpgrade);
			MaxExp = levels[Level].MaxExp;
			SellPrice += levels[Level].UpgradePrice / 2;


            DamageUpgrade = levels[Level].DamageUpgrade;
			ElementalUpgrade = levels[Level].ElementalUpgrade;
			DamageOverTime = levels[Level].DamageOverTimeUpgrade;
            SpeedUpgrade = levels[Level].SpeedUpgrade;
            RangeUpgrade = levels[Level].RangeUpgrade;
			MaxExp = levels[Level].MaxExp;
			UpgradePrice = levels[Level].UpgradePrice;
           
			

            rot = TWR.transform.rotation;
			Destroy(TWR);
			StopAllCoroutines();
			nextShoot = 0f;
			Setup(Level, rot);
		}
	}
	private void Start()
	{
		Damage = towerSetupParams.Damage;
		ElementalDamage = towerSetupParams.ElementalDamage;
		DamageOverTime = towerSetupParams.DamageOverTime;
		Cooldown = towerSetupParams.Speed;
		Range = towerSetupParams.Range;
		MaxExp = towerSetupParams.MaxExp;
		Area.transform.localScale =  new Vector3(towerSetupParams.Range, 0.1f, towerSetupParams.Range);

        SellPrice = towerSetupParams.Price / 2;

		UpgradePrice = levels[Level].UpgradePrice;
		DamageUpgrade = levels[Level].DamageUpgrade;
		ElementalUpgrade = levels[Level].ElementalUpgrade;
		DamageOverTimeUpgrade = levels[Level].DamageOverTimeUpgrade;
		SpeedUpgrade = levels[Level].SpeedUpgrade;
		RangeUpgrade = levels[Level].RangeUpgrade;

        Setup(Level, Quaternion.identity);
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
		if (Experience < MaxExp)
		{
			Experience++;
		};
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
    public float GetDamageOverTime()
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
    public int GetUpgradePrice()
	{
		return UpgradePrice;
	}
	public void Sell()
	{
		manager.GetComponent<RayCastFromCamera>().money += SellPrice;
        manager.GetComponent<RayCastFromCamera>().ts = null;
        manager.GetComponent<RayCastFromCamera>().ActiveTower = -1;
        //manager.GetComponent<RayCastFromCamera>().TowerStatsCanva.SetActive(false);
        Destroy(TowerObject);
	}
    public int GetSellIncome()
    {
		return SellPrice;
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
		if (!hologram)
		{
			if (target == null)
			{
				EnemiesInRange.Remove(target);
			}

			float dist = 0;

			//WYBIERANIE CELU
			#region normal
			if (Type == "Normal")
			{
				if (EnemiesInRange.Count > 0)
				{
					foreach (GameObject enemy in EnemiesInRange)
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
					target = EnemiesInRange[0];
				}
			}

            if (Type == "Nature")
            {
                if (EnemiesInRange.Count > 0)
                {
                    foreach (GameObject enemy in EnemiesInRange)
                    {
                        if (enemy != null && enemy.GetComponent<EnemyInfo>().OnStun == false)
                        {
                            target = enemy;
                            break;
                        }
                        else
                        {
                            target = null;
                        }
                    }
                }
            }

            #endregion
            if (Type == "Fire")
            {
                if (EnemiesInRange.Count > 0)
                {
                    foreach (GameObject enemy in EnemiesInRange)
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
					foreach (GameObject enemy in EnemiesInRange) {
						if (enemy != null && enemy.GetComponent<EnemyInfo>().OnFire == false)
						{
							target = enemy;
							break;
						}
						else {
							target = null;
                        }
					}
                }
            }

            #region tesla
            if (Type == "Electric")
			{
				if (EnemiesInRange.Count > 1)
				{
					EnemiesInRange.Sort((enemy1, enemy2) =>
					{
						if (enemy1 == null && enemy2 == null) return 0;
						if (enemy1 == null) return 1;
						if (enemy2 == null) return -1;

						return enemy2.GetComponent<EnemyInfo>().distanceTravelled.CompareTo(enemy1.GetComponent<EnemyInfo>().distanceTravelled);
					});
					}
                TopEnemies = EnemiesInRange.Take(3).ToList();
            }
            #endregion

            //STRZELANIE
            #region rifle
            if (Type == "Normal")
			{
				if (target != null)
				{
					Vector3 direction = target.transform.position - Turret.transform.position;
					direction.y = 0;

					if (direction != Vector3.zero)
					{
						Quaternion targetRotation = Quaternion.LookRotation(direction);
						transform.rotation = targetRotation;
						Debug.DrawRay(transform.position, direction, Color.red);

						if (Time.time > nextShoot)
						{
							nextShoot = Time.time + Cooldown;
							//
							Shoot(target.transform);
							if (ShootAnim != null)
							{
								ShootAnim.Rewind();
								ShootAnim.Play("shooting");
							}
								
						}
					}
				}			
			}
            #endregion

            if (Type == "Nature")
            {
                if (target != null)
                {
					if (Time.time > nextShoot)
					{
						nextShoot = Time.time + Cooldown;
                        Shoot(target.transform);
                    }            
                }
            }

            if (Type == "Electric")
            {
                if (TopEnemies.Count > 0 && Time.time > nextShoot)
                {
                    nextShoot = Time.time + Cooldown;

                    // Zmienna pomocnicza
                    List<Vector3> positions = new List<Vector3> { tower != null ? tower.transform.position : Vector3.zero };

                    // Zbieranie pozycji przeciwników
                    foreach (var enemy in TopEnemies)
                    {
                        if (enemy != null && enemy.activeInHierarchy)
                        {
                            positions.Add(enemy.transform.position);
                        }
                    }

                    for (int i = 0; i < positions.Count; i++)
                    {
                        Vector3 position = positions[i];
                        position.y = 0.3f;
                        positions[i] = position;
                    }


                    if (positions.Count > 1)
                    {
                        lineRenderer.positionCount = positions.Count;
                        lineRenderer.startWidth = 0.1f;
                        lineRenderer.endWidth = 0.1f;
                        lineRenderer.SetPositions(positions.ToArray());
                        lineRenderer.widthMultiplier = 1.0f;

                        // Zadawanie obrażeń
                        if (ShootAnim != null)
                        {
                            ShootAnim.Rewind();
                            ShootAnim.Play("shooting");
                        }
                        for (int i = 0; i<TopEnemies.Count; i++)
                        {
							GameObject enemy = TopEnemies[i];
							bool isDead = false;

                            if (enemy != null)
							{
								if (ElementalDamage - (i) > 0)
								{
									isDead = enemy.GetComponent<EnemyInfo>().DealDamage(Damage + ElementalDamage - i);
								}
								else
								{
                                    isDead = enemy.GetComponent<EnemyInfo>().DealDamage(Damage);

                                }
							}
							if (isDead) 
							{
								SetExperience();
							}
                        }
                        StartCoroutine(ClearElectricityAfterDuration(0.1f));
                    }
                }
                TopEnemies.Clear();
            }

			if (Type == "Fire")
			{
				if (target != null)
				{
					Vector3 direction = target.transform.position - Turret.transform.position;
					direction.y = 0;

					if (direction != Vector3.zero)
					{
						Quaternion targetRotation = Quaternion.LookRotation(direction);
						transform.rotation = targetRotation;
						Debug.DrawRay(transform.position, direction, Color.red);

						if (Time.time > nextShoot)
						{
							nextShoot = Time.time + Cooldown;
							Shoot(target.transform);						
						}
					}
				}			
			}













            if (ExpPS != null)
			{
				//Debug.Log("Checking Experience: " + Experience + "/" + MaxExp);
				if (Experience >= MaxExp)
				{
					canUpgrade = true;
					if (!ExpPS.isPlaying)
					{
						//Debug.Log("Playing Particle System");
						ExpPS.Play();
					}
				}
				else
				{
					canUpgrade = false;
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

    private IEnumerator ClearElectricityAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        lineRenderer.positionCount = 0; //czyszczenie
    }
}
