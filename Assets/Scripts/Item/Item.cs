using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // Start is called before the first frame update
    protected GhostScript player;

    void Awake()
    {
        gameObject.AddComponent<ItemShader>();
        player = FindObjectOfType<GhostScript>();
    }

    public virtual void Interact()
    {
        print("Interact");
    }

    public virtual void StopInteraction()
    {
        player.canMove = true;
    }
}
