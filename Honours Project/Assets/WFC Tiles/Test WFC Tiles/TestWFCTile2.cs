using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWFCTile2 : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Cannot be directly below a 1
        if (isDeterminedAsType(adjacents[WFCDirections.up], typeof(TestWFCTile1)))
            return false;
        return true;
    }
}
