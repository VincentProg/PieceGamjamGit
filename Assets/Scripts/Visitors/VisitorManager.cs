using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorManager : MonoBehaviour
{
    public List<Visitor> visitors;
    int currentIndex = 0;

    ComaScript comaScript;

    private void Start()
    {
        SpawnVisitor();
        comaScript = FindObjectOfType<ComaScript>();
    }

    public void SpawnVisitor()
    {
        if (currentIndex < visitors.Count)
        {
            visitors[currentIndex].visitorManager = this;
            visitors[currentIndex].transform.position = transform.position;
            visitors[currentIndex].gameObject.SetActive(true);
            visitors[currentIndex].exit = transform;
            currentIndex++;
        }
    }

    public void EndActionVisitors_AfterComa()
    {
        foreach(Visitor visitor in visitors)
        {
            if(visitor.indexCurrentAction < visitor.actions.Count &&  visitor.actions[visitor.indexCurrentAction].actionType == Action.ACTIONTYPE.SPEAK)
            {
                visitor.EndAction();
            }
        }
        comaScript.ActivateLoad();
    }
}
