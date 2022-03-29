using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    EnemyAI ai;
    public float resist;
    public float immunityTime;
    public float timeout;
    private string enemyName;
    GameObject target;
    public float enemyDamage;

    bool alwaysAggro;
    bool canWearArmor;

    bool invincibilityFrame = false;

    static List<Enemy> enemies;

    public string getName() { return enemyName; }

    public override void Damage(float dmg) 
    {
        if (timeout <= 0)
        {
            if (resist > 0)
            {
                HP -= (Mathf.Abs(dmg) * resist) ;
            }
            else
            {
                HP -= dmg  ;
            }
            timeout = immunityTime;
        }
        else
        { 
        }

        Debug.Log($"Was hit for {dmg}");

        DeathCheck();
    }

    public override void Die() 
    {
        Debug.Log($"{enemyName} Died!");
        //Destroy(gameObject);
    }

    public Enemy() 
    {
        HP = 100;
        maxHP = HP;
        resist = 0;
        strength = 10;
        magic = 10;
        faith = 10;
        dexterity = 10;
           alwaysAggro = true;
        canWearArmor = false;
        enemyName = "unnamed";
        enemyDamage = 10f;
    }

    void Update() 
    {
        if (timeout > 0) 
        {
            timeout -= Time.deltaTime;
        }
    }
    
}
