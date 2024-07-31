using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleMovement : MonoBehaviour
{
    public NavMeshAgent agent;
	public GameObject end;

	/*
	private void Start()
	{
		end = GameObject.FindGameObjectWithTag("end");
	}
	*/

	void Update()
	{
		if (agent != null && end != null)
		{
			agent.SetDestination(end.transform.position);
		}
	}

}
