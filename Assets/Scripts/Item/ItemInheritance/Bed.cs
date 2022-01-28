using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Item
{

    public PostProcessManager postProcessManager;
    public bool isEnterComa = true;

    protected override void Awake()
    {
        base.Awake();
        postProcessManager.bed = transform;
        canBeInteracted = false;

    }


    public override void Interact(bool canBeDeactivated = true)
    {
        if (!canBeInteracted) return;
        base.Interact();
        print(isEnterComa);
        if (currentIndexDialog < dialogues.Length && isEnterComa)
        {
            cDialogue.onDialogueEnd += player.EnterComa;
        }
        else if (isEnterComa)
        {
            player.EnterComa();
        }
    }


}
