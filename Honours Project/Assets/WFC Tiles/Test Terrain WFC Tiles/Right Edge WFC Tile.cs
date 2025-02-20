using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightEdgeWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        List<Type> edges = new() { typeof(BLOuterCornerWFCTile), typeof(BottomEdgeWFCTile), typeof(BROuterCornerWFCTile), typeof(LeftEdgeWFCTile), typeof(RightEdgeWFCTile), typeof(TLOuterCornerWFCTile), typeof(TopEdgeWFCTile), typeof(TROuterCornerWFCTile), typeof(BLInnerCornerWFCTile), typeof(BRInnerCornerWFCTile), typeof(TLInnerCornerWFCTile), typeof(TRInnerCornerWFCTile), };
        // Left must not be an edge tile
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.left], edges))
            return false;
        // Right must not be an edge tile or floor
        edges.Add(typeof(FloorWFCTile));
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.right], edges))
            return false;
        // Top must be able to be either another of this or TR outer corner or BR inner corner
        if (cannotBeOfType(adjacents[WFCDirections.up], typeof(RightEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.up], typeof(TROuterCornerWFCTile)) && cannotBeOfType(adjacents[WFCDirections.up], typeof(BRInnerCornerWFCTile)))
            return false;
        // Bottom must be able to be either another of this or BR outer corner or TR inner corner
        if (cannotBeOfType(adjacents[WFCDirections.down], typeof(RightEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.down], typeof(BROuterCornerWFCTile)) && cannotBeOfType(adjacents[WFCDirections.down], typeof(TRInnerCornerWFCTile)))
            return false;

        return true;
    }
}
