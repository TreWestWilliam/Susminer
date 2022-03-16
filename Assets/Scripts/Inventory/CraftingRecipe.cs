using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipe
{
    public InventoryItem resultItem;
    public InventoryItem [] requiredItems;
    public bool requiresWorkBench;
    public bool requiresFurnace;

    public static List<CraftingRecipe> recipes = new List<CraftingRecipe>();

    void addSelf() 
    {
        recipes.Add(this);
    }

    public CraftingRecipe(InventoryItem i, InventoryItem [] r) 
    {
        resultItem = i;
        requiredItems = r;
        addSelf();
    }

    public CraftingRecipe(InventoryItem i, InventoryItem r)
    {
        InventoryItem[] arr = new InventoryItem[1];
        arr[0] = r;
        resultItem = i;
        requiredItems = arr;
        addSelf();
    }

    public string toString() 
    {
        return resultItem.item.itemName + " (" + resultItem.stack + ")"; 
    }

}
