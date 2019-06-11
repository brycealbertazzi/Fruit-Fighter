using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Transform head;
    [SerializeField] private Transform gunTip;
    [SerializeField] private GameObject bullet;

    private Rigidbody rigidbody;
    private Animator pAnim;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        pAnim = GetComponentInChildren<Animator>();

        rigidbody.freezeRotation = true;
        pAnim.SetBool("isShooting", false);
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
            rigidbody.MovePosition(transform.position + (head.transform.forward * Time.fixedDeltaTime * playerSpeed));
        }
        else if (Input.GetAxisRaw("Vertical") < 0) {
            rigidbody.MovePosition(transform.position + (-head.transform.forward * Time.fixedDeltaTime * playerSpeed));
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            rigidbody.MovePosition(transform.position + (head.transform.right * Time.fixedDeltaTime * playerSpeed));
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            rigidbody.MovePosition(transform.position + (-head.transform.right * Time.fixedDeltaTime * playerSpeed));
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (timeSinceJump > 1.0f)
            {
                pAnim.SetTrigger("jump");
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            pAnim.SetBool("isShooting", true);
            InvokeRepeating("FireMachineGunBullet", 0, fireRate);
        }
        if (Input.GetMouseButtonUp(0)) {
            pAnim.SetBool("isShooting", false);
            CancelInvoke("FireMachineGunBullet");
        }
    }
    
    public float bulletSpeed;
    public float fireRate;
    void FireMachineGunBullet() {
        GameObject firedBullet = Instantiate(bullet, gunTip.transform.position, gunTip.transform.rotation, GameObject.Find("Bullets").transform) as GameObject;
        firedBullet.GetComponent<BoxCollider>().enabled = false;
        firedBullet.GetComponent<Rigidbody>().velocity = firedBullet.transform.forward * bulletSpeed;
    }


    [SerializeField] private float sensitivity;
    void RotateWithMouse() {
        head.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0, Space.Self);
    }

}
