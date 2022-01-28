using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportantItems : MonoBehaviour
{
    public static ImportantItems Instance;
    [SerializeField] List<Item> importantItems = new List<Item>();
    Bed bed;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else Destroy(gameObject);
    }

    private void Start()
    {
       bed = FindObjectOfType<Bed>();
    }


    public void AddImportantItem(Item item)
    {
        importantItems.Add(item);
    }

    public void ActivateImportantItem(Item item)
    {
        importantItems.Remove(item);
        if(importantItems.Count == 0)
        {
            bed.ActivateItem();
        }
    }

}
