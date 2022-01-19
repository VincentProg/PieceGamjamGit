using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDialogueManager : MonoBehaviour
{
    private float lineHeightStart = 0f;
    private float lineHeightAdd = 0f;
    private float yStartOneLine = 0f;
    
    private RectTransform dialogueBoxTransform;
    [SerializeField] private TMP_Text dialogueText;

    private void Start()
    {
        dialogueBoxTransform = dialogueText.transform.parent.gameObject.GetComponent<RectTransform>();
        
        lineHeightAdd = dialogueBoxTransform.anchoredPosition.y / 2f;
        yStartOneLine = dialogueBoxTransform.anchoredPosition.y;
        lineHeightStart = dialogueBoxTransform.anchoredPosition.y;
        dialogueText.text = "";
    }

    private void Update()
    {
        Vector3 diff = transform.position - Camera.main.transform.position;
        gameObject.transform.LookAt(diff);
    }

    public TMP_Text GetDialogueTextComponent()
    {
        return dialogueText;
    }

    public void MoveDialogueBox()
    {
        float yPos = yStartOneLine + lineHeightAdd * ((int)(dialogueBoxTransform.sizeDelta.y / yStartOneLine) - 1);
        dialogueBoxTransform.anchoredPosition = new Vector2(dialogueBoxTransform.anchoredPosition.x, yPos);
    }
}