using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    private CharacterDialogue cDialogue;
    
    public VisitorManager visitorManager;

    public VisitorDatas myDatas;

    // MOVEMENT
    private NavMeshAgent agent;

    public List<Action> actions;
    private int indexCurrentAction = 0;
    private bool isDoingSomething = false;

    [HideInInspector]
    public Transform exit;
    private bool isLeaving;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        gameObject.SetActive(false);
    }

    private void Start()
    {
        cDialogue = GetComponent<CharacterDialogue>();
        
        StartAction();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLeaving)
        {
            if (isDoingSomething)
            {
                switch (actions[indexCurrentAction].actionType)
                {
                    case Action.ACTIONTYPE.MOVE:
                        float distanceFromDestination = (transform.position - actions[indexCurrentAction].destination.position).magnitude;
                        if (agent.velocity.magnitude < 0.1f && distanceFromDestination < 4)
                        {
                            EndAction();
                        }
                        break;

                    case Action.ACTIONTYPE.SPEAK:

                        break;

                    case Action.ACTIONTYPE.PUTDOWN:

                        break;

                    case Action.ACTIONTYPE.PICKUP:

                        break;
                }
            }
        }
        else
        {
            float distanceFromDestination = (transform.position - exit.position).magnitude;
            if (agent.velocity.magnitude < 0.1f && distanceFromDestination < 4)
            {
                gameObject.SetActive(false);
            }
        }

    }

    public void StartAction()
    {
        if (actions[indexCurrentAction].spawnNextCharacter)
            visitorManager.SpawnVisitor();

        switch (actions[indexCurrentAction].actionType)
        {
            case Action.ACTIONTYPE.MOVE:
                Move(actions[indexCurrentAction].destination.position);
                break;

            case Action.ACTIONTYPE.SPEAK:
                cDialogue.SetFileParts(actions[indexCurrentAction].dialogue);
                cDialogue.onDialogueEnd += EndAction;
                cDialogue.Dialogue();
                break;

            case Action.ACTIONTYPE.PUTDOWN:

                break;

            case Action.ACTIONTYPE.PICKUP:

                break;
        }
        isDoingSomething = true;
    }

    public void Move(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void Speak()
    {

    }

    public void PutDown()
    {

    }

    public void PickUp()
    {

    }

    private void EndAction()
    {
        indexCurrentAction++;
        if (indexCurrentAction < actions.Count)
        {
            StartAction();
        }
        else Leave();
    }
     
    private void Leave()
    {
        isLeaving = true;
        agent.SetDestination(exit.position);
    }

}

[System.Serializable]
public class Action 
{
    public bool spawnNextCharacter;
    public enum ACTIONTYPE { MOVE, SPEAK, PUTDOWN, PICKUP };
    public ACTIONTYPE actionType;

    [Header("MOVE")]
    public Transform destination;

    [Header("SPEAK")] 
    public Object dialogue;
    public List<string> sentences;

    [Header("PUTDOWN")]
    public GameObject itemToPutDown;
    public Transform locationItem;

    [Header("PICKUP")]
    public GameObject itemToPickUp;

}

