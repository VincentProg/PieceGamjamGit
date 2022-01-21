using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerAction player;

    void Awake()
    {
        gameObject.AddComponent<ItemShader>();
        player = FindObjectOfType<PlayerAction>();
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
