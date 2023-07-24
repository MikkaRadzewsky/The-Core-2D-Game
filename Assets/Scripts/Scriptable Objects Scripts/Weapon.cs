using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{ 
    public string weaponName;
    public int strength;
    public int minRange;
    public int maxRange;


    public Weapon (string weaponName, int strength, int minRange, int maxRange)
    {
        this.weaponName = weaponName;
        this.strength = strength;
        this.minRange = minRange;
        this.minRange = maxRange;
    }
}
