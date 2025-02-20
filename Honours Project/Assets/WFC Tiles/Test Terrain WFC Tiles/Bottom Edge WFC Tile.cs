using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomEdgeWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        List<Type> edges = new() { typeof(BLOuterCornerWFCTile), typeof(BottomEdgeWFCTile), typeof(BROuterCornerWFCTile), typeof(LeftEdgeWFCTile), typeof(RightEdgeWFCTile), typeof(TLOuterCornerWFCTile), typeof(TopEdgeWFCTile), typeof(TROuterCornerWFCTile), typeof(BLInnerCornerWFCTile), typeof(BRInnerCornerWFCTile), typeof(TLInnerCornerWFCTile), typeof(TRInnerCornerWFCTile) };
        // Top must not be an edge tile
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.up], edges))
            return false;
        // Bottom must not be an edge tile or floor
        edges.Add(typeof(FloorWFCTile));
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.down], edges))
            return false;
        // Left must be able to be either another of this or BL outer corner or BR inner corner
        if (cannotBeOfType(adjacents[WFCDirections.left], typeof(BottomEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.left], typeof(BLOuterCornerWFCTile)) && cannotBeOfType(adjacents[WFCDirections.left], typeof(BRInnerCornerWFCTile)))
            return false;
        // Right must be able to be either another of this or BR outer corner or BL inner corner
        if (cannotBeOfType(adjacents[WFCDirections.right], typeof(BottomEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.right], typeof(BROuterCornerWFCTile)) && cannotBeOfType(adjacents[WFCDirections.right], typeof(BLInnerCornerWFCTile)))
            return false;

        return true;
    }
}
