using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public Image [] selectorSprites;

    public Image selectorImg;
    public int itemSelector = 0;



    Vector2 downScroll = new Vector2(0, -1);
    Vector2 upScroll = new Vector2(0, 1);

    public List<Item> itemIndex = Item.items; //Used to be its own thing but I implemented the list as a static within the item class itself making it redundant
    //The new solution has the advantage of easily adding to the list from the constructor

    int usedSlots = 0;
    int inventorySize = 40;
    bool initializeInv = false;

    public BlockManager BM;
    public UserInterfaceManager UIM;

    //Awake is called at the start of the scene
    void Awake() 
    {
        
    }
    

    // Start is called before the first frame update
    void Start()
    {
        createItems();
        createRecipes();

        itemSelector = 0;

        for (int i = 0; i < inventorySize; i++) 
        {
            inventory.Add( new InventoryItem() );
        }

        //logItems();
        
        //printInventory();
    }

   

    public void logItems() 
    {
        foreach (Item thisItem in Item.items) 
        {
            Debug.Log("Item index:" + thisItem.GetItemId() + ":" + thisItem.itemName);
        }
    }

    public void printInventory() 
    {
        int iterator = 0;
        foreach (InventoryItem thisItem in inventory)
        {
            Debug.Log("Inventory Item:" + iterator + " : " + thisItem.item.itemName + " Count: " + thisItem.stack);
            iterator++;
        }
    }

    public void addItem(Item myItem) //Adding single item by Item Object
    {
        int emptyIterator = -1;
        int iterator = 0;
        foreach (InventoryItem i in inventory)  // Foreach inventory item in the inventory
        {
            if (i.item == myItem) 
            {
                if (i.stack < i.item.maxStack)
                {
                    i.stack++; // Adding one since there'll only be one addition
                    return;
                }
                else if (i.isEmpty == false) //If the Object is empty
                {
                    emptyIterator = iterator; // Then store it for later
                }
            }
            iterator++;
        }
        //If we reach this point then we havent found an existing stack
        if (usedSlots < inventorySize) //Check if we have space in inventory
        {
            inventory[emptyIterator] = new InventoryItem(myItem); // Set the empty object to 
            usedSlots++;
        }
    }

    public void addItem(InventoryItem myInvItem)
    {
        int emptyIterator = -1;
        int iterator = 0;
        foreach (InventoryItem i in inventory)  // Foreach inventory item in the inventory
        {
            if (i.item == myInvItem.item)
            {
                if (i.stack < i.item.maxStack)
                {
                    i.stack += myInvItem.stack; // Adding one since there'll only be one addition
                    return;
                }
                
            }
            else if (i.isEmpty == true) //If the Object is empty
            {
                if (emptyIterator == -1) 
                { 
                    emptyIterator = iterator; // Then store it for later
                }
            }

            iterator++;
        }
        //If we reach this point then we havent found an existing stack
        if (usedSlots < inventorySize) //Check if we have space in inventory
        {
            if (emptyIterator == -1) { return; }
            inventory[emptyIterator] = myInvItem; // Set the empty object to 
            usedSlots++;
        }
        printInventory();
    }

    public void removeItem(int pos) 
    {
        inventory[pos] = new InventoryItem();
    }

    public void removeItem(Item thisItem) 
    {
        for (int i =0; i < inventorySize; i++)
        {
            if ( inventory[i].item == thisItem) 
            {
                inventory[i] = new InventoryItem();
                return;
            }
        }
    }

    public void subtractItem(int pos) 
    {
        inventory[pos].stack--;
    }

    public void subtractItem(Item thisItem)
    {

        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory[i].item == thisItem)
            {
                inventory[i].stack--;
                return;
            }
        }

    }

    public void subtractItem(int pos, int count)
    {
        inventory[pos].stack -= count;
    }

    public void subtractItem(Item thisItem, int count)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory[i].item == thisItem)
            {
                if (inventory[i].stack > count) 
                { 
                    inventory[i].stack -= count;
                    break;
                }
                else 
                {
                    int newCount = count - inventory[i].stack;
                    removeItem(i);
                    subtractItem(thisItem, newCount);
                    break;
                }
            }
        }
    }

    public void swapItem(int pos1, int pos2) 
    {
        InventoryItem item1 = inventory[pos1];
        inventory[pos1] = inventory[pos2];
        inventory[pos2] = item1;
    }

    public int countItem(Item item) 
    {
        int myCount = 0;
        foreach (InventoryItem inv in inventory) 
        {
            if (inv.item.GetItemId() == item.GetItemId()) 
            {
                myCount += inv.stack;
            }
        }
        return myCount;
    }

    public Item getSelectedItem()
    {
        return inventory[itemSelector].item;
    }

    void updateItemBar() 
    {

        for (int i =0; i < 10; i++)
        {

            //Debug.Log(selectorSprites[i]);
            
            if (inventory[i].isEmpty == true)
            {
                selectorSprites[i].gameObject.SetActive(false);
            }
            else 
            {
                selectorSprites[i].gameObject.SetActive(true);
                if (inventory[i].item.itemIcon != null) 
                {
                    selectorSprites[i].sprite = inventory[i].item.itemIcon;
                }
            }  
        }

        selectorImg.gameObject.GetComponent<RectTransform>().localPosition = selectorSprites[itemSelector].gameObject.GetComponent<RectTransform>().localPosition;
        selectorImg.gameObject.GetComponent<RectTransform>().anchoredPosition = selectorSprites[itemSelector].gameObject.GetComponent<RectTransform>().anchoredPosition;
        selectorImg.gameObject.GetComponent<RectTransform>().SetParent(selectorSprites[itemSelector].gameObject.GetComponent<RectTransform>().parent) ;
    }
    void scrollUp() 
    {
        if (itemSelector < 9)
        {
            itemSelector++;
        }
        else
        {
            itemSelector = 0;
        }
    }

    void scrollDown()
    {
        if (itemSelector > 0)
        {
            itemSelector--;
        }
        else
        {
            itemSelector = 9;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!initializeInv) 
        {
            //createItems();
            //logItems();
            
            inventory[0] = new InventoryItem(itemIndex[0]);
            inventory[1] = new InventoryItem(itemIndex[1]);
            inventory[2] = new InventoryItem(itemIndex[2]);
            inventory[3] = new InventoryItem(itemIndex[3]);
            inventory[4] = new InventoryItem(itemIndex[4]);
            inventory[5] = new InventoryItem(itemIndex[5]);
            inventory[5] = new InventoryItem(itemIndex[8], 5);
            inventory[6] = new InventoryItem( Item.GetItemByName("Wooden Sword") );
            inventory[7] = new InventoryItem(Item.GetItemByName("Pickaxe"));
            inventory[8] = new InventoryItem(Item.GetItemByName("Shovel"));
            usedSlots = 3;
            
            //printInventory();
            initializeInv = true;
        }

        updateItemBar();

        if (Input.GetButtonDown("Inventory")) 
        {
            //Debug.Log("Inventory Pressed");
            UIM.toggleInventory();
        }
        // Get number inputs for selecting the items
        { //This is only here to hide it in the editor
        if (Input.GetKey("1")) 
        {
            itemSelector = 0;
        }
        if (Input.GetKey("2"))
        {
            itemSelector = 1;
        }

        if (Input.GetKey("3"))
        {
            itemSelector = 2;
        }
        if (Input.GetKey("4"))
        {
            itemSelector = 3;
        }
        if (Input.GetKey("5"))
        {
            itemSelector = 4;
        }
        if (Input.GetKey("6"))
        {
            itemSelector = 5;
        }
        if (Input.GetKey("7"))
        {
            itemSelector = 6;
        }
        if (Input.GetKey("8"))
        {
            itemSelector = 7;
        }
        if (Input.GetKey("9"))
        {
            itemSelector = 8;
        }
        if (Input.GetKey("0"))
        {
            itemSelector = 0;
        }
        }
        //Scroll to get item
        if (Input.mouseScrollDelta == upScroll)
        {
            scrollUp();
        }
        else if (Input.mouseScrollDelta == downScroll) 
        {
            scrollDown();
        }

    }


    void createItems() 
    {
        blocksToItems();
    
    }

    void blocksToItems() // No longer does anything, every block should have a related item 
    {
        Debug.Log("Blocks to Items:");
        if (BM == null) { return; }
        /*
        foreach (BlockData b in BM.blocks)
        {
            new Item(b);
            Debug.Log("Created item from block " + b.blockName);
        }
        */
    }

    void createRecipes()
    {
        Debug.Log("Creating Recipes");

        //new CraftingRecipe();
        //Log to 
        
        CraftingRecipe cr = new CraftingRecipe(
            new InventoryItem(Item.GetItemByName("Wood Plank"), 4),
             new InventoryItem( Item.GetItemByName("Wood Log") 
            ));
        new CraftingRecipe(
            new InventoryItem(Item.GetItemByName("Wood Plank"), 2),
             new InventoryItem(Item.GetItemByName("Wood Log")
            ));
        new CraftingRecipe(
            new InventoryItem(Item.GetItemByName("Wood Plank"), 8),
             new InventoryItem(Item.GetItemByName("Wood Log")
            ));
        Debug.Log(cr);

        foreach( CraftingRecipe my in CraftingRecipe.recipes) 
        {
            Debug.Log(my.toString());
        }
        Debug.Log(CraftingRecipe.recipes);
        
    }

    public void CraftItem( CraftingRecipe CR ) 
    {
        //Check if we have the items required
        Debug.Log("Checking if we have materials to make item");
        foreach (InventoryItem ii in CR.requiredItems)
        {
            if ( (ii.stack > countItem(ii.item) ) ) 
            {
                Debug.Log("Required Item :" + ii.item.itemName + " Requires: " + ii.stack + " Have: " + countItem(ii.item));
                return;
            }
        }
        //Remove items used
        Debug.Log("Removing items used");
        foreach (InventoryItem ii in CR.requiredItems) 
        {
            subtractItem(ii.item, ii.stack);
        }
        Debug.Log("Giving Item");
        //Give item(s)
        addItem(CR.resultItem);
    
    } 

}
