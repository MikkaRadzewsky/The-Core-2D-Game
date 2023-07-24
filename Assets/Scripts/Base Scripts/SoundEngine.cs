using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEngine : MonoBehaviour
{
    public AudioSource hit;
    public AudioSource basic;
    public AudioSource special;
    public AudioSource death;

    public void DamageSound()
    {
        hit.Play();
    }

    public void BasicAttackSound()
    {
        basic.Play();
    }

    public void SpecialAttackSound()
    {
        special.Play();
    }

    public void DeathSound()
    {
        death.Play();
    }
}
