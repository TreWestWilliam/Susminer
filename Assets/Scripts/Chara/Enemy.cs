using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyAI ai;
    public float health;
    public float maxHealth;
    public float resist;
    public float immunityTime;
    public float timeout;
    GameObject target;

    bool alwaysAggro;
    bool canWearArmor;

    bool invincibilityFrame = false;

    static List<Enemy> enemies;

    public void Damage(float dmg) 
    {
        if (timeout <= 0)
        {
            if (resist > 0)
            {
                health -= (dmg * resist);
            }
            else
            {
                health -= dmg;
            }
            timeout = immunityTime;
        }
        else
        { 
        }

        Debug.Log($"Was hit for {dmg}");

        DeathCheck();
    }
    public void DeathCheck() 
    {
        if (health <= 0) 
        {
            Die();
        }
    }
    public void Die() 
    {
        Debug.Log("Died!");
        Destroy(gameObject);
    }

    public Enemy() 
    {
        health = 100;
        maxHealth = 100;
        resist = 0;
        alwaysAggro = true;
        canWearArmor = false;
    }
    void Update() 
    {
        if (timeout > 0) 
        {
            timeout -= Time.deltaTime;
        }
    }
    
}
