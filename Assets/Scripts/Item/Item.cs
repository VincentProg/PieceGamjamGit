using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // Start is called before the first frame update
    protected GhostScript player;
    [SerializeField] bool isImportant;
    protected bool canBeInteracted = true;
    protected ItemShader shader;

    protected virtual void Awake()
    {
        shader = gameObject.AddComponent<ItemShader>();    
        player = FindObjectOfType<GhostScript>();
    }

    protected virtual void Start()
    {
        if (!canBeInteracted)
        {
            print(gameObject.name);
            shader.DeactivateShader();
        }

        if (isImportant)
        {
            ImportantItems.Instance.AddImportantItem(this);
        }
    }

    public virtual void Interact()
    {
        if (canBeInteracted)
        {
            player.canMove = false;
            Deactivate_Item();
        }
    }

    public virtual void StopInteraction()
    {
        player.canMove = true;
        if (isImportant) ImportantItems.Instance.ActivateImportantItem(this);
    }

    private void Deactivate_Item()
    {
        shader.DeactivateShader();
        canBeInteracted = false;
    }

}
