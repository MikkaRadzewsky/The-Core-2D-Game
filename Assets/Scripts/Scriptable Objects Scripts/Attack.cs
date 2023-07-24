using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    public string attackName;
    public string description;
    public Sprite sprite;
    public AttackType type;
    public Intent[] intent;
    public bool targetsFoe;
    public int numberOfTargets;
    public StatOptions stat;
    public float damageAmount;
    public float otherAmount; // buff, debuff or heal amount...
    public int coolDown; // for special attacks
}


public enum Intent
{
    Buff,
    Debuff,
    Heal,
    Damage
}
public enum StatOptions
{
    Speed,
    Strength,
    Defense,
    None
}

public enum AttackType
{
    Basic,
    Special
}