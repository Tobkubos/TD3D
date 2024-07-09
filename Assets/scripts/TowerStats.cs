using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
	[SerializeField] string Name;
	[SerializeField] int Level;
	[SerializeField] int Experience;
	[SerializeField] string Type;
	[SerializeField] int Damage;

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
}
