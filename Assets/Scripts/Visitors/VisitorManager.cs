using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorManager : MonoBehaviour
{
    public List<Visitor> visitors;
    int currentIndex = 0;

    private void Start()
    {
        SpawnVisitor();
        
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
}
