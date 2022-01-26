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

    protected CharacterDialogue cDialogue;
    [SerializeField] protected Object dialogue;


    protected virtual void Awake()
    {
        cDialogue = gameObject.AddComponent<CharacterDialogue>();
        shader = gameObject.AddComponent<ItemShader>();    
        player = FindObjectOfType<GhostScript>();
    }

    protected virtual void Start()
    {
        if (!canBeInteracted)
        {
            shader.DeactivateShader();
        }

        if (isImportant)
        {
            ImportantItems.Instance.AddImportantItem(this);
        }
    }

    public virtual void Interact(bool canBeDeactivated = true)
    {
        if (canBeInteracted)
        {
            player.canMove = false;
            if(canBeDeactivated) Deactivate_Item();
            if (dialogue != null)
            {
                cDialogue.SetFileParts(dialogue);
                cDialogue.onDialogueEnd += StopInteraction;
                cDialogue.Dialogue();
            }
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
