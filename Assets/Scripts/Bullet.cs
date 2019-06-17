using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static int damage = 50;

    void Start() {
        Destroy(gameObject, 7f);
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag != "Gun")
        {
            Destroy(gameObject);
        }
    }
        
}
