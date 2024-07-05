using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastFromCamera : MonoBehaviour
{
	public Camera camera;

	void Start()
	{
		if (camera == null)
		{
			camera = Camera.main;
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.CompareTag("chunk"))
				{
					Debug.Log("Obiekt trafiony: " + hit.collider.name + " ma tag 'chunk'.");
					hit.collider.gameObject.GetComponent<ChunkReveal>().Reveal();
				}
			}
		}
	}
}
