using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    protected float HP;
    protected float maxHP;
    protected int strength;
    protected int magic;
    protected int faith;
    protected int dexterity;

    public bool isImmune;
    public bool hitStun;

    public float getHP() { return HP; }

    public float getMaxHP() { return maxHP; }

    public virtual void Heal(float a) 
    {
        HP += Mathf.Abs(a);
    }

    public virtual void Damage(float a) 
    {
        HP -= Mathf.Abs(a);
    }

    public virtual void DeathCheck()
    {
        if (HP <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log("Died!");
    }
    public CharacterBase() 
    {
        HP = 100;
        maxHP = HP;
        strength = 10;
        magic = 10;
        faith = 10;
        dexterity = 10;
    }

    public CharacterBase( int _hp)
    {
        HP = _hp;
        maxHP = HP;
        strength = 10;
        magic = 10;
        faith = 10;
        dexterity = 10;
    }

}
