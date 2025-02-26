using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftEdgeWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        List<Type> edges = new() { typeof(BLOuterCornerWFCTile), typeof(BottomEdgeWFCTile), typeof(BROuterCornerWFCTile), typeof(LeftEdgeWFCTile), typeof(RightEdgeWFCTile), typeof(TLOuterCornerWFCTile), typeof(TopEdgeWFCTile), typeof(TROuterCornerWFCTile), typeof(BLInnerCornerWFCTile), typeof(BRInnerCornerWFCTile), typeof(TLInnerCornerWFCTile), typeof(TRInnerCornerWFCTile), typeof(EmptyWFCTile) };
        // Right must not be an edge tile or empty
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.right], edges))
            return false;
        // Left must not be an edge tile or floor
        edges.RemoveAt(edges.Count - 1);
        edges.Add(typeof(FloorWFCTile));
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.left], edges))
            return false;
        // Top must be able to be either another of this or TL outer corner or BL inner corner
        if (cannotBeOfType(adjacents[WFCDirections.up], typeof(LeftEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.up], typeof(TLOuterCornerWFCTile)) && cannotBeOfType(adjacents[WFCDirections.up], typeof(BLInnerCornerWFCTile)))
            return false;
        // Bottom must be able to be either another of this or BL outer corner or TL inner corner
        if (cannotBeOfType(adjacents[WFCDirections.down], typeof(LeftEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.down], typeof(BLOuterCornerWFCTile)) && cannotBeOfType(adjacents[WFCDirections.down], typeof(BLInnerCornerWFCTile)) && cannotBeOfType(adjacents[WFCDirections.down], typeof(TLInnerCornerWFCTile)))
            return false;

        return true;
    }
}
