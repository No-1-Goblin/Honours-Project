using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLInnerCornerWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        List<Type> edges = new() { typeof(BLOuterCornerWFCTile), typeof(BottomEdgeWFCTile), typeof(BROuterCornerWFCTile), typeof(LeftEdgeWFCTile), typeof(RightEdgeWFCTile), typeof(TLOuterCornerWFCTile), typeof(TopEdgeWFCTile), typeof(TROuterCornerWFCTile), typeof(BLInnerCornerWFCTile), typeof(BRInnerCornerWFCTile), typeof(TLInnerCornerWFCTile), typeof(TRInnerCornerWFCTile), typeof(EmptyWFCTile) };
        // Top must not be an edge or empty
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.up], edges))
            return false;
        // Right must not be an edge or empty
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.right], edges))
            return false;
        // Left must be able to be bottom edge
        if (cannotBeOfType(adjacents[WFCDirections.left], typeof(BottomEdgeWFCTile)))
            return false;
        // Bottom must be able to be left edge
        if (cannotBeOfType(adjacents[WFCDirections.down], typeof(LeftEdgeWFCTile)))
            return false;
        return true;
    }
}
