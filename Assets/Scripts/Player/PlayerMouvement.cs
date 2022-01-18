using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMouvement : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogWarning("the Player need the component NavMeshAgent !");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Move();
        }
    }

    void Move()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = 1 << 8;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
        {
            print(hit.transform.name);
            agent.SetDestination(hit.point);
        }
    }
}
