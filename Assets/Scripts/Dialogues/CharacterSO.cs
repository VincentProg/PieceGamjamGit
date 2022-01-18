using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "SO/Character", order = 1)]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public Color colorName;
}