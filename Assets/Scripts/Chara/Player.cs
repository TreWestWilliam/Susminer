using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public float immunityTimer = 0;
    public float iframes = .5f;

    public int woodcuttingLevel;
    public int miningLevel;
    public int meleeLevel;
    public int rangedLevel;
    public int defenseLevel;
    public int agilityLevel;
    public int totalLevel;

    public float woodcuttingXP;
    public float miningXP;
    public float meleeXP;
    public float rangedXP;
    public float defenseXP;
    public float agilityXP;
    void Start() 
    {
        HP = 100;
        maxHP = 100;

    }

    public void gainXP(Skills skill, float xp) 
    {
        switch (skill) 
        {
            case Skills.woodcutting: woodcuttingXP += xp; break;
            case Skills.mining: miningXP += xp; break;
            case Skills.melee: meleeXP += xp; break;
            case Skills.ranged: rangedXP += xp; break;
            case Skills.defense: defenseXP += xp; break;
            case Skills.agility: agilityXP += xp; break;
        }
        checkLevelUp(skill);
    }

    void checkLevelUp(Skills skill) 
    {
        switch (skill)
        {
            case Skills.woodcutting: if (woodcuttingXP > Mathf.Pow(100, woodcuttingLevel)) { levelUp(skill); }; break;
            case Skills.mining: if (miningXP > Mathf.Pow(100, miningLevel)) { levelUp(skill); }; break;
            case Skills.melee: if (meleeXP > Mathf.Pow(100, meleeLevel)) { levelUp(skill); }; break;
            case Skills.ranged: if (rangedXP > Mathf.Pow(100, rangedLevel)) { levelUp(skill); }; break;
            case Skills.defense: if (defenseXP > Mathf.Pow(100, defenseLevel)) { levelUp(skill); }; break;
            case Skills.agility: if (agilityXP > Mathf.Pow(100, agilityLevel)) { levelUp(skill); }; break;
        }

    }

    void levelUp(Skills skill) 
    {
        Debug.Log($"{skillAsString(skill)} has leveled up!");
    }


    public override void Damage(float a) 
    {
        if (immunityTimer <= 0) 
        {
            HP -= Mathf.Abs(a);
            immunityTimer = iframes;
            Debug.Log($"Player was damaged for {a} Hp is now: {HP}");
        }
        DeathCheck();
        
    }

    

    void Update() 
    {
        if (immunityTimer > 0) 
        {
            immunityTimer -= Time.deltaTime;
        }
        
    }

    string skillAsString(Skills skill) 
    {
        switch (skill)
        {
            case Skills.woodcutting: return "Wood Cutting";
            case Skills.mining: return "Mining";
            case Skills.melee: return "Melee";
            case Skills.ranged: return "Ranged";
            case Skills.defense: return "Defense";
            case Skills.agility: return "Agility";
        }
        return "invalid skill";
    }

    public Player() { }

}
