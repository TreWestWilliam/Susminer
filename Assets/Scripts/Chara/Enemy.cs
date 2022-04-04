using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    EnemyAI ai;
    public float resist;
    public float immunityTime;
    public float timeout;
    public string enemyName;
    GameObject target;
    public GameObject body;
    public float enemyDamage;

    public bool alwaysAggro; // Always aggro is for TestAi stuff
    public bool aggro; // This swaps to true when hit, always aggro will overrite this
    public bool canWearArmor;
    private bool isDead =false;

    public bool showHealth = false;

    public bool invincibilityFrame = false;

    //static List<Enemy> enemies;

    public bool getDead() { return isDead; }

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
        aggro = true;
        showHealth = true;

        Debug.Log($"Was hit for {dmg}");

        DeathCheck();
    }

    public override void Die() 
    {
        Debug.Log($"{enemyName} Died!");
        isDead = true;
        Destroy(body);
    }
    public EnemyAI getAi() { return ai; }
    public GameObject getTarget() { return target; }
    public void setTarget(GameObject t) { target = t; }

    public Enemy() 
    {
        HP = 10;
        maxHP = HP;
        resist = 0;
        strength = 10;
        magic = 10;
        faith = 10;
        dexterity = 10;
        immunityTime = .5f;
        alwaysAggro = true;
        aggro = false;
        canWearArmor = false;
        enemyName = "unnamed";
        enemyDamage = 10f;
        ai = EnemyAI.zombie;

    }

    void Update() 
    {
        if (timeout > 0) 
        {
            timeout -= Time.deltaTime;
        }
    }
    
}
