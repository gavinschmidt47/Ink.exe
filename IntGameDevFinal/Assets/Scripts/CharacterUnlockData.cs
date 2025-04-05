using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterUnlockData", menuName = "Scriptable Objects/CharacterUnlockData")]
public class CharacterUnlockData : ScriptableObject
{
    public List<CharacterData> characters;
}
