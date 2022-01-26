using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowers : Item
{
    CharacterDialogue cDialogue;
    [SerializeField] Object dialogue;

    protected override void Start()
    {
        base.Start();
        cDialogue = gameObject.AddComponent<CharacterDialogue>();
    }

    public override void Interact()
    {
        base.Interact();
        cDialogue.SetFileParts(dialogue);
        cDialogue.onDialogueEnd += StopInteraction;
        cDialogue.Dialogue();

    }
}
