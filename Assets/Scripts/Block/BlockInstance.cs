using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInstance 
{
    public BlockData blockData;
    public short x;
    public short y;
    public GameObject GO;
    public bool isValid = false;

    public BlockInstance(BlockData d, short mx, short my) 
    {
        blockData = d;
        x = mx;
        y = my;
        isValid = true;
    }

    public BlockInstance(BlockData d, short mx, short my, GameObject g)
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
