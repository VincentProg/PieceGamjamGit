using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogStart : MonoBehaviour
{
    protected CharacterDialogue cDialogue;
    [SerializeField] Object dialogue1_WhileComa;
    [SerializeField] Object dialogue2_JustAfterComa;
    ComaScript comaScript;
    GhostScript ghostScript;

    [SerializeField] Visitor[] firstVisitors = new Visitor[3];

    Bed bed;
    Flowers flowers;

    private void Start()
    {
        cDialogue = gameObject.AddComponent<CharacterDialogue>();
        comaScript = FindObjectOfType<ComaScript>();
        ghostScript = FindObjectOfType<GhostScript>();

        bed = FindObjectOfType<Bed>();
        bed.isEnterComa = false;
        StartCoroutine(Wait());

        flowers = FindObjectOfType<Flowers>();



    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        cDialogue.SetFileParts(dialogue1_WhileComa);
        cDialogue.onDialogueEnd += comaScript.ActivateLoad;
        cDialogue.onDialogueEnd += SpawnVisitors;
        cDialogue.Dialogue();
        comaScript.comaExitDelegate += ActionAfterComa;
        print("Wait");
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

        }
        ghostScript.canMove = false;
        cDialogue.SetFileParts(dialogue2_JustAfterComa);
        cDialogue.onDialogueEnd += ghostScript.EndInteraction;

        cDialogue.onDialogueEnd += bed.ActivateItem;

        bed.endInteraction += flowers.ActivateItem;
        flowers.endInteraction += ActivateComa;
        flowers.endInteraction += EndInteractionBedFinal;
        cDialogue.Dialogue();
        print("Dialog2");
    }

    void ActivateComa()
    {
        bed.isEnterComa = true;
    }

    void EndInteractionBedFinal()
    {
        print("endflowers");
        bed.endInteraction += callEndScene;
    }

    void callEndScene()
    {
        print("startco");
        StartCoroutine(EndScene());
    }

    IEnumerator EndScene()
    {
        print("co");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
