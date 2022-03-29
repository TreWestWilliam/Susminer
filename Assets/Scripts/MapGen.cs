using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGen : MonoBehaviour
{

    public bool debugBlock = false;
    public bool debugGeneration = false;
    public bool debugTool = false;

    public ArrayList map = new ArrayList();
    public ArrayList blocks = new ArrayList();

    public BlockManager blockManager;

    public int[,] heatmap = new int[300, 300];


    public int maxWidth = 200;
    public int maxHeight = 60;

    public int minWidth = 40;
    public int minHeight = 20;

    public int maxPosYVar = 5;
    public int maxNegYVar = 5;

    public int posYVar = 1;
    public int negYVar = 1;



    void Start() 
    {
        blocks = blockManager.blocks;
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
        return w + 1;

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
        return th + 1;
    }

    public async void HeatMap()
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
                map.Add(new BlockInstance((BlockData)blocks[3] , (int)x, (int)y));
            }
        }
        */

        int iterator = 0;
        Vector3 myScale = new Vector3(4, 4, 1);
        Debug.Log("Drawing Blocks");

        int sanity = 0;
        Texture2D newText = new Texture2D(heatmap.GetUpperBound(0), heatmap.GetUpperBound(1));

        for (int x = 0; x < heatmap.GetUpperBound(0); x++)//
        {
            for (int y = 0; y < heatmap.GetUpperBound(1); y++)
            {
                float ratio = ((float)heatmap[x, y]) / iterations;

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

    public async void GenerateMapTest()
    {
        Random.InitState((int)(Random.value * 1000));

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

        int highestPoint = 0;
        int lowestPoint = 0;
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
                highestPoint = (int)startHeight;
            }
            int endHeight = (curHeight + negYVar) * -1;

            //Tree Generator, basic edition
            if (((int)(Random.value * 10)) == 5)
            {
                int treeHeight = (int)(Random.value * 10) + 2;
                for (int t = 1; t < treeHeight; t++)
                {
                    map.Add(new BlockInstance(log, (int)x, (int)(startHeight + t)));
                }
            }
            int dirtVariation = (int)(Random.value * 3);

            for (int y = startHeight; y > endHeight; y--)
            {
                if (y == startHeight)
                {
                    map.Add(new BlockInstance(grass, (int)x, (int)y));
                }
                else if (y < startHeight && y >= startHeight - 2 - dirtVariation)
                {
                    map.Add(new BlockInstance(dirt, (int)x, (int)y));
                }
                else
                {
                    map.Add(new BlockInstance(stone, (int)x, (int)y));
                }
            }

            if (endHeight < lowestPoint) { lowestPoint = (int)endHeight; }
            if (debugGeneration)
            {
                Debug.Log($"StartHeight: {startHeight} End Height: {endHeight}");
            }
            for (int y = startHeight; y > endHeight; y--)
            {
                if (y == startHeight)
                {
                    map.Add(new BlockInstance(grass, (int)(x), (int)(y)));
                }
                else if (y < startHeight && y >= startHeight - 2)
                {
                    map.Add(new BlockInstance(dirt, (int)(x), (int)(y)));
                }
                else
                {
                    map.Add(new BlockInstance(stone, (int)(x), (int)(y)));
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

            int startX = start.x;
            int startY = start.y;

            for (int s = 0; s < clusterSize; s++)
            {
                BlockInstance block = findBlock(startX + (int)cPos[s].x, startY + (int)cPos[s].y);
                block.blockData = ironOre;
            }
        }
        //Lowering the map to not stick the player in it (probably should be temporary;
        Debug.Log($"Lowest Point{lowestPoint}");
        foreach (BlockInstance b in map)
        {
            //b.y += (int) -lowestPoint;
            //b.x += (int) offset;
            b.y += (int)150;
            b.x += (int)150;
        }
    }

    public async void GenerateMap()
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



        int heightDiff = maxHeight - minHeight;
        int widthDiff = maxWidth - minWidth;

        int height = ((int)(Random.value * heightDiff) + minHeight);
        int width = ((int)(Random.value * widthDiff) + minWidth);

        int coreWidth = (int)(width / 3);
        int coreHeight = (int)(height);
        int baseHeight = 2;

        int prevHeightVar = (int)(Random.value * 4);
        int curHeight = baseHeight;


        int highestPoint = 0;
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
                highestPoint = (int)startHeight;
            }
            int endHeight = (curHeight + negYVar) * -1;
            if (debugGeneration)
            {
                Debug.Log($"StartHeight: {startHeight} End Height: {endHeight}");
            }

            int dirtVariation = (int)(Random.value * 3);

            //Tree Generator, basic edition
            if (((int)(Random.value * 10)) == 5)
            {
                int treeHeight = (int)(Random.value * 10) + 2;
                for (int t = 1; t < treeHeight; t++)
                {
                    map.Add(new BlockInstance(log, (int)x, (int)(startHeight + t)));
                }
            }

            for (int y = startHeight; y > endHeight; y--)
            {
                if (y == startHeight)
                {
                    map.Add(new BlockInstance(grass, (int)x, (int)y));
                }
                else if (y < startHeight && y >= startHeight - 2 - dirtVariation)
                {
                    map.Add(new BlockInstance(dirt, (int)x, (int)y));
                }
                else
                {
                    map.Add(new BlockInstance(stone, x, y));
                }
            }
        }

        // Carve Caves
        int caves = (int)(Random.value * 4) + 1;
        Debug.Log("Making caves " + caves);
        for (int i = 0; i < caves; i++)
        {
            int seedpointy = (int)-(Random.value * height);
            int seedpointx = (int)((Random.value * width) - (width / 2));

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
                int size = (int)(Random.value * 30) + 1;
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
                if (debugGeneration)
                {
                    Debug.Log("The seed wasnt stone");
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

            int startX = start.x;
            int startY = start.y;

            for (int s = 0; s < clusterSize; s++)
            {
                BlockInstance block = findBlock(startX + (int)cPos[s].x, startY + (int)cPos[s].y);
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

}
