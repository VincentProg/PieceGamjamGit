using System.Collections;
using UnityEngine;

public class DialogStart : MonoBehaviour
{
    CharacterDialogue cDialogue;
    [SerializeField] Object dialogue1_WhileComa;
    [SerializeField] Object dialogue2_JustAfterComa;
    ComaScript comaScript;
    GhostScript ghostScript;

    [SerializeField] Visitor[] firstVisitors = new Visitor[3];
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
        cDialogue.onDialogueEnd += SpawnVisitors;
        cDialogue.Dialogue();
        comaScript.comaExitDelegate += ActionAfterComa;
    }

    void SpawnVisitors()
    {
        foreach(Visitor visitor in firstVisitors)
        {
            visitor.gameObject.SetActive(true);
        }
    }

    void ActionAfterComa()
    {
        foreach (Visitor visitor in firstVisitors)
        {
            visitor.EndAction();
            ghostScript.canMove = false;
            cDialogue.SetFileParts(dialogue2_JustAfterComa);
            cDialogue.onDialogueEnd += ghostScript.EndInteraction;
            cDialogue.Dialogue();
        }
    }
}
