using System.Collections;
using System.Collections.Generic;
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
	public GameObject Turret;
	private GameObject target;

	[SerializeField] ParticleSystem ExpPS;

	public float Cooldown = 0.5f;
	float nextShoot = 3;

	private void Start()
	{
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
						Shoot();

						Animation animComp = GetComponent<Animation>();
						animComp.Play("shooting");
						foreach (AnimationState state in animComp)
						{
							state.speed = 3;
						}
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
			Debug.LogError("ExpPS is not assigned in the Inspector");
		}
	}
	private void Shoot()
	{
		GameObject bllt = Instantiate(Bullet, Turret.transform.position, Turret.transform.rotation);
		bllt.GetComponent<BulletMovement>().damage = Damage;
		bllt.GetComponent<BulletMovement>().enemy = target.transform;
		bllt.GetComponent<BulletMovement>().ts = this;
	}
}
