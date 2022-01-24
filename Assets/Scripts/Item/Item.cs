using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // Start is called before the first frame update
    protected GhostScript player;
    [SerializeField] bool canActivateBed;
    protected bool canBeInteracted = true;
    protected ItemShader shader;

    protected virtual void Awake()
    {
        shader = gameObject.AddComponent<ItemShader>();    
        player = FindObjectOfType<GhostScript>();
    }

    private void Start()
    {
        if (!canBeInteracted)
        {
            print(gameObject.name);
            shader.DeactivateShader();
        }
    }

    public virtual void Interact()
    {
        if (canBeInteracted)
        {
            player.canMove = false;
            Deactivate_Item();
            print("Interact");
        }
    }

    public virtual void StopInteraction()
    {
        player.canMove = true;
        if (canActivateBed) Activate_Bed();
    }

    public virtual void Activate_Bed()
    {
        FindObjectOfType<Bed>().Activate_Bed();
    }

    private void Deactivate_Item()
    {
        shader.DeactivateShader();
        canBeInteracted = false;
    }

}
