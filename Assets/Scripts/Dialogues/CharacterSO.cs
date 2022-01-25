using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELanguage
{
    FR,
    EN
}

[System.Serializable]
public struct SCharacter
{
    [System.Serializable]
    public struct LanguageName
    {
        public ELanguage language;
        public string charactername;
    }
    
    public string characterTagName;
    public Color colorName;

    public LanguageName[] LanguageNames;
}

[CreateAssetMenu(fileName = "CharacterList", menuName = "SO/CharacterList", order = 1)]
public class CharacterSO : ScriptableObject
{
    public SCharacter[] characters;
}