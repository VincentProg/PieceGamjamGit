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
    
    [SerializeField] private Object dialogueFile;

    private void Start()
    {
        SetFileParts();
        
        // Debug
        Dialogue();
    }
    
    private void SetFileParts()
    {
        if (dialogueFile == null) return;
        
        string unityFilePath = AssetDatabase.GetAssetPath(dialogueFile).Remove(0, 7);
            
        string path = Path.Combine(Application.dataPath, unityFilePath);
        StreamReader reader = new StreamReader(path);
        string testString = "";
        
        do
        {
            testString = reader.ReadLine();
            if (testString != "Null")
            {
                string nString = testString;
                lines.Add(nString);
            }

        } while (testString != null);
        
        reader.Close();
        
        if (lines.Count >= 1)
            lines.RemoveAt(lines.Count-1);
    }

    private void Dialogue()
    {
        if (dialogueFile == null) return;
        
        DialogueManager.instance.onEnd -= Dialogue;
        
        lineIndex++;

        if (lineIndex >= lines.Count)
        {
            return;
        }
        
        DialogueManager.instance.onEnd += Dialogue;
        DialogueManager.instance.StartDialogue(lines[lineIndex], "Character Name");
    }
}
