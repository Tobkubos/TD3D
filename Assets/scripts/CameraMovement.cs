using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject MainCamera;
    private Camera cam;
    [SerializeField] float sens = 0.1f;
    [SerializeField] float zoom = 1;
    private void Start()
    {
        Application.targetFrameRate = 60;
        cam = MainCamera.GetComponent<Camera>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
           MainCamera.transform.position += new Vector3(sens, 0, sens);
        }

        if (Input.GetKey(KeyCode.S))
        {
            MainCamera.transform.position += new Vector3(-sens, 0, -sens);
        }

        if (Input.GetKey(KeyCode.A))
        {
            MainCamera.transform.position += new Vector3(-sens, 0, sens);
        }

        if (Input.GetKey(KeyCode.D))
        {
            MainCamera.transform.position += new Vector3(sens, 0, -sens);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            sens = 0.3f;
            zoom = 2f;
        }
        else
        {
            sens = 0.1f;
            zoom = 1;
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (cam != null)
        {
            if (scrollInput > 0 && cam.orthographicSize < 100)
            {
                cam.orthographicSize -= 1f * zoom;
                if(cam.orthographicSize < 2)
                {
                    cam.orthographicSize = 2;
                }
            }
            else if (scrollInput < 0 && cam.orthographicSize > 1)
            {
                cam.orthographicSize += 1f * zoom;
                if (cam.orthographicSize > 99)
                {
                    cam.orthographicSize = 99;
                }
            }
        }
    }
}
