using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private GameObject target;
    private NavMeshAgent navmesh;
    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navmesh.SetDestination(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

        /*
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
    }


}
