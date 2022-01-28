using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // Start is called before the first frame update
    protected GhostScript player;
    [SerializeField] bool isImportant;
    [SerializeField] protected bool canBeInteracted = true;
    protected ItemShader shader;

    protected CharacterDialogue cDialogue;
    [SerializeField] protected Object[] dialogues;
    protected int currentIndexDialog = 0;

    public delegate void EndInteraction();
    public EndInteraction endInteraction;

    [SerializeField] float sensHighlight;
    protected virtual void Awake()
    {
        cDialogue = gameObject.AddComponent<CharacterDialogue>();
        shader = gameObject.AddComponent<ItemShader>();
        shader.sens = sensHighlight;
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
            if (dialogues != null && currentIndexDialog < dialogues.Length)
            {

                cDialogue.SetFileParts(dialogues[currentIndexDialog]);
                cDialogue.onDialogueEnd += StopInteraction;
                cDialogue.Dialogue();
                cDialogue.onDialogueEnd += IncrementDialogIndex;
            }
            else StopInteraction();
        }
    }

    private void IncrementDialogIndex()
    {
        currentIndexDialog++;
    }

    public virtual void StopInteraction()
    {
        player.canMove = true;
        if (isImportant) ImportantItems.Instance.ActivateImportantItem(this);
        if(endInteraction != null)
        endInteraction();
        endInteraction = null;
    }

    private void Deactivate_Item()
    {
        shader.DeactivateShader();
        canBeInteracted = false;
    }

    public void ActivateItem()
    {
        canBeInteracted = true;
        shader.ActivateShader();
    }

}
