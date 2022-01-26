using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Item
{

    public PostProcessManager postProcessManager;

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
        if (cDialogue != null) cDialogue.onDialogueEnd += player.EnterComa;
        else player.EnterComa();
    }

    public void Activate_Bed()
    {
        canBeInteracted = true;
        shader.ActivateShader();
    }

}
