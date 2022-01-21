using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Item
{

    public PostProcessManager postProcessManager;

    private void Start()
    {
        postProcessManager.bed = transform;
    }

    public override void Interact()
    {
        base.Interact();
        player.EnterComa();
    }

}
