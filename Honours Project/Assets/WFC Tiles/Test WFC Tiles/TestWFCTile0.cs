using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWFCTile0 : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Cannot be directly next to another 0
        if (isDeterminedAsType(adjacents[WFCDirections.up], typeof(TestWFCTile0)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.down], typeof(TestWFCTile0)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.left], typeof(TestWFCTile0)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.right], typeof(TestWFCTile0)))
            return false;
        return true;
    }
}
