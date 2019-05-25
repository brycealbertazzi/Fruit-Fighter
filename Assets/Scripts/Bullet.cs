using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timeTilColliderEnable = 0.1f;
    void Start() {
        Invoke("EnableCollider", timeTilColliderEnable);
        Destroy(this.gameObject, 7f);
    }

    void EnableCollider() {
        this.GetComponent<BoxCollider>().enabled = true;
    }

    void OnTriggerEnter(Collider col) {
        Destroy(this.gameObject);
    }
        
}
