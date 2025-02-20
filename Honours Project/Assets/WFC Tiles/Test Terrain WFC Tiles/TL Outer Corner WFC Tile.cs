using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLOuterCornerWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        List<Type> edges = new() { typeof(BLOuterCornerWFCTile), typeof(BottomEdgeWFCTile), typeof(BROuterCornerWFCTile), typeof(LeftEdgeWFCTile), typeof(RightEdgeWFCTile), typeof(TLOuterCornerWFCTile), typeof(TopEdgeWFCTile), typeof(TROuterCornerWFCTile), typeof(BLInnerCornerWFCTile), typeof(BRInnerCornerWFCTile), typeof(TLInnerCornerWFCTile), typeof(TRInnerCornerWFCTile), typeof(FloorWFCTile) };
        // Left must not be an edge tile or floor
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.left], edges))
            return false;
        // Top must not be an edge tile or floor
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.up], edges))
            return false;
        // Bottom must be able to be left edge
        if (cannotBeOfType(adjacents[WFCDirections.down], typeof(LeftEdgeWFCTile)))
            return false;
        // Right must be able to be top edge
        if (cannotBeOfType(adjacents[WFCDirections.right], typeof(TopEdgeWFCTile)))
            return false;


        return true;
    }
}
