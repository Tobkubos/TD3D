using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerStats : MonoBehaviour
{
	[SerializeField] string Name;
	[SerializeField] int Level;
    [SerializeField] int Experience;
    [SerializeField] int UpgradePrice;
    private int SellPrice;
    [SerializeField] int MaxExp;
	[SerializeField] string Type;

	[SerializeField] int Damage;
    [SerializeField] int ElementalDamage;
    [SerializeField] int DamageOverTime;
    [SerializeField] int Range;

	public int DamageUpgrade;
    public int ElementalUpgrade;
    public int DamageOverTimeUpgrade;
    public float SpeedUpgrade;
    public float RangeUpgrade;
	public bool canUpgrade = false;

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
		if (manager.GetComponent<RayCastFromCamera>().money >= UpgradePrice && canUpgrade)
		{
			manager.GetComponent<RayCastFromCamera>().money -= UpgradePrice;
			Experience = 0;
				

            Level += 1;
            if (Level == 1) 
			{
				Cooldown -= SpeedUpgrade;
				Damage += DamageUpgrade;
				Area.transform.localScale += new Vector3(RangeUpgrade,0,RangeUpgrade);

				SellPrice += UpgradePrice / 2;
                DamageUpgrade = 2;
                RangeUpgrade = 0.6f;
                SpeedUpgrade = 0.25f;
				MaxExp = 2;
				UpgradePrice = 250;
            }
            if (Level == 2)
            {
                Cooldown -= SpeedUpgrade;
                Damage += DamageUpgrade;
                Area.transform.localScale += new Vector3(RangeUpgrade, 0, RangeUpgrade);

                SellPrice += UpgradePrice / 2;
                DamageUpgrade = 4;
                RangeUpgrade = 1.2f;
                SpeedUpgrade = 0.125f;
                MaxExp = 3;
                UpgradePrice = 500;
            }
            if (Level == 3)
            {
                SellPrice += UpgradePrice / 2;
                Cooldown -= SpeedUpgrade;
                Damage += DamageUpgrade;
                Area.transform.localScale += new Vector3(RangeUpgrade, 0, RangeUpgrade);
				MaxExp = 0;
                UpgradePrice = 900;
            }
            rot = TWR.transform.rotation;
			Destroy(TWR);
			StopAllCoroutines();
			nextShoot = 0f;
			Setup(Level, rot);
		}
	}
	private void Start()
	{
        SellPrice = UpgradePrice / 2;
        if (Type == "Normal") 
		{
			DamageUpgrade = 1;
			RangeUpgrade = 0.4f;
			SpeedUpgrade = 0.5f;
		}

        if (Type == "Electric")
        {
            DamageUpgrade = 1;
            ElementalUpgrade = 2;
            RangeUpgrade = 0.5f;
            SpeedUpgrade = 0.15f;
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
    public int GetUpgradePrice()
	{
		return UpgradePrice;
	}
	public void Sell()
	{
		manager.GetComponent<RayCastFromCamera>().money += SellPrice;
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
            #endregion

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

            /*
            if (Type == "Electric")
            {
                if (TopEnemies.Count > 0 && Time.time > nextShoot)
                {
                    nextShoot = Time.time + Cooldown;

                    GameObject testProj = null;

                    for (int i = 0; i < 3; i++)
                    {
                        if (i < TopEnemies.Count && TopEnemies[i] != null)
                        {
							testProj = Instantiate(TestProj, TopEnemies[i].transform.position, Quaternion.identity);
							Destroy(testProj, 1);
						}
					}
                }
                TopEnemies.Clear();
            }
			*/

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

                        // Zadawanie obra¿eñ
                        for(int i = 0; i<TopEnemies.Count; i++)
                        {
							GameObject enemy = TopEnemies[i];
							bool isDead = false;

                            if (enemy != null)
							{
								if (ElementalDamage - (i * 2) > 0)
								{
									isDead = enemy.GetComponent<EnemyInfo>().DealDamage(Damage + ElementalDamage - (i * 2));
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
