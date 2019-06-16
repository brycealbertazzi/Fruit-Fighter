using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Transform head;
    [SerializeField] private Transform gunTip;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int bulletsInClip;
    [SerializeField] private int maxBulletsInClip;
    [SerializeField] private float reloadTime;

    private Rigidbody rBody;
    private Animator pAnim;
   
    private bool hasMaxClip;
    private bool isReloading;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        pAnim = GetComponentInChildren<Animator>();

        rBody.freezeRotation = true;
        pAnim.SetBool("isShooting", false);
        bulletsInClip = maxBulletsInClip;
        hasMaxClip = true;
        isReloading = false;
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
            rBody.MovePosition(transform.position + (head.transform.forward * Time.fixedDeltaTime * playerSpeed));
        }
        else if (Input.GetAxisRaw("Vertical") < 0) {
            rBody.MovePosition(transform.position + (-head.transform.forward * Time.fixedDeltaTime * playerSpeed));
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            rBody.MovePosition(transform.position + (head.transform.right * Time.fixedDeltaTime * playerSpeed));
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            rBody.MovePosition(transform.position + (-head.transform.right * Time.fixedDeltaTime * playerSpeed));
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!hasMaxClip && !isReloading) //Prevents reload if player has a max clip or is currently reloading
            {
                isReloading = true;
                Invoke("Reload", reloadTime);
                pAnim.SetTrigger("reloadTrigger");
            }
        }

       
        if (Input.GetMouseButtonDown(0) && bulletsInClip > 0 && !isReloading)
        {
            pAnim.SetBool("isShooting", true);
            InvokeRepeating("FireMachineGunBullet", 0, (1 / fireRate));
        }
        if (Input.GetMouseButtonUp(0) || bulletsInClip <= 0 || isReloading)
        {
                pAnim.SetBool("isShooting", false);
                CancelInvoke("FireMachineGunBullet");
        }
        
    }
    
    public float bulletSpeed;
    public float fireRate;
    [SerializeField] private AudioClip fireSound;
    void FireMachineGunBullet() {
        GameObject firedBullet = Instantiate(bullet, gunTip.transform.position, gunTip.transform.rotation, GameObject.Find("Bullets").transform) as GameObject;
        firedBullet.GetComponent<BoxCollider>().enabled = false;
        firedBullet.GetComponent<Rigidbody>().velocity = firedBullet.transform.forward * bulletSpeed;
        GetComponent<AudioSource>().PlayOneShot(fireSound, 0.6f);
        bulletsInClip--;
        hasMaxClip = false;
    }

    void Reload() {
        bulletsInClip = maxBulletsInClip;
        hasMaxClip = true;
        isReloading = false;
    }

    [SerializeField] private float sensitivity;
    void RotateWithMouse() {
        head.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0, Space.Self);
    }

}
