using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int hp;
	public int speed;
	public int defence;

	public ParticleSystem ps;

	private void Update()
	{
		if(hp == 0)
		{
			ps.Play();
			Destroy(gameObject);
		}
	}
}
