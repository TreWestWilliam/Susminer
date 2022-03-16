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

    public Sprite[] colorsprites;

    int maxsize = 250;
    public int[,] heatmap = new int[300, 300];

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
    private short blockSize = 32;

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
        for (short i = -10; i < 10; i++) 
        {
            for (short y = -10; y < 1; y++) 
            {
                map.Add(new BlockInstance(debug, i, y));
            }
        }
        */
        selector = CreateSelector();

        //HeatMap();

        GenerateMap();
        CreateMap();

        miningTime = 0;

    }

    int mapWidth() 
    {
        int w = 0;
        foreach (BlockInstance b in map) 
        {
            if (b.x > w) 
            {
                w = b.x;
            }
            
        }
        Debug.Log($"Width:{w}");
        return w+1;
        
    }
    int mapHeight()
    {
        int h = 0;
        int l = 0;
        foreach (BlockInstance b in map)
        {
            if (b.y > h)
            {
                h = b.y;
            }
            if (b.y < l) 
            {
                l = b.y;
            }
            
        }
        int th = h + (-l);
        Debug.Log($"Height:{th}");
        return th+1;
    }

    async void HeatMap() 
    {
        int iterations = 200;
        int iiterations = iterations;
        
        for (int x = 0; x < heatmap.GetUpperBound(0); x++) 
        {
            for (int y = 0; y < heatmap.GetUpperBound(1); y++) 
            {
                heatmap[x, y] = 0;
            }
        }
        Debug.Log(" going forward");

        while (iiterations > 0)
        {
            GenerateMapTest();

            int mw = mapWidth();
            int mh = mapHeight();
            Debug.Log("Reached Map Check"); 
            if (mw > heatmap.GetUpperBound(0))
            {
                if (mh > heatmap.GetUpperBound(1))
                {
                    Debug.Log("Resizing both");
                    int[,] newMap = new int[mw, mh];
                    for (int x = 0; x < newMap.GetUpperBound(0); x++)
                    {
                        for (int y = 0; y < newMap.GetUpperBound(1); y++)
                        {
                            if (x < heatmap.GetUpperBound(0) && y < heatmap.GetUpperBound(1))
                            {
                                newMap[x, y] = heatmap[x, y];
                            }
                            else
                            {
                                newMap[x, y] = 0;
                            }
                        }
                    }
                    heatmap = newMap;
                }
                else
                {
                    Debug.Log("Resizing X");
                    int[,] newMap = new int[mw, heatmap.GetUpperBound(1)];
                    for (int x = 0; x < newMap.GetUpperBound(0); x++)
                    {
                        for (int y = 0; y < newMap.GetUpperBound(1); y++)
                        {
                            if (x < heatmap.GetUpperBound(0) && y < heatmap.GetUpperBound(1))
                            {
                                newMap[x, y] = heatmap[x, y];
                            }
                            else
                            {
                                newMap[x, y] = 0;
                            }
                        }
                    }
                    heatmap = newMap;
                    Debug.Log(heatmap.GetUpperBound(0));
                }

            }
            else if (mh > heatmap.GetUpperBound(1))
            {
                Debug.Log("Resizing Y");
                int[,] newMap = new int[heatmap.GetUpperBound(0), mh];
                for (int x = 0; x < newMap.GetUpperBound(0); x++)
                {
                    for (int y = 0; y < newMap.GetUpperBound(1); y++)
                    {
                        if (x < heatmap.GetUpperBound(0) && y < heatmap.GetUpperBound(1))
                        {
                            newMap[x, y] = heatmap[x, y];
                        }
                        else
                        {
                            newMap[x, y] = 0;
                        }
                    }
                }
                heatmap = newMap;
            }
            
            foreach (BlockInstance b in map)
            {
                heatmap[(int)b.x, (int)b.y]++;
            }
            while (map.Count > 0) 
            {
                map.Clear();
            }

            iiterations--;
        }
        //Debug.Log("making filler blocks");

        int max = 0;
        /*
        for (int x = 0; x < heatmap.GetUpperBound(0); x++)
        {
            for (int y = 0; y < heatmap.GetUpperBound(1); y++)
            {
                map.Add(new BlockInstance((BlockData)blocks[3] , (short)x, (short)y));
            }
        }
        */

        int iterator = 0;
        Vector3 myScale = new Vector3(4, 4, 1);
        Debug.Log("Drawing Blocks");

        int sanity = 0;
        Texture2D newText = new Texture2D(heatmap.GetUpperBound(0), heatmap.GetUpperBound(1) );

        for (int x = 0; x < heatmap.GetUpperBound(0); x++)//
        {
            for (int y = 0; y < heatmap.GetUpperBound(1); y++)
            {
                float ratio = ( (float) heatmap[x, y] ) / iterations;

                if (max < heatmap[x, y]) 
                {
                    max = heatmap[x, y];
                }

                if (heatmap[x, y] != 0 && heatmap[x, y] != max) 
                {
                    sanity++;
                }

                float red = ratio;
                float blue = 1 - red;
                Color myColor = new Color(red, 0, blue, 1f);
                newText.SetPixel(x, y, myColor);



                /*
                GameObject block = new GameObject("block" + iterator + "R:" + ratio);
                SpriteRenderer sr = block.AddComponent<SpriteRenderer>();
                BoxCollider2D bc = block.AddComponent<BoxCollider2D>();

                sr.sprite = blockSprites[3];

                switch (ratio) 
                {
                    case 1: sr.sprite = colorsprites[10]; break;
                    case 0.9f: sr.sprite = colorsprites[9]; ; break;
                    case 0.8f: sr.sprite = colorsprites[8]; break;
                    case 0.7f: sr.sprite = colorsprites[7]; break;
                    case 0.6f: sr.sprite = colorsprites[6]; break;
                    case 0.5f: sr.sprite = colorsprites[5]; break;
                    case 0.4f: sr.sprite = colorsprites[4]; break;
                    case 0.3f: sr.sprite = colorsprites[3]; break;
                    case 0.2f: sr.sprite = colorsprites[2]; break;
                    case 0.1f: sr.sprite = colorsprites[1]; break;
                    case 0: sr.sprite = colorsprites[1]; break;
                    //default: int re = (int)(((float)255) * ratio); int bl = (int)(255 - (((float)255) * ratio)); Color myColor = new Color(re, 0, bl, 255); sr.color = myColor;break;

                }

                bc.size = new Vector2(.32f, .32f);

                block.transform.position = new Vector3((((float)x * ((float)blockSize / 100f)) * 4), (((float)y * ((float)blockSize / 100f)) * 4), 0);
                block.transform.localScale = myScale;
                //Making the block a child of the block manager
                block.transform.SetParent(this.transform);
                */
                iterator++;
            }
        }

        newText.Apply();

        byte[] bytes = newText.EncodeToPNG();

        File.WriteAllBytes(Application.dataPath + "/../heatMap.png", bytes);

        Sprite mySprite = Sprite.Create(newText, new Rect(0.0f, 0.0f, newText.width, newText.height), new Vector2(0.5f, 0.5f), .32f);
        GameObject block = new GameObject("heatmap");
        SpriteRenderer sr = block.AddComponent<SpriteRenderer>();
        //BoxCollider2D bc = block.AddComponent<BoxCollider2D>();
        sr.sprite = mySprite;
        block.transform.position = new Vector3(0f, 0f, 0f);
        block.transform.SetParent(this.transform);


        Debug.Log($"Highest block: {max} Iterations: {iterations}");


    }

    async void GenerateMapTest()
    {
        Random.InitState((int) (Random.value *1000));

        if (debugGeneration)
        {
            Debug.Log("Generating Map");
        }

        Vector2 center = new Vector2(0, 0);
        BlockData grass = (BlockData)blocks[1];
        BlockData dirt = (BlockData)blocks[0];
        BlockData stone = (BlockData)blocks[3];
        BlockData ironOre = (BlockData)blocks[4];
        BlockData log = (BlockData)blocks[8];

        int maxWidth = 200;
        int maxHeight = 60;

        int minWidth = 40;
        int minHeight = 20;

        int heightDiff = maxHeight - minHeight;
        int widthDiff = maxWidth - minWidth;

        int height = ((int)(Random.value * heightDiff) + minHeight);
        int width = ((int)(Random.value * widthDiff) + minWidth);

        int coreWidth = (int)(width / 3);
        int coreHeight = (int)(height);
        int baseHeight = 2;

        int prevHeightVar = (int)(Random.value * 4);
        int curHeight = baseHeight;

        int maxPosYVar = 5;
        int maxNegYVar = 5;

        int posYVar = 1;
        int negYVar = 1;

        short highestPoint = 0;
        short lowestPoint = 0;
        if (debugGeneration)
        {
            Debug.Log($"Height: {height}, Width: {width} ");
        }

        int offset = width / 2;

        for (int x = -(width / 2); x < width / 2; x++)
        {
            int calcX = x + (width / 2);
            int coreHeightDiff = coreHeight - curHeight;
            int coreWidthDiff = coreWidth - calcX;
            int coreWidthDiffLate = (coreWidth * 2);
            float dist = 0;
            if (coreHeightDiff > 0 && coreWidthDiff > 0)
            {
                dist = (coreHeightDiff / coreWidthDiff) * 4;
                if (debugGeneration)
                    Debug.Log($" coreWidthDiff: {coreWidthDiff} , coreHeightDiff: {coreHeightDiff} DIST: {dist}");
            }
            else
            {
                dist = ((-curHeight / (width - calcX)) * 3);
            }

            if (coreWidthDiff == 0)
            {
                if (debugGeneration)
                    Debug.Log($"Core Width Diff equals zero");
                coreWidthDiff = 1;
            }

            if (calcX < coreWidth)
            {
                if (curHeight < coreHeight)
                {
                    int temp = (int)(Random.value * (dist));
                    curHeight += temp;
                    if (debugGeneration)
                        Debug.Log($"CurHeight added to by {temp}");
                }
                else
                {
                    curHeight -= (int)(Random.value * 3);
                }
            }
            else if (calcX >= coreWidth && calcX < (coreWidth * 2))
            {
                curHeight = coreHeight;
                if (debugGeneration)
                    Debug.Log("Is Centered");
            }
            else
            {
                if (debugGeneration)
                {
                    Debug.Log($"CurHeight added to by {dist}");
                }
                curHeight += (int)(Random.value * (dist));
            }

            if (curHeight <= 0) { curHeight = 1; }

            if (posYVar < maxPosYVar)
            {
                int coinflip = (int)(Random.value * 5);
                if (coinflip > 0 && coinflip <= 2)
                {
                    posYVar += (int)(Random.value * 2);
                }
                else if (coinflip == 0)
                {
                    posYVar -= (int)(Random.value * 2);
                }
            }
            else
            {
                posYVar -= (int)(Random.value * 2);
            }

            if (posYVar < 0) { posYVar = 2; }

            if (negYVar < maxNegYVar)
            {
                int coinflip = (int)(Random.value * 2);
                if (coinflip > 0)
                {
                    negYVar += (int)(Random.value * 4);
                }
                else
                {
                    negYVar -= (int)(Random.value * 2);
                }
            }
            else
            {
                negYVar -= (int)(Random.value * 4);
            }

            if (negYVar <= 0) { negYVar = 1; }

            int startHeight = posYVar;
            if (startHeight > highestPoint)
            {
                highestPoint = (short)startHeight;
            }
            int endHeight = (curHeight + negYVar) * -1;

            //Tree Generator, basic edition
            if (((int)(Random.value * 10)) == 5)
            {
                int treeHeight = (int)(Random.value * 10) + 2;
                for (short t = 1; t < treeHeight; t++)
                {
                    map.Add(new BlockInstance(log, (short)x, (short)(startHeight + t)));
                }
            }
            int dirtVariation = (int)(Random.value * 3);

            for (int y = startHeight; y > endHeight; y--)
            {
                if (y == startHeight)
                {
                    map.Add(new BlockInstance(grass, (short)x, (short)y));
                }
                else if (y < startHeight && y >= startHeight - 2 - dirtVariation)
                {
                    map.Add(new BlockInstance(dirt, (short)x, (short)y));
                }
                else
                {
                    map.Add(new BlockInstance(stone, (short)x, (short)y));
                }
            }

        if (endHeight < lowestPoint) { lowestPoint = (short) endHeight; }
            if (debugGeneration)
            {
                Debug.Log($"StartHeight: {startHeight} End Height: {endHeight}");
            }
            for (int y = startHeight; y > endHeight; y--)
            {
                if (y == startHeight)
                {
                    map.Add(new BlockInstance(grass, (short) (x), (short) (y) ));
                }
                else if (y < startHeight && y >= startHeight - 2)
                {
                    map.Add(new BlockInstance(dirt, (short)(x),   (short) (y) ));
                }
                else
                {
                    map.Add(new BlockInstance(stone, (short)(x), (short) ( y) ));
                }
            }
        }



        //Adding Ore Clusters
        int clusterSizeMin = 4;
        int clusterSizeMax = 16;

        int maxClusters = 24;
        int minClusters = 4;

        int clusters = minClusters + ((int)(Random.value * (maxClusters - minClusters)));
        if (debugGeneration)
        {
            Debug.Log($"Clusters to be spawned: {clusters}");
        }

        for (int i = 0; i < clusters; i++)
        {

            int clusterSize = clusterSizeMin + ((int)(Random.value * (clusterSizeMax - clusterSizeMin)));
            Vector2[] cPos = new Vector2[clusterSize];

            int clusterX = 0;
            int clusterY = 0;
            cPos[0] = new Vector2(0, 0);

            for (int ii = 1; ii < clusterSize; ii++)
            {
                int die = (int)(Random.value * 4);
                if (die == 1) //drawing a square for remainder on one
                {
                    int diff = clusterSize - i;
                    int count = 0;
                    if (diff % 2 == 0)
                    {
                        for (int x = 0; x < diff / 2; x++)
                        {
                            for (int y = 0; y < diff / 2; y++)
                            {
                                if (ii + count >= clusterSize) { break; }
                                cPos[ii + count] =
                                    new Vector2(cPos[ii - 1].x +
                                    x, cPos[ii - 1].y - y);
                                count++;
                            }
                        }
                        break;
                    }
                    else
                    {
                        for (int x = 0; x < (diff / 2) - 1; x++)
                        {
                            for (int y = 0; y < diff / 2; y++)
                            {
                                if (ii + count >= clusterSize) { break; }
                                cPos[ii + count] = new Vector2(cPos[ii - 1].x + x, cPos[ii - 1].y - y);
                            }
                        }
                        break;
                    }
                }
                else if (die == 2) // going down
                {
                    cPos[ii] = new Vector2(cPos[ii].x, cPos[ii].y - 1);
                }
                else if (die == 3) // going down
                {
                    cPos[ii] = new Vector2(cPos[ii].x + 1, cPos[ii].y);
                }
            }

            BlockInstance start = (BlockInstance)map[(int)(Random.value * map.Count)];
            while (start.blockData != stone)
            {
                start = (BlockInstance)map[(int)(Random.value * map.Count)];
            }

            short startX = start.x;
            short startY = start.y;

            for (short s = 0; s < clusterSize; s++)
            {
                BlockInstance block = findBlock(startX + (short)cPos[s].x, startY + (short)cPos[s].y);
                block.blockData = ironOre;
            }
        }
        //Lowering the map to not stick the player in it (probably should be temporary;
        Debug.Log($"Lowest Point{lowestPoint}");
        foreach (BlockInstance b in map)
        {
            //b.y += (short) -lowestPoint;
            //b.x += (short) offset;
            b.y += (short) 150;
            b.x += (short) 150;
        }
    }

    async void GenerateMap()
    {
        if (debugGeneration) 
        {
            Debug.Log("Generating Map");
        }
        
        Vector2 center = new Vector2(0, 0);
        BlockData grass = (BlockData)blocks[1];
        BlockData dirt = (BlockData)blocks[0];
        BlockData stone = (BlockData)blocks[3];
        BlockData ironOre = (BlockData)blocks[4];
        BlockData log = (BlockData)blocks[8];

        int maxWidth = 200;
        int maxHeight = 60;

        int minWidth = 40;
        int minHeight = 20;

        int heightDiff = maxHeight - minHeight;
        int widthDiff = maxWidth - minWidth;

        int height = ((int)(Random.value * heightDiff) + minHeight);
        int width = ((int)(Random.value * widthDiff) + minWidth);

        int coreWidth = (int)(width / 3);
        int coreHeight = (int)(height);
        int baseHeight = 2;

        int prevHeightVar = (int)(Random.value * 4);
        int curHeight = baseHeight;

        int maxPosYVar = 5;
        int maxNegYVar = 5;

        int posYVar = 1;
        int negYVar = 1;

        short highestPoint = 0;
        if (debugGeneration)
        {
            Debug.Log($"Height: {height}, Width: {width} ");
        }

        for (int x = (width / 2) * -1; x < width / 2; x++)
        {
            int calcX = x + width / 2;
            int coreHeightDiff = coreHeight - curHeight;
            int coreWidthDiff = coreWidth - calcX;
            int coreWidthDiffLate = (coreWidth * 2);
            float dist = 0;
            if (coreHeightDiff > 0 && coreWidthDiff > 0)
            {
                dist = (coreHeightDiff / coreWidthDiff) * 4;
                if (debugGeneration)
                    Debug.Log($" coreWidthDiff: {coreWidthDiff} , coreHeightDiff: {coreHeightDiff} DIST: {dist}");
            }
            else
            {
                dist = ((0 - curHeight) / (width - calcX) * 2);
            }

            if (coreWidthDiff == 0)
            {
                if (debugGeneration)
                    Debug.Log($"Core Width Diff equals zero");
                coreWidthDiff = 1;
            }

            if (calcX < coreWidth)
            {
                if (curHeight < coreHeight)
                {
                    int temp = (int)(Random.value * (dist));
                    curHeight += temp;
                    if (debugGeneration)
                        Debug.Log($"CurHeight added to by {temp}");
                }
                else
                {
                    curHeight -= (int)(Random.value * 3);
                }
            } else if (calcX >= coreWidth && calcX < (coreWidth * 2))
            {
                curHeight = coreHeight;
                if (debugGeneration) 
                    Debug.Log("Is Centered");
            }
            else
            {
                if (debugGeneration)
                {
                    Debug.Log($"CurHeight added to by {dist}");
                }
                curHeight += (int)(Random.value * (dist));
            }

            if (curHeight <= 0) { curHeight = 1; }

            if (posYVar < maxPosYVar)
            {
                int coinflip = (int)(Random.value * 5);
                if (coinflip > 0 && coinflip <= 2)
                {
                    posYVar += (int)(Random.value * 2);
                }
                else if (coinflip == 0)
                {
                    posYVar -= (int)(Random.value * 2);
                }
            }
            else
            {
                posYVar -= (int)(Random.value * 2);
            }

            if (posYVar < 0) { posYVar = 2; }

            if (negYVar < maxNegYVar)
            {
                int coinflip = (int)(Random.value * 2);
                if (coinflip > 0)
                {
                    negYVar += (int)(Random.value * 4);
                }
                else
                {
                    negYVar -= (int)(Random.value * 2);
                }
            }
            else
            {
                negYVar -= (int)(Random.value * 4);
            }

            if (negYVar <= 0) { negYVar = 1; }

            int startHeight = posYVar;
            if (startHeight > highestPoint) 
            {
                highestPoint = (short) startHeight;
            }
            int endHeight = (curHeight + negYVar) * -1;
            if (debugGeneration)
            {
                Debug.Log($"StartHeight: {startHeight} End Height: {endHeight}");
            }

            int dirtVariation = (int) (Random.value * 3);

            //Tree Generator, basic edition
            if (((int)(Random.value * 10)) == 5)
            {
                int treeHeight = (int)(Random.value * 10) + 2;
                for (short t = 1; t < treeHeight; t++)
                {
                    map.Add(new BlockInstance(log, (short)x, (short)(startHeight + t)));
                }
            }

            for (int y = startHeight; y > endHeight; y--)
            {
                if (y == startHeight)
                {
                    map.Add(new BlockInstance(grass, (short)x, (short)y));
                }
                else if (y < startHeight && y >= startHeight - 2 - dirtVariation)
                {
                    map.Add(new BlockInstance(dirt, (short)x, (short)y));
                }
                else
                {
                    map.Add(new BlockInstance(stone, (short)x, (short)y));
                }
            }
        }

        // Carve Caves
        int caves = (int) ( Random.value * 4 ) +1;
        Debug.Log("Making caves " + caves);
        for (int i = 0; i <caves;i++) 
        {
            short seedpointy = (short) -(Random.value * height);
            short seedpointx =  (short) ( (Random.value * width) - (width /2) );

            Debug.Log($"Seedpoint X:{seedpointx} Y:{seedpointy}");
            BlockInstance seed = findBlock(seedpointx, seedpointy);
            if (seed == new BlockInstance()) 
            {
                Debug.Log("Making cave failed");
                break; 
            }

            Debug.Log("Checking if seed is stone");

            if (seed.blockData == stone)
            {
                Debug.Log("The Seed was stone");
                //Generating rando cave
                int size = (int) (Random.value * 30) + 1;
                int wh = (int)(Random.value * 7) + 1;// WH = Width Height aka what the cave will be sized
                Debug.Log($"Size: {size}");
                /* simple square cave with variation
                for (int x = 0; x < size / 2; x++)
                {
                    int caveYVariation = (int)(Random.value * 4);

                    if (((int)(Random.value * 2)) == 1) 
                    {
                        caveYVariation = -caveYVariation;
                    }

                    for (int y = 0; y < size + caveYVariation / 2; y++) 
                    {
                        Debug.Log($"Removing {x},{y}");

                        BlockInstance b = findBlock(seedpointx + x, seedpointy + y);
                        if (b != new BlockInstance()) 
                        {
                            map.Remove(b);
                        }
                        BlockInstance c = findBlock(seedpointx - x, seedpointy + y);
                        if (c != new BlockInstance())
                        {
                            map.Remove(c);
                        }

                    }
                }
                */
                //Branching cave system
                float dir = Random.value * 360;
                Debug.Log($"Dir: {dir}");

                for (int j = 0; j < size; j++) 
                {
                    if (dir < 180) // Lower 
                    {
                        if (dir > 90) // Left
                        {
                            if (dir > 135) // 
                            {
                                if (dir > 157) // -.5, -.5 down left
                                {
                                    for (int my = 0; my < 1; my++)
                                    {
                                        for (int mx = 0; mx < wh; my++)
                                        {
                                            BlockInstance b = findBlock(seedpointx - mx, seedpointy - my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy -= 1;
                                    seedpointx -= 1;

                                }
                                else  // -1, 0 left
                                {
                                    for (int mx = 0; mx < 2; mx++) 
                                    {
                                        for (int my = 0;my < wh; my++) 
                                        {
                                            BlockInstance b = findBlock(seedpointx - mx, seedpointy + my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointx -= 2;
                                }
                            }
                            else 
                            {
                                if (dir > 122) // -.5 -.5 down left
                                {
                                    for (int my = 0; my < 1; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx - mx, seedpointy - my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy -= 1;
                                    seedpointx -= 1;

                                }
                                else // down
                                {
                                    for (int my = 0; my < 2; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx + mx, seedpointy - my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy -= 2;
                                }
                            }
                        }
                        else //  Right
                        {
                            if (dir > 45)
                            {
                                if (dir > 67) // down 0, -1
                                {

                                    for (int my = 0; my < 2; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx + mx, seedpointy - my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy -= 2;

                                }
                                else  // down right -.5 -.5
                                {
                                    for (int my = 0; my < 1; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx + mx, seedpointy - my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy -= 1;
                                    seedpointx += 1;
                                }
                            }
                            else 
                            {
                                if (dir > 22) // down right 
                                {
                                    for (int my = 0; my < 1; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx + mx, seedpointy - my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy -= 1;
                                    seedpointx += 1;
                                }
                                else // right 
                                {
                                    for (int mx = 0; mx < 2; mx++)
                                    {
                                        for (int my = 0; my < wh; my++)
                                        {
                                            BlockInstance b = findBlock(seedpointx + mx, seedpointy + my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointx += 2;
                                }
                            }
                        }

                    }
                    else //Upper Half
                    {
                        if (dir < 270) // Left
                        {
                            if (dir > 225) // up left
                            {
                                if (dir > 247) // up
                                {
                                    for (int my = 0; my < 2; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx + mx, seedpointy + my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy += 2;
                                }
                                else //up left
                                {
                                    for (int my = 0; my < 1; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx - mx, seedpointy + my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy += 1;
                                    seedpointx -= 1;
                                }
                            }
                            else 
                            {
                                if (dir > 203) // up left
                                {
                                    for (int my = 0; my < 1; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx - mx, seedpointy + my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy += 1;
                                    seedpointx -= 1;
                                }
                                else // left
                                {
                                    for (int mx = 0; mx < 2; mx++)
                                    {
                                        for (int my = 0; my < wh; my++)
                                        {
                                            BlockInstance b = findBlock(seedpointx - mx, seedpointy + my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointx -= 2;
                                }
                            }
                        }
                        else //Right
                        {
                            if (dir > 315)
                            {
                                if (dir > 337) // right
                                {
                                    for (int mx = 0; mx < 2; mx++)
                                    {
                                        for (int my = 0; my < wh; my++)
                                        {
                                            BlockInstance b = findBlock(seedpointx + mx, seedpointy + my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointx += 2;
                                }
                                else // up right
                                {
                                    for (int my = 0; my < 1; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx + mx, seedpointy + my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy += 1;
                                    seedpointx += 1;
                                }
                            }
                            else 
                            {
                                if (dir > 292) // up
                                {
                                    for (int my = 0; my < 2; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx + mx, seedpointy + my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy += 2;
                                }
                                else //up right
                                {
                                    for (int my = 0; my < 1; my++)
                                    {
                                        for (int mx = 0; mx < wh; mx++)
                                        {
                                            BlockInstance b = findBlock(seedpointx + mx, seedpointy + my);
                                            if (b != new BlockInstance())
                                            {
                                                map.Remove(b);
                                            }
                                        }
                                    }
                                    seedpointy += 1;
                                    seedpointx += 1;
                                }
                            }
                        }
                    }


                }
            }
            else 
            {
                Debug.Log("The seed wasnt stone");
            }

            
        }
        

        //Adding Ore Clusters
        int clusterSizeMin = 4;
        int clusterSizeMax = 16;

        int maxClusters = 24;
        int minClusters = 4;

        int clusters = minClusters + ((int)(Random.value * (maxClusters - minClusters)));
        if (debugGeneration)
        {
            Debug.Log($"Clusters to be spawned: {clusters}");
        }

        for (int i = 0; i < clusters; i++)
        {

            int clusterSize = clusterSizeMin + ((int)(Random.value * (clusterSizeMax - clusterSizeMin)));
            Vector2[] cPos = new Vector2[clusterSize];

            int clusterX = 0;
            int clusterY = 0;
            cPos[0] = new Vector2(0, 0);

            for (int ii = 1; ii < clusterSize; ii++)
            {
                int die = (int)(Random.value * 4);
                if (die == 1) //drawing a square for remainder on one
                {
                    int diff = clusterSize - i;
                    int count = 0;
                    if (diff % 2 == 0)
                    {
                        for (int x = 0; x < diff / 2; x++)
                        {
                            for (int y = 0; y < diff / 2; y++)
                            {
                                if (ii + count >= clusterSize) { break; }
                                cPos[ii + count] =
                                    new Vector2(cPos[ii - 1].x +
                                    x, cPos[ii - 1].y - y);
                                count++;
                            }
                        }
                        break;
                    }
                    else
                    {
                        for (int x = 0; x < (diff / 2) - 1; x++)
                        {
                            for (int y = 0; y < diff / 2; y++)
                            {
                                if (ii + count >= clusterSize) { break; }
                                cPos[ii + count] = new Vector2(cPos[ii - 1].x + x, cPos[ii - 1].y - y);
                            }
                        }
                        break;
                    }
                }
                else if (die == 2) // going down
                {
                    cPos[ii] = new Vector2(cPos[ii].x, cPos[ii].y - 1);
                }
                else if (die == 3) // going down
                {
                    cPos[ii] = new Vector2(cPos[ii].x + 1, cPos[ii].y);
                }
            }

            BlockInstance start = (BlockInstance)map[(int)(Random.value * map.Count)];
            while (start.blockData != stone)
            {
                start = (BlockInstance)map[(int)(Random.value * map.Count)];
            }

            short startX = start.x;
            short startY = start.y;

            for (short s = 0; s < clusterSize; s++)
            {
                BlockInstance block = findBlock(startX + (short)cPos[s].x, startY + (short)cPos[s].y);
                block.blockData = ironOre;
            }
        }
        //Lowering the map to not stick the player in it (probably should be temporary;
        foreach (BlockInstance b in map) 
        {
            b.y -= highestPoint;
        }
    }

    void GenerateIsland()
    {

    }

    void CreateMap()
    {
        Vector3 myScale = new Vector3(4, 4, 1);
        int iterator = 0;
        foreach (BlockInstance b in map)
        {
            //Creating the block object
            GameObject block = new GameObject("block" + iterator + b.blockData.blockSprite);

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

    void OnDrawGizoms()
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
        short selectx = (short)Mathf.Floor((selector.transform.position.x / 1.28f) + .5f);
        short selecty = (short)Mathf.Floor((selector.transform.position.y / 1.28f) + .5f);

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

    BlockInstance findBlock(short x, short y)
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

    BlockInstance findBlock(int x, int y)
    {
        return findBlock((short)x, (short)y);
    }

    void DeleteBlock(BlockInstance bi)
    {
        Destroy(bi.GO);
        map.Remove(bi);
    }

    void BuildBlock(BlockData bd)
    {
        short selectx = (short)Mathf.Floor((selector.transform.position.x / 1.28f) + .5f);
        short selecty = (short)Mathf.Floor((selector.transform.position.y / 1.28f) + .5f);

        BlockInstance bi = new BlockInstance(debug, (short)selectx, (short)selecty);
        GameObject go = new GameObject("block X" + selectx + "Y" + selecty + bd.blockSprite);

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
                Enemy n = collided.GetComponent(typeof(Enemy)) as Enemy;
                if (n != null) 
                {
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
