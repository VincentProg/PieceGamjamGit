using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : Item
{

    public AudioSource son;
    public override void Interact(bool canBeDeactivated = true)
    {
        base.Interact(false);
        son.Play();
        cDialogue.onDialogueEnd += son.Stop;
        currentIndexDialog = 0;
        canBeInteracted = true;
    }
}
