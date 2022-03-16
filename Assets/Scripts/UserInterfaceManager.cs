using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    //Other Manger Instances
    public BlockManager BM;
    public InventoryManager IM;
    //UI Objects
    public Image InventoryPanel;
    public Image CraftingPanel;
    public Button [] CraftingSelectors;
    public Image InventoryMenu;
    Image[] inventoryUISlots;

    public Image HotBarPanel;
    Image[] HotBarImages = new Image[10];

    CraftingRecipe temp;

    public InterfaceState iState = InterfaceState.def;

    int craftingScroll = 0;

    bool test = true;

    void Awake() 
    {
        initializeInventoryObjects();
    }

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (test) 
        {
            temp = new CraftingRecipe(
            new InventoryItem(Item.GetItemByName("Wood Plank"), 20),
             new InventoryItem(Item.GetItemByName("Wood Log")
            ));
            test = false;

            initializeCrafting();
        }

        if (iState == InterfaceState.inv)
        {
            if (InventoryMenu.gameObject.activeInHierarchy == false) 
            {
                InventoryMenu.gameObject.SetActive(true);
            }
            drawInventoryItems();
        }
        else if (InventoryMenu.gameObject.activeInHierarchy == true) 
        {
            InventoryMenu.gameObject.SetActive(false);
        }
        
        
    }

    public void toggleInventory() // Simple state swap
    {
        if (iState == InterfaceState.def) 
        {
            iState = InterfaceState.inv;
        }
        else if (iState == InterfaceState.inv) 
        {
            iState = InterfaceState.def;
        }
        //More here when more UI states exist
    }

    void drawInventoryItems() 
    {
        int iterator = 0;
        foreach ( Image i in inventoryUISlots ) 
        {
            if (iterator < IM.inventory.Count) 
            {
                if (IM.inventory[iterator].isEmpty == false)
                {
                    i.gameObject.SetActive(true);
                    i.sprite = IM.inventory[iterator].item.itemIcon;
                    Text t = i.GetComponentInChildren<Text>(true);
                    if (t != null) 
                    {
                        t.text = IM.inventory[iterator].stack.ToString();
                    }
                    
                }
                else 
                {
                    i.gameObject.SetActive(false);
                }
            
            }
            iterator++;
        }
    
    }

    void initializeInventoryObjects() 
    {
        if (InventoryPanel == null) { return; }
        inventoryUISlots = InventoryPanel.GetComponentsInChildren<Image>(true);
    }

    void initializeHotBarObjects() 
    {
        if (HotBarPanel == null) { return; }
    
    }

    void initializeCrafting() 
    {
        int iterator = 0;
        //CraftingSelectors[0].onClick.AddListener(CraftClick(0));
        foreach (Button b in CraftingSelectors) 
        {
            if (iterator + craftingScroll < CraftingRecipe.recipes.Count)
            {
                b.gameObject.SetActive(true);
                Text t = b.gameObject.GetComponentInChildren<Text>();
                t.text = CraftingRecipe.recipes[iterator + craftingScroll].toString();
                Image [] i = b.GetComponentsInChildren<Image>();
                i[1].sprite = CraftingRecipe.recipes[iterator + craftingScroll].resultItem.item.itemIcon;
            }
            else 
            {
                b.gameObject.SetActive(false);
            }
            iterator++;
        }
    }

    public void CraftClick(int num) 
    {
        //Todo figure out what number corresponds to what recipie
        int id = num + craftingScroll; // this is which one we've selected

        CraftingRecipe cr;
        if ( id > CraftingRecipe.recipes.Count ) 
        {
            Debug.Log("Invalid Crafting Recipe accessed");
            return;  
        }
        cr = CraftingRecipe.recipes[id];
        IM.CraftItem(cr);


        //IM.CraftItem( temp );

    }

}
