using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
	[SerializeField] string Name;
	[SerializeField] int Level;
	[SerializeField] int Experience;
	[SerializeField] string Type;
	[SerializeField] int Damage;
	[SerializeField] float RotSpeed;

	private List<GameObject> EnemiesInRange = new List<GameObject>();
	public GameObject Bullet;
	public GameObject Turret;

	float Cooldown = 0.1f;
	float nextShoot = 2;

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
		if(EnemiesInRange.Count > 0)
		{
			GameObject target = EnemiesInRange[0];

			Vector3 direction = target.transform.position - Turret.transform.position;
			direction.y = 0;

			if (direction != Vector3.zero)
			{
				Quaternion targetRotation = Quaternion.LookRotation(direction);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
				Debug.DrawRay(transform.position, direction, Color.red);

				if(Time.time > nextShoot)
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
	private void Shoot()
	{
		Instantiate(Bullet, Turret.transform.position, Turret.transform.rotation);
	}
}
