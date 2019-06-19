using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] private Transform head;
    [SerializeField] private Transform gunTip;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int bulletsInClip;
    [SerializeField] private int maxBulletsInClip;
    [SerializeField] private float reloadTime;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    private Rigidbody rBody;
    private Animator pAnim;
    [SerializeField] private Slider playerSlider;
    [SerializeField] private Text clipDisplay;

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

        maxHealth = 100;
    }

    [SerializeField] private float playerSpeed;
    float timeSinceJump = 10; //An arbitrary value which allows jumping
    void FixedUpdate() {
        if (GameManager.instance.state == GameManager.GameStates.GameOn)
        {
            timeSinceJump += Time.fixedDeltaTime;
            RotateWithMouse();
            //Handle movement w/ rigidbodies
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                rBody.MovePosition(transform.position + (head.transform.forward * Time.fixedDeltaTime * playerSpeed));
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
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
    }

    private void Update()
    {
        if (GameManager.instance.state == GameManager.GameStates.GameOn)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!hasMaxClip && !isReloading) //Prevents reload if player has a max clip or is currently reloading
                {
                    isReloading = true;
                    Invoke("Reload", reloadTime);
                    pAnim.SetTrigger("reloadTrigger");
                }
            }


            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && bulletsInClip > 0 && !isReloading)
            {
                pAnim.SetBool("isShooting", true);
                InvokeRepeating("FireMachineGunBullet", 0, (1 / fireRate));
            }
            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space) || bulletsInClip <= 0 || isReloading || GameManager.instance.state != GameManager.GameStates.GameOn)
            {
                pAnim.SetBool("isShooting", false);
                CancelInvoke("FireMachineGunBullet");
            }
        }
    }

    public float sensitivity;
    void RotateWithMouse()
    {
        head.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0, Space.Self);
    }

    public float bulletSpeed;
    public float fireRate;
    [SerializeField] private AudioClip fireSound;
    void FireMachineGunBullet() {
        if (GameManager.instance.state == GameManager.GameStates.GameOn) {
            GameObject firedBullet = Instantiate(bullet, gunTip.transform.position, gunTip.transform.rotation, GameObject.Find("Bullets").transform) as GameObject;
            firedBullet.GetComponent<Rigidbody>().velocity = firedBullet.transform.forward * bulletSpeed;
            GetComponent<AudioSource>().PlayOneShot(fireSound, 0.6f);
            bulletsInClip--;
            hasMaxClip = false;
            UpdateClipDisplay();
        }
    }

    void Reload() {
        bulletsInClip = maxBulletsInClip;
        hasMaxClip = true;
        isReloading = false;
        UpdateClipDisplay();
    }

    void UpdateClipDisplay() {
        clipDisplay.text = bulletsInClip.ToString();
    }

    void UpdateHealthDisplay() {
        playerSlider.value = (currentHealth / maxHealth);
    }

    [SerializeField] private AudioClip playerHit;
    public void TakeHit(int damage) {
        GetComponent<AudioSource>().PlayOneShot(playerHit, 0.8f);
        currentHealth -= damage;
        UpdateHealthDisplay();
        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        GameManager.instance.GameOver();
    }

    public void ResetPlayer() {
        currentHealth = maxHealth;
        bulletsInClip = maxBulletsInClip;
        UpdateHealthDisplay();
        UpdateClipDisplay();
    }

}
