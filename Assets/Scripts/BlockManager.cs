using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    public ArrayList blocks = new ArrayList();
    public ArrayList map = new ArrayList();

    Color r9 = new Color(229, 0, 26);
    Color r8 = new Color(204, 0, 51);
    Color r7 = new Color(178, 0, 77);
    Color r6 = new Color(153, 0, 102);
    Color r5 = new Color(127, 0, 127);
    Color r4 = new Color(102, 0, 153);
    Color r3 = new Color(77, 0, 178);
    Color r2 = new Color(51, 0, 204);
    Color r1 = new Color(26, 0, 229);

    public AudioSource audioSource;

    public Sprite[] colorsprites;

    int maxsize = 250;
    

    public InventoryManager IM;

    public bool debugBlock = false;
    public bool debugGeneration = false;
    public bool debugTool = false;

    bool isAttacking = false;
    float attackDuration = 0;
    float attackLag = 0;


    public Sprite[] blockSprites;

    public Sprite debugTexture;
    public Sprite selectTexture;
    private int blockSize = 32;

    GameObject selector;
    public GameObject gameCamera;

    public GameObject player;
    public GameObject plyWeapon;
    Item usedWeap;

    private BlockData debug;

    bool hoveringBlock = false;
    public BlockInstance hoveredBlock;
    public BlockInstance miningBlock;

    public float miningTime;

    public MapGen mapGen;

    OnTriggerEnterEvent selectEvent;

    void Awake() // Called at very start of game
    {
        if (debugTexture == null)
        {
            Debug.Log("No Debuggging Texture Loaded");
        }
        debug = new BlockData("Dirt", blockSprites[0], 0, 1f, ItemType.shovel);

        blocks.Add(debug);
        blocks.Add(new BlockData("Grass", blockSprites[1],0,1f, ItemType.shovel));
        blocks.Add(new BlockData("Cobblestone", blockSprites[2]));
        blocks.Add(new BlockData("Stone", blockSprites[3],1,1.5f,ItemType.pickaxe));
        blocks.Add(new BlockData("Iron Ore", blockSprites[4],2,3f,ItemType.pickaxe));
        blocks.Add(new BlockData("Gold Ore", blockSprites[5], 2, 3f, ItemType.pickaxe));
        blocks.Add(new BlockData("Diamond Ore", blockSprites[6], 2, 3f, ItemType.pickaxe));
        blocks.Add(new BlockData("Copper Ore", blockSprites[7], 2, 3f, ItemType.pickaxe));
        blocks.Add(new BlockData("Wood Log", blockSprites[8], 0, 0.8f, ItemType.axe, BlockType.background));
        blocks.Add(new BlockData("Wood Plank", blockSprites[9], 0, 0.8f, ItemType.axe));

        Item stick = new Item("Stick", ItemType.item, blockSprites[10]);
        Item sword = new Item("Wooden Sword", ItemType.weapon, blockSprites[11]);
        Item iingot = new Item("Iron Ingot", ItemType.item, blockSprites[12]);
        Item pick = new Item("Pickaxe", ItemType.pickaxe, blockSprites[14]);
        Item shovel = new Item("Shovel", ItemType.shovel, blockSprites[13]);

        new CraftingRecipe(new InventoryItem(stick, 4), new InventoryItem(Item.GetItemByName("Wood Plank"), 1));
        new CraftingRecipe(new InventoryItem(sword, 1), new InventoryItem(Item.GetItemByName("Wood Plank"), 2));
        new CraftingRecipe(new InventoryItem(iingot, 1), new InventoryItem(Item.GetItemByName("Iron Ore"), 1));

        

    }

    // Start is called before the first frame update
    void Start()
    {

        //blocksToItems();
        /*
        for (int i = -10; i < 10; i++) 
        {
            for (int y = -10; y < 1; y++) 
            {
                map.Add(new BlockInstance(debug, i, y));
            }
        }
        */
        selector = CreateSelector();

        //HeatMap();

        mapGen.GenerateMap();
        map = mapGen.map;
        CreateMap();

        miningTime = 0;

    }


    void CreateMap()
    {
        Vector3 myScale = new Vector3(4, 4, 1);
        int iterator = 0;
        foreach (BlockInstance b in map)
        {
            //Creating the block object
            GameObject block = new GameObject("block" + iterator + b.blockData.blockSprite);

            block.tag = "Block";

            b.GO = block;

            SpriteRenderer sr = block.AddComponent<SpriteRenderer>();

            if (b.blockData.blockType == BlockType.solid) 
            {
                BoxCollider2D bc = block.AddComponent<BoxCollider2D>();
                bc.size = new Vector2(.32f, .32f);
            }


            //Making sure we have a sprite to render
            if (b.blockData.blockSprite != null)
            {
                sr.sprite = b.blockData.blockSprite;
            }
            else
            {
                sr.sprite = debugTexture;
                Debug.LogError(b.blockData.blockName + "Lacks a sprite");
            }

            //Debug.Log("X" + b.x + " Y " + b.y);


            

            //Debug.Log("Creating block " + iterator + " at x " + b.x + " " + (b.x * blockSize) );

            block.transform.position = new Vector3((((float)b.x * ((float)blockSize / 100f)) * 4), (((float)b.y * ((float)blockSize / 100f)) * 4), 0);
            block.transform.localScale = myScale;
            //Making the block a child of the block manager
            block.transform.SetParent(this.transform);
            iterator++;
        }
    }

    GameObject CreateSelector()
    {
        GameObject block = new GameObject("selector");
        SpriteRenderer sr = block.AddComponent<SpriteRenderer>();
        BoxCollider2D bc = block.AddComponent<BoxCollider2D>();
        sr.sprite = selectTexture;


        block.transform.position = new Vector3(0, 0, 0);
        block.transform.localScale = new Vector3(4, 4, 1);
        block.transform.SetParent(this.transform);
        bc.size = new Vector2(.32f, .32f);
        bc.isTrigger = true;


        return block;

    }

    void OnDrawGizmos()
    {
        //Drawing our grid
        /*
        Gizmos.color = Color.white;
        for (int x = -100; x < 100; x++) 
        {
            for (int y = -100; y < 100; y++) 
            {
                
                Gizmos.DrawWireCube(new Vector3(transform.position.x + 1.28f * x, transform.position.y + 1.28f * y, -1), new Vector3(1.28f * x, 1.28f * y, 0)) ;
            }
        }
        */
    }

    void moveSelector()
    {
        Vector3 mousePos = Input.mousePosition;

        //Getting the Camera's leftmost position for easier math
        float cameraX = gameCamera.transform.position.x - ((Screen.width) / 100);
        float cameraY = gameCamera.transform.position.y - ((Screen.height) / 100);

        Vector3 pos = gameCamera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 10));
        //float myX = pos.x;
        //float myY = pos.y;
        pos = new Vector3((float)Mathf.Floor(pos.x / ((blockSize / 100f) * 4) + .5f) * 1.28f, (float)Mathf.Floor(pos.y / ((blockSize / 100f) * 4) + .5f) * 1.28f, -10);
        //float posx = MathF.floor( pos.x / ((blockSize / 100f) * 4) ) * 1.28;

        float y = ((cameraY));
        float x = ((cameraX));

        //Debug.Log(pos) ;
        int thisX = (int)x;
        int thisY = (int)y;

        selector.transform.position = pos;

    }

    void checkPos()
    {
        int selectx = (int)Mathf.Floor((selector.transform.position.x / 1.28f) + .5f);
        int selecty = (int)Mathf.Floor((selector.transform.position.y / 1.28f) + .5f);

        foreach (BlockInstance BI in map)
        {
            if (BI.x == selectx)
            {
                if (BI.y == selecty)
                {
                    hoveringBlock = true;
                    hoveredBlock = BI;
                    //Debug.Log("Found it X" + selectx + "=" + BI.x + "Y" + selecty + "=" + BI.y);
                    return;
                }
            }
            else
            {
                //Debug.Log("This X" + BI.x + "Isnt equal to " + ((selector.transform.position.x * 100) / 128));
            }
        }
        //Debug.Log("Couldnt find block x" + selectx + "y" + selecty);
        hoveringBlock = false;

    }

    public BlockInstance findBlock(int x, int y)
    {
        foreach (BlockInstance BI in map)
        {
            if (BI.x == x)
            {
                if (BI.y == y)
                {
                    return BI;
                }
            }
        }
        return new BlockInstance();
    }


    void DeleteBlock(BlockInstance bi)
    {
        Destroy(bi.GO);
        map.Remove(bi);
    }

    void BuildBlock(BlockData bd)
    {
        int selectx = (int)Mathf.Floor((selector.transform.position.x / 1.28f) + .5f);
        int selecty = (int)Mathf.Floor((selector.transform.position.y / 1.28f) + .5f);

        BlockInstance bi = new BlockInstance(debug, (int)selectx, (int)selecty);
        GameObject go = new GameObject("block X" + selectx + "Y" + selecty + bd.blockSprite);
        go.tag = "Block";

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        if (bd.blockType == BlockType.solid)
        {
            BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
            bc.size = new Vector2(.32f, .32f);
        }
        //Making sure we have a sprite to render
        if (bd.blockSprite != null)
        {
            sr.sprite = bd.blockSprite;
        }
        else
        {
            sr.sprite = debugTexture;
            Debug.Log(bd.blockName + "Lacks a sprite");
        }

        

        go.transform.position = new Vector3((((float)selectx * ((float)blockSize / 100f)) * 4), selecty * (((float)blockSize / 100f) * 4), 0);
        go.transform.localScale = new Vector3(4, 4, 1);
        go.transform.SetParent(this.transform);

        bi.GO = go;
        map.Add(bi);
    }

    void destroyBlock() //This funciton is differentiated for later when we slowly progressively destroy blocks
    {
        checkPos();

        //hoveredBlock = new BlockInstance(debug,0,0);
        //hoveredBlock.GO = GameObject.Find("block1debug (UnityEngine.Sprite)");
        Item myItem = IM.getSelectedItem();
        if (hoveringBlock)
        {
            if (debugBlock)
            {
                Debug.Log(hoveredBlock);
                Debug.Log(hoveredBlock.GO);
            }
            if (hoveredBlock != miningBlock)
            {
                miningBlock = hoveredBlock;
                miningTime = 0;
            }
            //Debug.Log($"Mining without tool");

            if (miningBlock.blockData.minableItem == ItemType.tool || miningBlock.blockData.minableItem == myItem.itemType)
            {
                if (miningTime >= miningBlock.blockData.blockDurability)
                {
                    DeleteBlock(hoveredBlock);
                    miningTime = 0;
                }
            }
            else 
            {
                if ((miningTime) >= miningBlock.blockData.blockDurability * 2)
                {
                    DeleteBlock(hoveredBlock);
                    miningTime = 0;
                }
            }
            miningTime += Time.deltaTime;

            
        }
    }

    void destroyBlock(Item toolItem) //This funciton is differentiated for later when we slowly progressively destroy blocks
    {
        checkPos();

        //hoveredBlock = new BlockInstance(debug,0,0);
        //hoveredBlock.GO = GameObject.Find("block1debug (UnityEngine.Sprite)");
        if (toolItem.miningMultiplier == 0 || toolItem.miningMultiplier == null)
        {
            destroyBlock();
            return;
        }
        

        if (hoveringBlock)
        {
            if (debugBlock)
            {
                Debug.Log(hoveredBlock);
                Debug.Log(hoveredBlock.GO);
            }

            if (hoveredBlock != miningBlock) 
            {
                miningBlock = hoveredBlock;
                miningTime = 0;
            }

            miningTime += Time.deltaTime;


            if (miningBlock.blockData.minableItem == ItemType.tool || miningBlock.blockData.minableItem == toolItem.itemType)
            {
                if (miningTime >= miningBlock.blockData.blockDurability)
                {
                    DeleteBlock(hoveredBlock);
                    miningTime = 0;
                }
            }
            else 
            {
                if ( ( miningTime) >= miningBlock.blockData.blockDurability * 2)
                {
                    DeleteBlock(hoveredBlock);
                    miningTime = 0;
                }

            }
            

            
        }


    }

    void StartAttack(Item weapon)
    {
        Vector3 mousePos = gameCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 playerPos = player.transform.position;

        plyWeapon.SetActive(true);
        isAttacking = true;
        attackDuration = weapon.attackDuration;
        attackLag = weapon.swingLag;

        float dist = .1f;
        //float newRot = ((mousePos.y - playerPos.y) / (mousePos.x - playerPos.x));
        float posX =  (mousePos.x - playerPos.x ) / (Mathf.Abs(mousePos.x - playerPos.x) );
        float posY = (mousePos.y -playerPos.y) / ( Mathf.Abs(mousePos.y - playerPos.y));

        Vector3 test = player.transform.InverseTransformPoint(mousePos);
        test = player.transform.TransformPoint(test.normalized * dist);


        Vector3 coolPos = new Vector3(posX, posY, plyWeapon.transform.position.z);

        if (float.IsNaN(coolPos.y)) 
        {
            return;
        }
        if (float.IsNaN(coolPos.x))
        {
            return;
        }

        coolPos = new Vector3(coolPos.x * dist, coolPos.y * dist, coolPos.z); 

        plyWeapon.transform.position = test;// setting the position

        mousePos.x = mousePos.x - plyWeapon.transform.position.x;
        mousePos.y = mousePos.y - plyWeapon.transform.position.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        angle -= 90;

        plyWeapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        usedWeap = IM.getSelectedItem();


    }
    void attacking() 
    {
        if (attackDuration >= 0)
        {
            attackDuration -= Time.deltaTime;

            BoxCollider2D swordHitbox = plyWeapon.GetComponent<BoxCollider2D>();

            Collider2D[] results = new Collider2D[4];

            ContactFilter2D testFilter = new ContactFilter2D();

            int colisions = swordHitbox.OverlapCollider(testFilter.NoFilter(), results);

            for (int i = 0; i < colisions; i++) 
            {
                GameObject collided = results[i].gameObject;
                //Debug.Log(collided);
                Enemy n = collided.GetComponent(typeof(Enemy)) as Enemy;
                if (n != null) 
                {
                    Debug.Log($"Damaging {n}");
                    n.Damage(usedWeap.weapDamage);
                }
            }


        }
        else if (attackLag > 0)
        {
            plyWeapon.SetActive(false);
            attackLag -= Time.deltaTime;
        }
        else 
        {
            Debug.Log("Finished attack");
            isAttacking = false;
        }
        
    }

    void useBlock(Item blockItem) 
    {
        if (blockItem.thisBlock == null) 
        {
            Debug.Log("Couldn't find blockdata in item");
            return;
        }
        useBlock(blockItem.thisBlock);
       
    }

    void useBlock(BlockData bd) 
    {
        checkPos(); // checking if we can build
        //hoveredBlock = new BlockInstance(debug,0,0);
        //hoveredBlock.GO = GameObject.Find("block1debug (UnityEngine.Sprite)");
        if (!hoveringBlock)
        {
            BuildBlock(bd);
            //TODO subtract item
        }
    }

    float dt = 0;

    // Update is called once per frame
    void Update()
    {

        moveSelector();

        if ( isAttacking ) 
        {
            attacking();
        }
        else 
        {
            if (Input.GetButton("Fire1")) //Leftclicking
            {
                Item selected = IM.getSelectedItem();

                switch (selected.itemType) // Check what we're doing
                {
                    case ItemType.block: destroyBlock(); break;
                    case ItemType.item: destroyBlock(); break;
                    case ItemType.none: destroyBlock(); break;
                    case ItemType.tool: destroyBlock(selected); break;
                    case ItemType.pickaxe: destroyBlock(selected); break;
                    case ItemType.axe: destroyBlock(selected); break;
                    case ItemType.shovel: destroyBlock(selected); break;
                    case ItemType.weapon: StartAttack(selected); break;
                    case ItemType.sword: StartAttack(selected); break;
                    default: Debug.Log("Left click switch statement didnt work!"); break;

                }
            }

            if (Input.GetButton("Fire2")) //Right clicking
            {
                Item selected = IM.getSelectedItem();

                switch (selected.itemType) // Check what we're doing
                {
                    case ItemType.block: useBlock(selected); break;
                    /*case ItemType.item: destroyBlock(); break;
                    case ItemType.none: destroyBlock(); break;
                    case ItemType.tool: destroyBlock(selected); break;
                    case ItemType.pickaxe: destroyBlock(selected); break;
                    case ItemType.axe: destroyBlock(selected); break;
                    case ItemType.shovel: destroyBlock(selected); break;
                    case ItemType.weapon: attack(selected); break;
                    case ItemType.sword: attack(selected); break;*/
                    default: Debug.Log("Right click switch statement didnt find anything to do!"); break;

                }
            }
        }
    }

}
