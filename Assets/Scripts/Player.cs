using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Camera playerCamera;
    private Rigidbody rigidbody;
    
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        rigidbody = GetComponent<Rigidbody>();
        playerCamera.enabled = true;
    }

    [SerializeField] private float playerSpeed;
    void FixedUpdate() {
        RotateWithMouse();
    
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
