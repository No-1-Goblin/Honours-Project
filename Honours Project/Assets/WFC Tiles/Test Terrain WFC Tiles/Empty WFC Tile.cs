using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {

        return true;
    }
}
