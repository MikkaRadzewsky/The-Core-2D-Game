using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInstance : MonoBehaviour
{
    public Character character;
    public Animator animator;
    public Image healthBar;
    public bool isDead;
    public int cooldownTurnsLeft;
    [HideInInspector] public bool attackExecutionComplete;
    public Stats currentStats;


    private void Start()
    {
        isDead = false;
        gameObject.SetActive(true);
    }

    void Awake()
    {
        currentStats = new Stats(character.coreStats.strength,
                                 character.coreStats.speed,
                                 character.coreStats.health,
                                 character.coreStats.shieldlevel,
                                 character.coreStats.strengthBuffLevel,
                                 character.coreStats.strengthDebuffLevel);

        Debug.Log($"{character.characterName}: {character.coreStats}");
        //var specialAttack = Array.Find(character.attacks, element => element.type == AttackType.Special);
        cooldownTurnsLeft = 0;
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        healthBar.fillAmount = currentStats.health / character.coreStats.health;
    }
}


