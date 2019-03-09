using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{

    private Camera playerCamera;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        playerCamera.enabled = false;
    }
    [SerializeField] private float playerSpeed;
    private float cos45 = Mathf.Sqrt(2) / 2;
    void Update() {
        RotateWithMouse();
        if (Input.GetKeyDown(KeyCode.Space)) {
            playerCamera.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.LeftShift)) {
            playerCamera.enabled = false;
        }
        //Handle movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, playerSpeed) * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-playerSpeed, 0, 0) * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -playerSpeed) * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(playerSpeed, 0, 0) * Time.deltaTime);
        }
     
    }

    [SerializeField] private float rotationSpeed;
    void RotateWithMouse() {
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0, Space.Self);
    }

}
