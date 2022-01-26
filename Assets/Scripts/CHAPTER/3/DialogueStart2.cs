using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStart2 : MonoBehaviour
{
    protected CharacterDialogue cDialogue;
    [SerializeField] Object dialogue1_WhileComa;
    [SerializeField] Object dialogue2_JustAfterComa;
    ComaScript comaScript;
    GhostScript ghostScript;

    private void Start()
    {
        cDialogue = gameObject.AddComponent<CharacterDialogue>();
        comaScript = FindObjectOfType<ComaScript>();
        ghostScript = FindObjectOfType<GhostScript>();

        StartCoroutine(Wait());


    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        cDialogue.SetFileParts(dialogue1_WhileComa);
        cDialogue.onDialogueEnd += comaScript.ActivateLoad;
        cDialogue.Dialogue();
        comaScript.comaExitDelegate += ActionAfterComa;
    }

    void ActionAfterComa()
    {
        ghostScript.canMove = false;
        cDialogue.SetFileParts(dialogue2_JustAfterComa);
        cDialogue.onDialogueEnd += ghostScript.EndInteraction;
        cDialogue.onDialogueEnd += AfterDialogue;
        cDialogue.Dialogue();
    }

    void AfterDialogue()
    {
        FindObjectOfType<VisitorManager>().SpawnVisitor();
    }
}
