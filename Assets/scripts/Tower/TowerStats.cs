using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using Unity.VisualScripting;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
	[SerializeField] string Name;
	[SerializeField] int Level;
	[SerializeField] int Experience;
	[SerializeField] string Type;
	[SerializeField] int Damage;

	private List<GameObject> EnemiesInRange = new List<GameObject>();

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
		Debug.Log("Enemy entered range: " + enemy.name);
		EnemiesInRange.Add(enemy);
	}

	public void OnEnemyExitRange(GameObject enemy)
	{
		Debug.Log("Enemy exited range: " + enemy.name);
		EnemiesInRange.Remove(enemy);
	}

	private void Update()
	{
		if(EnemiesInRange.Count > 0)
		{
			GameObject target = EnemiesInRange[0];
			Debug.Log(target.name);
		}
	}
}
