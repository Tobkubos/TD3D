using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleMovement : MonoBehaviour
{
    public NavMeshAgent agent;
	public GameObject finish;
	void Update()
    {
        agent.SetDestination(finish.transform.position);
    }
}
