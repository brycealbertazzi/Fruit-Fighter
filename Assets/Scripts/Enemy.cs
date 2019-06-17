using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    private GameObject target;
    private NavMeshAgent navmesh;
    private Animator anim;
    private SkinnedMeshRenderer enemyMesh;
    private bool isAttacking = false;
    private bool isDead = false;

    [SerializeField] private int health = 100;

    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = FindObjectOfType<Player>().gameObject;
        enemyMesh = transform.Find("EnemyDroid").transform.Find("Starwars_Droid").GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        if (GameManager.instance.state == GameManager.GameStates.GameOn)
        {
            navmesh.SetDestination(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.Play("Hit");
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.Play("Walk");
            }

            if (EnemyInRadius() && !isAttacking && !isDead)
            {
                isAttacking = true;
                StartCoroutine("DamagePlayerCoroutine");
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Bullet") {
            TakeHit(Bullet.damage);
        }
    }

    void TakeHit(int damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    [SerializeField] private AudioClip deathSound;
    void Die() {
        GameManager.instance.totalEnemiesOnMap--;
        isDead = true;
        enemyMesh.enabled = false;
        StartCoroutine("DeathEffect");
        GetComponent<BoxCollider>().enabled = false;
        GameManager.instance.UpdateScore();
    }

    [SerializeField] private ParticleSystem deathEffect;
    IEnumerator DeathEffect() {
        ParticleSystem dieParticleSystem = Instantiate(deathEffect, transform.position + new Vector3(0, 0.65f, 0), Quaternion.identity) as ParticleSystem;
        dieParticleSystem.Play();
        FindObjectOfType<Player>().gameObject.GetComponent<AudioSource>().PlayOneShot(deathSound, 0.5f);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        Destroy(dieParticleSystem.gameObject);
    }

    [SerializeField] private float hitsPerSecond;
    IEnumerator DamagePlayerCoroutine() {
        if (!EnemyInRadius() || isDead)
        {
            StopCoroutine("DamagePlayerCoroutine");
            isAttacking = false;
        }
        else
        {
            Debug.Log("Hit");
            DamagePlayer(damageDealtOnHit);
            yield return new WaitForSeconds(1 / hitsPerSecond);
            StartCoroutine(DamagePlayerCoroutine());
        }
    }

    [SerializeField] private int damageDealtOnHit;
    void DamagePlayer(int damage) {
        target.GetComponent<Player>().TakeHit(damage);
    }

    bool EnemyInRadius() {
        Vector3 enemyToPlayer = transform.position - target.transform.position;
        return Mathf.Abs(enemyToPlayer.magnitude) < navmesh.stoppingDistance; //Damage player if enemy is held at its stopping distance
    }

}

/* Cool piece of code
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                navmesh.SetDestination(hit.point);
                Debug.Log(hit.point);
                Vector3 cameraPos = Camera.main.transform.position;
                Debug.DrawRay(cameraPos, (hit.point - cameraPos), Color.green, 7f);
            }
        }
        */
