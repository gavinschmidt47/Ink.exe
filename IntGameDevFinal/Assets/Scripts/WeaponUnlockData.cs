using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponUnlockData", menuName = "Scriptable Objects/WeaponUnlockData")]
public class WeaponUnlockData : ScriptableObject
{
    public List<WeaponData> weapons;
}
