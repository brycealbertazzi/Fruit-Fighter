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

        rigidbody.freezeRotation = true;
    }

    [SerializeField] private float playerSpeed;
    [SerializeField] private float junpForce;
    float timeSinceJump = 10; //An arbitrary value which allows jumping
    void FixedUpdate() {
        timeSinceJump += Time.fixedDeltaTime; 
        RotateWithMouse();
        //Handle movement w/ rigidbodies
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            rigidbody.MovePosition(transform.position + (transform.forward * Time.fixedDeltaTime * playerSpeed));
        }
        else if (Input.GetAxisRaw("Vertical") < 0) {
            rigidbody.MovePosition(transform.position + (-transform.forward * Time.fixedDeltaTime * playerSpeed));
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            rigidbody.MovePosition(transform.position + (transform.right * Time.fixedDeltaTime * playerSpeed));
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            rigidbody.MovePosition(transform.position + (-transform.right * Time.fixedDeltaTime * playerSpeed));
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (timeSinceJump > 0.7f)
            {
                rigidbody.AddForce(new Vector3(0, junpForce, 0), ForceMode.Impulse);
                timeSinceJump = 0;
            }
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0, 10), transform.position.z);
    }

    [SerializeField] private float rotationSpeed;
    void RotateWithMouse() {
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0, Space.Self);
    }

}
