using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CharacterEngine : MonoBehaviour
{
   CombatManager combatManager;

   public void startAttack(Attack attack, CharacterInstance user, CharacterInstance target)
    {
        foreach( Intent i in attack.intent)
        {
            switch (i)
            {
                case Intent.Buff:
                    addBuff(attack.stat, target, attack.otherAmount);
                    break;
                case Intent.Debuff:
                    addDebuff(attack.stat, target, attack.otherAmount);
                    break;
                case Intent.Heal:
                    heal(target, attack.otherAmount);
                    break;
                case Intent.Damage:
                        //                 Attack Power     +            User Strength(including strength buffs if there are any)  -    target shield level(defense) 
                        var newAmount = attack.damageAmount + (user.currentStats.strength+(5*user.currentStats.strengthBuffLevel)) - (5 * target.currentStats.shieldlevel);
                        StartCoroutine(TakeDamageCoroutine(target, newAmount));
                    break;
                default:
                    return;
            }
        }
    }



    public void  addBuff(StatOptions stat, CharacterInstance target, float amount)
    {
        var buffBar = target.transform.GetChild(0).gameObject;
            buffBar.SetActive(true);

        switch (stat)
            {
                case StatOptions.Strength:
                    target.currentStats.strength += amount;
                    //Add sprite
                    break;
                case StatOptions.Speed:
                    target.currentStats.speed += amount;
                    //Add sprite
                    break;
                case StatOptions.Defense:
                    target.currentStats.shieldlevel += amount;
                        buffBar.transform.GetChild(0).gameObject.SetActive(true);
                break;
                default:
                    return;
            }
    }
    public void addDebuff(StatOptions stat, CharacterInstance target, float amount)
    {
        //Add...
            switch (stat)
            {
                case StatOptions.Strength:
                    target.currentStats.strength -= amount;
                    break;
                case StatOptions.Speed:
                    target.currentStats.speed -= amount;
                    break;
                case StatOptions.Defense:
                    target.currentStats.shieldlevel -= amount;
                    break;
                default:
                    return;
            }
    }


    public void heal(CharacterInstance target, float amount)
    {
        target.currentStats.health += amount;
        if(target.currentStats.health > target.character.coreStats.health)
        {
            target.currentStats.health = target.character.coreStats.health;
        }
    }


    private IEnumerator TakeDamageCoroutine(CharacterInstance target, float amount)
    {
        yield return new WaitForSeconds(0.5f);
        target.animator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        target.currentStats.health -= amount;
    }
}
