using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public Sprite image;
    public UnitType unitType;
    public Attack[] attacks;
    public Stats coreStats;
    public Stats currentStats;
    public Aleigence aleigence;

}


public enum Aleigence
{
    Ally,
    Enemy
}