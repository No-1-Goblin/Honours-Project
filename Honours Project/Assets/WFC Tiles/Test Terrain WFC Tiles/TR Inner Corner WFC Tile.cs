using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRInnerCornerWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        List<Type> edges = new() { typeof(BLOuterCornerWFCTile), typeof(BottomEdgeWFCTile), typeof(BROuterCornerWFCTile), typeof(LeftEdgeWFCTile), typeof(RightEdgeWFCTile), typeof(TLOuterCornerWFCTile), typeof(TopEdgeWFCTile), typeof(TROuterCornerWFCTile), typeof(BLInnerCornerWFCTile), typeof(BRInnerCornerWFCTile), typeof(TLInnerCornerWFCTile), typeof(TRInnerCornerWFCTile), typeof(EmptyWFCTile) };
        // Bottom must not be an edge or empty
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.down], edges))
            return false;
        // Left must not be an edge or empty
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.left], edges))
            return false;
        // Right must be able to be top edge
        if (cannotBeOfType(adjacents[WFCDirections.right], typeof(TopEdgeWFCTile)))
            return false;
        // Top must be able to be right edge
        if (cannotBeOfType(adjacents[WFCDirections.up], typeof(RightEdgeWFCTile)))
            return false;
        return true;
    }
}
