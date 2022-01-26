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
    private string characterName = "";

    private bool isWaiting = false;
    private bool isSkipping = false;
    private bool isWaitingInput = false;

    public ELanguage language;
    public CharacterSO characterList;
    [Space]
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TMP_Text textName;
    [SerializeField] private GameObject box;
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

    private void Start()
    {
        if (box != null) box.SetActive(false);
    }

    private void Update()
    {
        if (isDialogueEnd && isWaitingInput && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            isDialogueEnd = false;
            isWaitingInput = false;
            
            if (onEnd != null)
                onEnd();
        }
    }

    public void StartDialogue(string _dialogueText)
    {
        isDialogueEnd = false;
        breakState = false;
        isTextCompleted = false;
        isWaiting = false;
        isSkipping = false;
        isWaitingInput = false;
        
        dialogueText = _dialogueText;

        StartCoroutine(IEDialogue());
    }

    private IEnumerator IEDialogue()
    {
        textName.text = characterName;
        
        float time = 0f;
        string command = EvaluateLine(dialogueText, out time);
        ComputeEvaluation(command);

        if (isWaiting)
        {
            yield return new WaitForSeconds(time);
        }
        else if (isSkipping)
        {
            
        }
        else
        {
            string displayedText = "";
            int alphaIndex = 0;
            char[] textChars = dialogueText.ToCharArray();

            textBox.text = dialogueText.Insert(alphaIndex, "<color=#00000000>");

            foreach (char c in textChars)
            {
                alphaIndex++;
                textBox.text = dialogueText;
                displayedText = textBox.text.Insert(alphaIndex, "<color=#00000000>");
                textBox.text = displayedText;
            
                if (breakState)
                    break;

                yield return new WaitForSeconds(textSpeed * Time.deltaTime);
            }
            
            textBox.text = dialogueText;
        }
        
        EndActualDialogue();
    }

    private void EndActualDialogue()
    {
        isDialogueEnd = true;
        isTextCompleted = true;
        
        if (onEnd != null && !isWaitingInput)
            onEnd();
    }

    private string EvaluateLine(string _line, out float _time)
    {
        _time = 0f;
        
        if (_line.StartsWith("@"))
        {
            // Command
            if (_line.StartsWith("@Wait"))
            {
                string strTime = "";
                for (int i = 6; i < _line.Length; i++)
                    strTime += _line[i];
                
                if (float.TryParse(strTime, out _time))
                    return "Wait";
                else
                {
                    isWaitingInput = true;
                    return "Wait";
                }
            }
            
            if (_line.StartsWith("@Blank") || _line.StartsWith("@End"))
            {
                return "Blank";
            }

            if (_line.StartsWith("@" + language))
            {
                EvaluateLineParameters(_line);
                return "Line";
            }

            if (_line.StartsWith("@Name"))
            {
                characterName = _line.Replace("@Name ", "");
                for (int i = 0; i < characterList.characters.Length; i++)
                {
                    if (characterList.characters[i].characterTagName == characterName)
                    {
                        for (int j = 0; j < characterList.characters[i].LanguageNames.Length; j++)
                        {
                            if (characterList.characters[i].LanguageNames[j].language == language)
                            {
                                characterName = characterList.characters[i].LanguageNames[j].charactername;
                                break;
                            }
                        }

                        textName.color = characterList.characters[i].colorName;
                        break;
                    }
                }
                return "Name";
            }

            return "Nothing";
        }
        
        if (_line.StartsWith("#"))
        {
            // Tag go to
            return "Nothing";
        }
        
        return "Nothing";
    }

    private void EvaluateLineParameters(string _line)
    {
        int indexOf = _line.IndexOf("@Input:");
        
        if (indexOf > -1)
        {
            int startIndex = indexOf + 7;
            string strResult = "";
            bool boolResult = false;

            while (_line[startIndex] != ' ')
            {
                strResult += _line[startIndex];

                startIndex++;
            }

            if (strResult.ToLower() == "true")
            {
                boolResult = true;
            }
            else if (strResult.ToLower() != "false")
            {
                Debug.LogError("Parameter @Input does not have a correct value.\nDefault value used: True." );
                boolResult = true;
            }

            if (boolResult)
            {
                isWaitingInput = true;
            }

            dialogueText = dialogueText.Replace("@Input:" + strResult, "");
        }
        else
        {
            // Default value True
            isWaitingInput = true;
        }
    }

    private void ComputeEvaluation(string _command)
    {
        switch (_command)
        {
            case "Line":
                // Show normal line
                dialogueText = dialogueText.Replace("@" + language + " ", "");
                box.SetActive(true);
                break;
            
            case "Wait":
                // Only wait
                isWaiting = true;
                break;
            
            case "Blank":
                // Reset textbox text
                textBox.text = "";
                textName.text = "";
                box.SetActive(false);
                isSkipping = true;
                break;

            default:
                isSkipping = true;
                break;
        }
    }
}
