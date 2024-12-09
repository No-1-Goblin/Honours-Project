using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWFCTile1 : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        if (isDeterminedAsType(adjacents[WFCDirections.up], typeof(TestWFCTile1)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.down], typeof(TestWFCTile1)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.left], typeof(TestWFCTile1)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.right], typeof(TestWFCTile1)))
            return false;
        return true;
    }
}
