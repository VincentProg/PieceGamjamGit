using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CharacterDialogue : MonoBehaviour
{
    private List<string> lines = new List<string>();

    private int lineIndex = -1;

    public delegate void DialogueEnd();
    public event DialogueEnd onDialogueEnd;

    public void SetFileParts(Object _dialogue)
    {
        if (_dialogue == null) return;
        lines = new List<string>();
        lineIndex = -1;

        string unityFilePath = _dialogue.name + ".txt";
        
        string path = Path.Combine(Application.streamingAssetsPath + "/Dialogues/", unityFilePath);
        Debug.LogError(path);
        StreamReader reader = new StreamReader(path);
        string testString;
        
        do
        {
            testString = reader.ReadLine();
            if (testString != null )
            {
                if (testString.Replace(" ", "") != "")
                {
                    string nString = testString;
                    lines.Add(nString);
                }
            }

        } while (testString != null);
        
        reader.Close();
    }

    public void Dialogue()
    {
        if (lines.Count <= 0f) return;
        
        DialogueManager.instance.onEnd -= Dialogue;
        
        lineIndex++;

        if (lineIndex >= lines.Count)
        {
            if (onDialogueEnd != null)
            {
                onDialogueEnd();
                onDialogueEnd = null;
            }

            return;
        }
        
        if (lines[lineIndex] == "@End")
        {
            DialogueManager.instance.StartDialogue("@Blank");
            
            if (onDialogueEnd != null)
            {
                onDialogueEnd();
                onDialogueEnd = null;
            }

            return;
        }
        
        DialogueManager.instance.onEnd += Dialogue;
        DialogueManager.instance.StartDialogue(lines[lineIndex]);
    }
    
    public void Dialogue(string _startTag)
    {
        if (lines.Count <= 0f) return;
        
        DialogueManager.instance.onEnd -= Dialogue;

        if (!string.IsNullOrEmpty(_startTag))
            lineIndex = lines.IndexOf('#' + _startTag);
        else
            lineIndex = 0;

        if (lineIndex < 0) lineIndex = 0;
        
        if (lineIndex >= lines.Count)
        {
            if (onDialogueEnd != null)
            {
                onDialogueEnd();
                onDialogueEnd = null;
            }

            return;
        }
        
        DialogueManager.instance.onEnd += Dialogue;
        DialogueManager.instance.StartDialogue(lines[lineIndex]);
    }
}
