using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Item 
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStack;
    public ItemTier itemTier;

    int itemId; // Item ID is a unique identifier of an item, useful for quick comparisons

    static int idIterator = 0; // ID Iterator is a static number that iterates for every item created, used to set the Item Id
    public static List<Item> items = new List<Item>(); //items is the list of all items that have been made

    //Variables that arent' always used 
    public BlockData thisBlock; // Used for blocks

    public float durability; //Used for tools/armors/weapons
    public float maxDurability; //Used for tools/armors/weapons
    public float miningMultiplier; //used for digging tools

    public float weapDamage; //Used for weapons
    public float attackDuration;
    public float swingLag;

    public float armorValue; //Used for armors



    void addSelf() 
    {
        items.Add(this);
    }

    public int GetItemId() 
    {
        return itemId;
    }

    public static Item GetItemByName(string name) 
    {
        foreach (Item i in items) 
        {
            if (i.itemName == name) 
            {
                return i;
            } 
        }
        return new Item(false);
    } 

    public Item() 
    {
        itemName = "Uninitialized Item";
        itemType = ItemType.none;
        maxStack = 64;

        setItemValues();

        itemId = idIterator;
        idIterator++;
        addSelf();
    }

    public Item(bool b)
    {
        itemName = "Nothing";
        itemType = ItemType.none;
        maxStack = -1;

        durability = -1;
        maxDurability = -1;

        weapDamage = -1;
        armorValue = -1;

        itemId = -1; // The id of Nothing is always -1 to ensure comparability
    }

    public Item(string name, ItemType iT) 
    {
        itemName = name;
        itemType = iT;
        maxStack = 64;
        itemTier = ItemTier.diamond;

        setItemValues();

        itemId = idIterator;
        idIterator++;
        addSelf();
    }

    public Item(string name, ItemType iT, Sprite s)
    {
        itemName = name;
        itemType = iT;
        maxStack = 64;
        itemIcon = s;

        setItemValues();

        itemId = idIterator;
        idIterator++;
        addSelf();
    }

    public Item(BlockData bd) 
    {
        itemName = bd.blockName;
        itemType = ItemType.block;
        maxStack = 64;
        itemIcon = bd.blockSprite;
        thisBlock = bd;

        setItemValues();

        itemId = idIterator;
        idIterator++;
        addSelf();
    }

    void setItemValues() 
    {
        ItemType iT = this.itemType;

        if (iT == ItemType.weapon || iT == ItemType.weaponRanged || iT == ItemType.sword || iT == ItemType.longsword || iT == ItemType.knife || iT == ItemType.throwingknife || iT == ItemType.bow || iT == ItemType.arrow || iT == ItemType.gun)
        {
            weapDamage = 2;
            durability = 100;
            maxDurability = 100;
            swingLag = .5f;
            attackDuration = 1;
            //unused for this type
            armorValue = -1;
            miningMultiplier = 0;
        }
        else if (iT == ItemType.armor)
        {
            armorValue = 1;
            durability = 100;
            maxDurability = 100;
            //unused for this
            weapDamage = -1;
        }
        else if (iT == ItemType.tool || iT == ItemType.pickaxe || iT == ItemType.shovel || iT == ItemType.axe)
        {
            miningMultiplier = 0;
            durability = 100;
            maxDurability = 100;

            //unused for this type
            armorValue = -1;
            weapDamage = -1;
            swingLag = -1;
            attackDuration = -1;
        }
        else
        {
            durability = -1;
            maxDurability = -1;
            weapDamage = -1;
            armorValue = -1;
        }
    }

}
