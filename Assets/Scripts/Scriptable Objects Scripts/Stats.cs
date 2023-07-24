using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stats")]
public class Stats : ScriptableObject
{

    public float strength;
    public float speed;
    public float health;
    public float shieldlevel; // defense
    public float strengthBuffLevel;
    public float strengthDebuffLevel;



    public Stats(float strength, float speed, float health, float shieldlevel, float strengthBuffLevel, float strengthDebuffLevel)
    {
        this.strength = strength;
        this.speed = speed;
        this.health = health;
        this.shieldlevel = shieldlevel;
        this.strengthBuffLevel = strengthBuffLevel;
        this.strengthDebuffLevel = strengthDebuffLevel;

    }

    public override string ToString()
    {
        return $"Strength: {strength}\nSpeed: {speed}\nHealth:{health}\n";
    }
}
