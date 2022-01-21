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

    // Debug
    public Object dilogue;
    
    public delegate void DialogueEnd();
    public event DialogueEnd onDialogueEnd;

    public void SetFileParts(Object _dialogue)
    {
        if (_dialogue == null) return;
        
        string unityFilePath = AssetDatabase.GetAssetPath(_dialogue).Remove(0, 7);
            
        string path = Path.Combine(Application.dataPath, unityFilePath);
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
        
        DialogueManager.instance.onEnd += Dialogue;
        DialogueManager.instance.StartDialogue(lines[lineIndex], "Character Name");
    }
}
