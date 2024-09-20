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
	public bool Support;
	[SerializeField] string Name;
	public string Description;
	int Level;
	float Experience;
	int UpgradePrice;
    private int SellPrice;
	int MaxExp;
	[SerializeField] string Type;
	int Damage;
	int ElementalDamage;
    int DamageOverTime;
	float Range;

    [Header("SUPPORT STATS")]
    public int DamageSupport;
    public int ElementalDamageSupport;
    public int DamageOverTimeSupport;
    public float RangeSupport;
    public float CooldownSupport;

    [Header("STATS FROM SUPPORTS")]
    public int DamageFromSupports;
    public int ElementalDamageFromSupports;
    public int DamageOverTimeFromSupports;
    public float RangeFromSupports;
    public float CooldownFromSupports;

	[Header("TOWER STATS")]
    public int DamageUpgrade;
    public int ElementalUpgrade;
    public int DamageOverTimeUpgrade;
    public float SpeedUpgrade;
    public float RangeUpgrade;
	public bool canUpgrade = false;

	[Header("FINAL STATS")]
	public int FinalDamage;
	public int FinalElementalDamage;
    public int FinalDamageOverTime;
    public float FinalCooldown;
    public float FinalRange;

    public TowerSetupParams towerSetupParams;
	public TowerUpgradeParams[] levels;

    public bool hologram;

	[SerializeField] GameObject[] Towers;
    [SerializeField] List<GameObject> SupportingTowers = new List<GameObject>();
	public GameObject BonusTile;
    public List<GameObject> EnemiesInRange = new List<GameObject>();
    public List<GameObject> TowersInRange = new List<GameObject>();
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
	float Cooldown = 0f;
	float nextShoot = 0f;

	public bool CanShoot = true;
	private int counter = 0;
	GameObject manager;

	public GameObject Block;

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
	public IEnumerator BlockVisualizer(float time)
	{
		float t = UnityEngine.Random.Range(0.1f, 0.2f);
		yield return new WaitForSeconds(t);
		LeanTween.scale(Block, new Vector3(1.1f,2f,1.1f), 0.3f).setEase(LeanTweenType.easeInOutSine);
		yield return new WaitForSeconds(time-0.6f-t);
        LeanTween.scale(Block, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine);
    }
		
	public void Upgrade()
	{
		if (manager.GetComponent<RayCastFromCamera>().money >= UpgradePrice && canUpgrade)
		{
			manager.GetComponent<RayCastFromCamera>().money -= UpgradePrice;
			Experience -= MaxExp;


			MaxExp = levels[Level].MaxExp;
            Level += 1;
			Damage += DamageUpgrade;
			ElementalDamage += ElementalUpgrade;
			DamageOverTime += DamageOverTimeUpgrade;
			Cooldown -= SpeedUpgrade;
			Area.transform.localScale += new Vector3(RangeUpgrade,0, RangeUpgrade);


			FinalDamage = Damage + DamageFromSupports;

			if (Level < 3)
			{
				SellPrice += levels[Level].UpgradePrice / 2;


				DamageUpgrade = levels[Level].DamageUpgrade;
				ElementalUpgrade = levels[Level].ElementalUpgrade;
				DamageOverTimeUpgrade = levels[Level].DamageOverTimeUpgrade;
				SpeedUpgrade = levels[Level].SpeedUpgrade;
				RangeUpgrade = levels[Level].RangeUpgrade;
				UpgradePrice = levels[Level].UpgradePrice;
			}
			else
			{
				MaxExp = 0;
			}

			CheckSupports();
            rot = TWR.transform.rotation;
			Destroy(TWR);
			StopAllCoroutines();
			nextShoot = 0f;
			Setup(Level, rot);
		}
	}
	private void Start()
	{
		if (Support) 
		{ 
			DamageSupport = towerSetupParams.DamageSupport;
			ElementalDamageSupport = towerSetupParams.ElementalDamageSupport;
			DamageOverTimeSupport = towerSetupParams.DamageOverTimeSupport;
			CooldownSupport = towerSetupParams.SpeedSupport;
			RangeSupport = towerSetupParams.RangeSupport;
		}
		else
		{
			Damage = towerSetupParams.Damage;
			ElementalDamage = towerSetupParams.ElementalDamage;
			DamageOverTime = towerSetupParams.DamageOverTime;
			Cooldown = towerSetupParams.Speed;
			MaxExp = towerSetupParams.MaxExp;
		}

		Range = towerSetupParams.Range;
		Area.transform.localScale = new Vector3(towerSetupParams.Range, 0.1f, towerSetupParams.Range);
		SellPrice = towerSetupParams.Price / 2;
		//FinalDamage = Damage + DamageFromSupports;

		if (!Support) { 
			UpgradePrice = levels[Level].UpgradePrice;
			DamageUpgrade = levels[Level].DamageUpgrade;
			ElementalUpgrade = levels[Level].ElementalUpgrade;
			DamageOverTimeUpgrade = levels[Level].DamageOverTimeUpgrade;
			SpeedUpgrade = levels[Level].SpeedUpgrade;
			RangeUpgrade = levels[Level].RangeUpgrade;
        }
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
	public float GetExperience()
	{
		return Experience;
	}
	public void SetExperience(float exp)
	{
		Experience+= exp;
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
        return Range;
    }
    public int GetUpgradePrice()
	{
		return UpgradePrice;
	}
	public void Sell()
	{
		if (Support)
		{
			foreach(GameObject tower in TowersInRange)
			{
				tower.GetComponentInChildren<TowerStats>().SupportingTowers.Remove(TowerObject);
				tower.GetComponentInChildren<TowerStats>().CheckSupports();
            }
		}

		if (!Support) 
		{
			foreach(GameObject support in SupportingTowers)
			{
                support.GetComponentInChildren<TowerStats>().TowersInRange.Remove(TowerObject);

            }		
		}

		manager.GetComponent<RayCastFromCamera>().money += SellPrice;
        manager.GetComponent<RayCastFromCamera>().ts = null;
        manager.GetComponent<RayCastFromCamera>().ActiveTower = -1;
        Destroy(TowerObject);
	}
    public int GetSellIncome()
    {
		return SellPrice;
    }
    public void OnSupportEnterRange(GameObject support)
    {
        SupportingTowers.Add(support);
    }
    public void OnSupportExitRange(GameObject support)
    {
        SupportingTowers.Remove(support);
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

			//WYBIERANIE CELU i STRZELANIE
			#region normal
			if (CanShoot)
			{
				if (Type == "Normal" || Type == "Fire" || Type == "Nature")
				{
					if (EnemiesInRange.Count > 0)
					{
						for (int i = 0; i < EnemiesInRange.Count; i++)
						{
							if (EnemiesInRange[i] == null)
							{
								EnemiesInRange.RemoveAt(i);
								continue;
							}

							float enemyDist = 0;
							if (EnemiesInRange[i] != null && ((Type == "Normal") || (Type == "Fire" && EnemiesInRange[i].GetComponent<EnemyInfo>().OnFire == false) || (Type == "Nature" && EnemiesInRange[i].GetComponent<EnemyInfo>().OnStun == false)))
							{
								enemyDist = EnemiesInRange[i].GetComponent<EnemyInfo>().distanceTravelled;
							}

							if (enemyDist > dist)
							{
								dist = enemyDist;

								if (EnemiesInRange[i] != null)
								{
									target = EnemiesInRange[i];
								}
							}
						}
					}
					else
					{
						target = null;
					}

					if (target != null && ((Type == "Normal") || (Type == "Fire" && target.GetComponent<EnemyInfo>().OnFire == false) || (Type == "Nature" && target.GetComponent<EnemyInfo>().OnStun == false)))
					{
						Vector3 direction = target.transform.position - Turret.transform.position;
						direction.y = 0;

						if (direction != Vector3.zero)
						{
							Quaternion targetRotation = Quaternion.LookRotation(direction);
							transform.rotation = targetRotation;
							Debug.DrawRay(transform.position, direction, Color.red);

							if (Time.time > nextShoot && EnemiesInRange.Count != 0)
							{
								nextShoot = Time.time + FinalCooldown;
								//
								Shoot(target.transform);
								if (Type == "Normal" && ShootAnim != null)
								{
									ShootAnim.Rewind();
									ShootAnim.Play("shooting");
								}

							}
						}
					}
				}
				/*
				if (Type == "Nature")
				{
					if (EnemiesInRange.Count > 0)
					{
						foreach (GameObject enemy in EnemiesInRange)
						{

							float enemyDist = 0;
							float maxHp = 0;
							if (enemy != null && enemy.GetComponent<EnemyInfo>().OnStun == false)
							{
								enemyDist = enemy.GetComponent<EnemyInfo>().distanceTravelled;
							}
							if (enemyDist > dist)
							{
								dist = enemyDist;

								if (enemy != null)
								{
									target = enemy;

									if (target != null)
									{
										if (Time.time > nextShoot && EnemiesInRange.Count != 0)
										{
											nextShoot = Time.time + Cooldown;
											Shoot(target.transform);
										}
									}
								}
							}

						}
					}
					else
					{
						target = null;
					}
				}
				*/

				#endregion
				/*
				if (Type == "Fire")
				{
					if (EnemiesInRange.Count > 0)
					{
						foreach (GameObject enemy in EnemiesInRange)
						{
							//Debug.Log(enemy);
							float enemyDist = 0;
							if (enemy != null && enemy.GetComponent<EnemyInfo>().OnFire == false)
							{
								enemyDist = enemy.GetComponent<EnemyInfo>().distanceTravelled;
							}

							if (enemyDist > dist)
							{
								dist = enemyDist;

								if (enemy != null)
								{
									target = enemy;

									if (target != null)
									{
										Vector3 direction = target.transform.position - Turret.transform.position;
										direction.y = 0;

										if (direction != Vector3.zero)
										{
											Quaternion targetRotation = Quaternion.LookRotation(direction);
											transform.rotation = targetRotation;
											Debug.DrawRay(transform.position, direction, Color.red);
										}
										if (Time.time > nextShoot && EnemiesInRange.Count != 0)
										{
											nextShoot = Time.time + Cooldown;
											Shoot(target.transform);
										}
									}
								}
							}
						}
					}
					else
					{
						target = null;
					}
				}
				*/

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
				if (Type == "Electric")
				{
					if (TopEnemies.Count > 0 && Time.time > nextShoot)
					{
						nextShoot = Time.time + FinalCooldown;

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

							// Zadawanie obra¿eñ
							if (ShootAnim != null)
							{
								ShootAnim.Rewind();
								ShootAnim.Play("shooting");
							}
							for (int i = 0; i < TopEnemies.Count; i++)
							{
								GameObject enemy = TopEnemies[i];
								bool isDead = false;

								if (enemy != null)
								{
									if (ElementalDamage - (i) > 0)
									{
										enemy.GetComponent<EnemyInfo>().DealDamage(Damage + ElementalDamage - i, this);
									}
									else
									{
										enemy.GetComponent<EnemyInfo>().DealDamage(Damage, this);
									}
								}
							}
							StartCoroutine(ClearElectricityAfterDuration(0.1f));
						}
					}
					TopEnemies.Clear();
				}
			}
			//STRZELANIE
			#region rifle
			/*
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

						if (Time.time > nextShoot && EnemiesInRange.Count != 0)
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
		    */
			#endregion
			/*
            if (Type == "Nature")
            {
                if (target != null)
                {
					if (Time.time > nextShoot && EnemiesInRange.Count != 0)
					{
						nextShoot = Time.time + Cooldown;
                        Shoot(target.transform);
                    }            
                }
            }
			*/
			/*
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
					}
                    if (Time.time > nextShoot && EnemiesInRange.Count != 0)
                    {
                        nextShoot = Time.time + Cooldown;
                        Shoot(target.transform);
                    }
                }			
			}
			*/











            if (ExpPS != null)
			{
				//Debug.Log("Checking Experience: " + Experience + "/" + MaxExp);
				if (Experience >= MaxExp && Level<3)
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

	public void CheckSupports()
	{
		DamageFromSupports = 0;
		ElementalDamageFromSupports = 0;
		DamageOverTimeFromSupports = 0;
		RangeFromSupports = 0;
		CooldownFromSupports = 0;

		if(Damage !=0) {
			foreach(GameObject support in SupportingTowers)
			{
				DamageFromSupports += support.GetComponentInChildren<TowerStats>().DamageSupport;
			}
		FinalDamage = Damage + DamageFromSupports;
		}

        if (ElementalDamage != 0)
        {
            foreach (GameObject support in SupportingTowers)
            {
				ElementalDamageFromSupports += support.GetComponentInChildren<TowerStats>().ElementalDamageSupport;
            }
            FinalElementalDamage = ElementalDamage + ElementalDamageFromSupports;
        }

        if (DamageOverTime != 0)
        {
            foreach (GameObject support in SupportingTowers)
            {
                DamageOverTimeFromSupports += support.GetComponentInChildren<TowerStats>().DamageOverTimeSupport;
            }
            FinalDamageOverTime = DamageOverTime + DamageOverTimeFromSupports;
        }

        foreach (GameObject support in SupportingTowers)
        {
            RangeFromSupports += support.GetComponentInChildren<TowerStats>().RangeSupport;
        }
        FinalRange = Range + RangeFromSupports;
		Area.transform.localScale = new Vector3(FinalRange, Area.transform.localScale.y, FinalRange);

        foreach (GameObject support in SupportingTowers)
        {
            CooldownFromSupports += support.GetComponentInChildren<TowerStats>().CooldownSupport;
        }
        FinalCooldown = Cooldown - CooldownFromSupports;


		if(BonusTile != null)
		{
			//Debug.Log("BONUS");
			FinalDamage += BonusTile.GetComponent<BonusTile>().Damage;
			FinalElementalDamage += BonusTile.GetComponent<BonusTile>().ElementalDamage;
			FinalDamageOverTime += BonusTile.GetComponent<BonusTile>().DamageOverTime;
			FinalRange += BonusTile.GetComponent<BonusTile>().Range;
            Area.transform.localScale = new Vector3(FinalRange, Area.transform.localScale.y, FinalRange);
            FinalCooldown -= BonusTile.GetComponent<BonusTile>().Cooldown;
        }

		if (FinalCooldown < 0.1f) FinalCooldown = 0.1f;
    }
	private void Shoot(Transform target)
	{
		GameObject bllt = Instantiate(Bullet, Turret.transform.position, Turret.transform.rotation);
		bllt.GetComponent<BulletMovement>().damage = FinalDamage;
        bllt.GetComponent<BulletMovement>().Elementaldamage = FinalElementalDamage;
        bllt.GetComponent<BulletMovement>().DamageOverTime = FinalDamageOverTime;
        bllt.GetComponent<BulletMovement>().enemy = target;
		bllt.GetComponent<BulletMovement>().ts = this;
    }

    private IEnumerator ClearElectricityAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        lineRenderer.positionCount = 0; //czyszczenie
    }
}
