using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopEdgeWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        List<Type> edges = new() { typeof(BLOuterCornerWFCTile), typeof(BottomEdgeWFCTile), typeof(BROuterCornerWFCTile), typeof(LeftEdgeWFCTile), typeof(RightEdgeWFCTile), typeof(TLOuterCornerWFCTile), typeof(TopEdgeWFCTile), typeof(TROuterCornerWFCTile), typeof(BLInnerCornerWFCTile), typeof(BRInnerCornerWFCTile), typeof(TLInnerCornerWFCTile), typeof(TRInnerCornerWFCTile), typeof(EmptyWFCTile) };
        // Bottom must not be an edge tile or empty
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.down], edges))
            return false;
        // Top must not be an edge tile or floor
        edges.RemoveAt(edges.Count - 1);
        edges.Add(typeof(FloorWFCTile));
        if (isDeterminedAsOneOfTypes(adjacents[WFCDirections.up], edges))
            return false;
        // Left must be able to be either another of this or TL outer corner or TR inner corner
        if (cannotBeOfType(adjacents[WFCDirections.left], typeof(TopEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.left], typeof(TLOuterCornerWFCTile)) && cannotBeOfType(adjacents[WFCDirections.left], typeof(TRInnerCornerWFCTile)))
            return false;
        // Right must be able to be either another of this or TR outer corner or TL inner corner
        if (cannotBeOfType(adjacents[WFCDirections.right], typeof(TopEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.right], typeof(TROuterCornerWFCTile)) && cannotBeOfType(adjacents[WFCDirections.right], typeof(TLInnerCornerWFCTile)))
            return false;

        return true;
    }
}
