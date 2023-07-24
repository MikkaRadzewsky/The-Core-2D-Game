using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Type", menuName = "Unit Type")]
public class UnitType : ScriptableObject
{
    public string unitTypeName;
    public string unitSubType;
    public string weapon;
    public string description;
}
