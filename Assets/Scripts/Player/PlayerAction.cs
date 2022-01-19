using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAction : MonoBehaviour
{
    [HideInInspector]
    public bool canMove = true;

    // MOVEMENT
    private NavMeshAgent agent;

    // INTERACTION
    private Item lastItem;
    private bool isPreparingToInteract;
    [SerializeField] float rangeInteraction;



    // DEBUG
    [SerializeField] bool isDebug;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogWarning("the Player need the component NavMeshAgent !");
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ActivateAction();
            }

            UpdateCheckInteraction();
        }

    }

    private void OnDrawGizmos()
    {
        if (isDebug)
        {
            Gizmos.DrawSphere(transform.position, rangeInteraction);
        }
    }

    void ActivateAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
        {
            Move(hit);

            if (hit.transform.TryGetComponent<Item>(out lastItem))
            {
                isPreparingToInteract = true;
            }
            else isPreparingToInteract = false;
        }
    }

    void Move(RaycastHit hit)
    {
        agent.SetDestination(hit.point);
    }

    void UpdateCheckInteraction()
    {
        if (isPreparingToInteract && agent.velocity.magnitude < 0.1f)
        {
            float distanceToItem = (transform.position - lastItem.transform.position).magnitude;
            if (distanceToItem < rangeInteraction)
            {
                isPreparingToInteract = false;
                Interact();
            }
        }
    }

    void Interact()
    {
        canMove = false;
        lastItem.Interact();
    }
}
