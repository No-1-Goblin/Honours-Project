using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BROuterCornerWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        List<Type> edges = new() { typeof(BLOuterCornerWFCTile), typeof(BottomEdgeWFCTile), typeof(BROuterCornerWFCTile), typeof(LeftEdgeWFCTile), typeof(RightEdgeWFCTile), typeof(TLOuterCornerWFCTile), typeof(TopEdgeWFCTile), typeof(TROuterCornerWFCTile), typeof(BLInnerCornerWFCTile), typeof(BRInnerCornerWFCTile), typeof(TLInnerCornerWFCTile), typeof(TRInnerCornerWFCTile), typeof(FloorWFCTile) };
        // Right must not be an edge tile or floor
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.right], edges))
            return false;
        // Bottom must not be an edge tile or floor
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.down], edges))
            return false;
        // Top must be able to be right edge
        if (cannotBeOfType(adjacents[WFCDirections.up], typeof(RightEdgeWFCTile)))
            return false;
        // Left must be able to be bottom edge
        if (cannotBeOfType(adjacents[WFCDirections.left], typeof(BottomEdgeWFCTile)))
            return false;


        return true;
    }
}
