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

    public int indexCurrentAction = 0;
    public List<Action> actions;

    private bool isDoingSomething = false;
    private bool isSpeaking = false;

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
        cDialogue = gameObject.AddComponent<CharacterDialogue>();
        if (actions.Count == 0) Debug.LogWarning("The visitor " + gameObject.name + " doesn't have any Action to do!");
        StartCoroutine(WaitBeforeStartAction());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLeaving)
        {
            if (isDoingSomething)
            {
                if (isSpeaking && !GameManager.Instance.isGhostMode)
                {
                    StartCoroutine(ActivateDialogue());
                    return;
                }
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
                Speak();
                break;

            case Action.ACTIONTYPE.PUTDOWN:
                PutDown();
                break;

            case Action.ACTIONTYPE.PICKUP:
                PickUp();
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
        isSpeaking = true;
    }

    IEnumerator ActivateDialogue()
    {
        isSpeaking = false;
        yield return new WaitForSeconds(5);
        if (actions[indexCurrentAction].dialogue != null)
        {
            cDialogue.SetFileParts(actions[indexCurrentAction].dialogue);
            cDialogue.onDialogueEnd += visitorManager.EndActionVisitors_AfterComa;
            cDialogue.Dialogue();
        }
    }

    public void PutDown()
    {
        Transform newItem = Instantiate(actions[indexCurrentAction].itemToPutDown.transform, actions[indexCurrentAction].slot.transform);
        actions[indexCurrentAction].slot.item = newItem.GetComponent<Item>();
        EndAction();
    }

    public void PickUp()
    {
        if (actions[indexCurrentAction].slot.item)
            Destroy(actions[indexCurrentAction].slot.item.gameObject);

        EndAction();
    }

    public void EndAction()
    {
        isDoingSomething = false;
        indexCurrentAction++;
        if (indexCurrentAction < actions.Count)
        {
            StartCoroutine(WaitBeforeStartAction());
        }
        else Leave();
    }
     
    private void Leave()
    {
        isLeaving = true;
        agent.SetDestination(exit.position);
    }

    IEnumerator WaitBeforeStartAction()
    {
        yield return new WaitForSeconds(actions[indexCurrentAction].secondsToWaitBeforeAction);      
        StartAction();
    }

}

[System.Serializable]
public class Action 
{
    public bool spawnNextCharacter;
    public float secondsToWaitBeforeAction;
    public enum ACTIONTYPE { MOVE, SPEAK, PUTDOWN, PICKUP };
    public ACTIONTYPE actionType;

    [Header("MOVE")]
    public Transform destination;

    [Header("SPEAK")] 
    public Object dialogue;
    public string startTag;

    [Header("PICKUP & PUTDOWN" )]
    public GameObject itemToPutDown;
    public Slot slot;

}

