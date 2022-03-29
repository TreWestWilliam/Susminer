using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInstance 
{
    public BlockData blockData;
    public int x;
    public int y;
    public GameObject GO;
    public bool isValid = false;

    public BlockInstance(BlockData d, int mx, int my) 
    {
        blockData = d;
        x = mx;
        y = my;
        isValid = true;
    }

    public BlockInstance(BlockData d, int mx, int my, GameObject g)
    {
        blockData = d;
        x = mx;
        y = my;
        GO = g;
        isValid = true;
    }

    public BlockInstance()
    {
    }
}
