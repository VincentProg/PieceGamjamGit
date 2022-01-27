using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGO : MonoBehaviour
{
    public GameObject GO;
    private void Awake()
    {
        GO.SetActive(true);
    }
}
