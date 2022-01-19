using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.AddComponent<ItemShader>();
    }

    public virtual void Interact()
    {
        print("Interact");
    }

    public virtual void StopInteraction()
    {

    }
}
