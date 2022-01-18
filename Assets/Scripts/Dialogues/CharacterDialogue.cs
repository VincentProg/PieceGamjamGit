using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CharacterDialogue : MonoBehaviour
{
    private CharacterDialogueManager cdm;
    
    private List<string> lines = new List<string>();

    private int lineIndex = -1;
    
    [SerializeField] private Object dialogueFile;

    private void Start()
    {
        cdm = GetComponentInChildren<CharacterDialogueManager>();
        
        SetFileParts();
        
        // Debug
        Dialogue();
    }

    private void SetFileParts()
    {
        string unityFilePath = AssetDatabase.GetAssetPath(dialogueFile).Remove(0, 7);
            
        string path = Path.Combine(Application.dataPath, unityFilePath);
        StreamReader reader = new StreamReader(path);
        string testString = "";

        int i = 0;
        
        while (i < 3)
        {
            testString = reader.ReadLine();
            Debug.Log(testString);
            if (testString != "Null")
            {
                string nString = testString;
                lines.Add(nString);
            }

            i++;
        }
        
        reader.Close();
        
        if (lines.Count >= 1)
            lines.RemoveAt(lines.Count-1);
    }

    private void Dialogue()
    {
        DialogueManager.instance.onEnd -= Dialogue;
        
        lineIndex++;

        if (lineIndex >= lines.Count)
        {
            cdm.GetDialogueTextComponent().text = "";
            return;
        }
        
        string inst = EvaluateLine();

        if (inst == "Line")
        {
            DialogueManager.instance.onEnd += Dialogue;
            DialogueManager.instance.StartDialogue(lines[lineIndex], cdm);
        }
    }

    private string EvaluateLine()
    {
        if (lines[lineIndex].StartsWith("@"))
        {
            // Command
            return "Command";
        }
        else
        {
            return "Line";
        }
    }
}
