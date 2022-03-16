using UnityEngine;
using UnityEditor;

public class BlockData
{
    public BlockType blockType = BlockType.solid;
    public string blockName = "Undefined Block Name";
    public Sprite blockSprite;
    public Item droppedItem = new Item(false);
    Item blockAsItem;

    public ItemType minableItem;
    public float blockDurability;
    public int tier;

    public Item AsItem() 
    {
        return blockAsItem;
    }

    public BlockData(string n, Sprite s) 
    {
        blockDurability = 2f;
        tier = 0;
        minableItem = ItemType.tool;
        blockName = n;
        blockSprite = s;
        blockAsItem = new Item(this);
        droppedItem = blockAsItem;
    }

    public BlockData(string n, Sprite s, ItemType mi)
    {
        blockDurability = 2f;
        tier = 0;
        minableItem = mi;
        blockName = n;
        blockSprite = s;
        blockAsItem = new Item(this);
        droppedItem = blockAsItem;
    }

    public BlockData(string n, Sprite s, BlockType t)
    {
        blockDurability = 2f;
        tier = 0;
        minableItem = ItemType.tool;
        blockType = t;
        blockName = n;
        blockSprite = s;
        blockAsItem = new Item(this);
        droppedItem = blockAsItem;
    }
    public BlockData(string n, Sprite s, Item drop)
    {
        blockDurability = 2f;
        tier = 0;
        minableItem = ItemType.tool;
        blockName = n;
        blockSprite = s;
        droppedItem = drop;
        blockAsItem = new Item(this);
    }
    public BlockData(string n, Sprite s,int t, float bd, ItemType mi)
    {
        blockDurability = bd;
        tier = t;
        minableItem = mi;
        blockName = n;
        blockSprite = s;
        blockAsItem = new Item(this);
        droppedItem = blockAsItem;
    }

    public BlockData(string n, Sprite s, Item drop, int t, float bd, ItemType mi)
    {
        blockDurability = bd;
        tier = t;
        minableItem = mi;
        blockName = n;
        blockSprite = s;
        droppedItem = drop;
        blockAsItem = new Item(this);
    }

    public BlockData(string n, Sprite s, Item drop, int t, float bd, ItemType mi,BlockType bt)
    {
        blockDurability = bd;
        tier = t;
        minableItem = mi;
        blockName = n;
        blockSprite = s;
        droppedItem = drop;
        blockAsItem = new Item(this);
        blockType = bt;
    }
    public BlockData(string n, Sprite s, int t, float bd, ItemType mi, BlockType bt)
    {
        blockDurability = bd;
        tier = t;
        minableItem = mi;
        blockName = n;
        blockSprite = s;
        droppedItem = blockAsItem;
        blockAsItem = new Item(this);
        blockType = bt;
    }
}
