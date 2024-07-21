using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
	[SerializeField] string Name;
	[SerializeField] int Level;
	[SerializeField] int Experience;
	[SerializeField] int MaxExp;
	[SerializeField] string Type;
	[SerializeField] int Damage;
	[SerializeField] float RotSpeed;

	private List<GameObject> EnemiesInRange = new List<GameObject>();
	public GameObject Bullet;
	private GameObject Turret;
	private GameObject target;
	private GameObject tower;
	private Animation ShootAnim;

	public int level = 2;

	[SerializeField] ParticleSystem ExpPS;

	public float Cooldown = 1f;
	float nextShoot = 3;

	private int counter = 0;

	private void Start()
	{

		//zmienic na tagi
		if(level == 2) { 
			Transform towerTransform = this.transform.Find("tower");
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


        nextShoot = Time.time + 3;
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

	public void OnEnemyEnterRange(GameObject enemy)
	{
		EnemiesInRange.Add(enemy);
	}

	public void OnEnemyExitRange(GameObject enemy)
	{
		EnemiesInRange.Remove(enemy);
	}


	private IEnumerator ShootingLEVEL2(Transform target)
	{
		Shoot(target);
		counter--;
		/*
        Animation animComp = tower.GetComponent<Animation>();
        animComp.Rewind("shooting lvl 1");
        animComp.Play("shooting lvl 1");
        foreach (AnimationState state in animComp)
        {
            state.speed = 1;
        }
		*/

		ShootAnim.Rewind("shooting lvl 1");
		ShootAnim.Play("shooting lvl 1");
		foreach (AnimationState state in ShootAnim)
		{
			state.speed = 1;
		}


        yield return new WaitForSeconds(Cooldown);
		Shoot(target);
		counter--;

    }
	private void Update()
	{

		if (target == null)
		{
			EnemiesInRange.Remove(target);
		}

		if (EnemiesInRange.Count > 0)
		{
			target = EnemiesInRange[0];
			if (target != null)
			{
				Vector3 direction = target.transform.position - Turret.transform.position;
				direction.y = 0;
				
				if (direction != Vector3.zero)
				{
					Quaternion targetRotation = Quaternion.LookRotation(direction);
					transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
					Debug.DrawRay(transform.position, direction, Color.red);
					
					if (Time.time > nextShoot)
					{
						nextShoot = Time.time + Cooldown;

						//
						if (level == 1)
						{
                            Shoot(target.transform);
                            Animation animComp = tower.GetComponent<Animation>();
							animComp.Play("shooting lvl 1");
							foreach (AnimationState state in animComp)
							{
								state.speed = 1;
							}
						}

                        if (level == 2 && counter == 0)
                        {
                            counter = 2;
                            StartCoroutine(ShootingLEVEL2(target.transform));
                        }

                        //
                    }
				}
			}
		}

		if (ExpPS != null)
		{
			Debug.Log("Checking Experience: " + Experience + "/" + MaxExp);
			if (Experience >= MaxExp)
			{
				if (!ExpPS.isPlaying)
				{
					Debug.Log("Playing Particle System");
					ExpPS.Play();
				}
			}
			else
			{
				if (ExpPS.isPlaying)
				{
					Debug.Log("Stopping Particle System");
					ExpPS.Stop();
				}
			}
		}
		else
		{
			Debug.LogWarning("ExpPS is not assigned in the Inspector");
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
