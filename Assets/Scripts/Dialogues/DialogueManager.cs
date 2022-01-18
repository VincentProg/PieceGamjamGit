using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    
    private bool isDialogueEnd = false;
    private bool breakState = false;
    private bool isTextCompleted = false;
    private string dialogueText = "";

    private CharacterDialogueManager actualCharacterDialogueManager;

    [SerializeField] private float textSpeed;

    public delegate void End();
    public event End onEnd;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void StartDialogue(string _dialogueText, CharacterDialogueManager _cDialogueManager)
    {
        isDialogueEnd = false;
        breakState = false;
        isTextCompleted = false;

        actualCharacterDialogueManager = _cDialogueManager;
        actualCharacterDialogueManager.GetDialogueTextComponent().text = "";

        dialogueText = _dialogueText;

        StartCoroutine(IEDialogue());
    }

    private IEnumerator IEDialogue()
    {
        string displayedText = "";
        int alphaIndex = 0;
        char[] textChars = dialogueText.ToCharArray();

        TMP_Text dialogueBoxText = actualCharacterDialogueManager.GetDialogueTextComponent();

        dialogueBoxText.text = dialogueText.Insert(alphaIndex, "<color=#00000000>");
        yield return new WaitForSeconds(0.01f);
        actualCharacterDialogueManager.MoveDialogueBox();

        foreach (char c in textChars)
        {
            alphaIndex++;
            dialogueBoxText.text = dialogueText;
            displayedText = dialogueBoxText.text.Insert(alphaIndex, "<color=#00000000>");
            dialogueBoxText.text = displayedText;
            
            if (breakState)
                break;

            yield return new WaitForSeconds(textSpeed * Time.deltaTime);
        }

        breakState = false;
        dialogueBoxText.text = dialogueText;
        isTextCompleted = true;

        // TEMP
        yield return new WaitForSeconds(2f);

        breakState = false;
        
        EndActualDialogue();
    }

    private void EndActualDialogue()
    {
        isDialogueEnd = true;
        isTextCompleted = true;

        if (onEnd != null)
            onEnd();
    }
}
