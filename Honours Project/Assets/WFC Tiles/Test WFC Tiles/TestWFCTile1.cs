using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWFCTile1 : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Cannot be directly next to another 1
        if (isDeterminedAsType(adjacents[WFCDirections.up], typeof(TestWFCTile1)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.down], typeof(TestWFCTile1)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.left], typeof(TestWFCTile1)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.right], typeof(TestWFCTile1)))
            return false;
        // Cannot be directly above a 2
        if (isDeterminedAsType(adjacents[WFCDirections.down], typeof(TestWFCTile2)))
            return false;
        return true;
    }
}
