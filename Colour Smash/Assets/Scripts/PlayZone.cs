using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayZone
{
    public static PlayZone instance;
    public PlayZone()
    {
        instance = this;
    }
    public TileProperty[,] currentState = new TileProperty[AlignCubes.gridSize, AlignCubes.gridSize];
}
