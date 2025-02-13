using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLOuterCornerWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Left must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.left], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(FloorWFCTile)))
            return false;
        // Top must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.up], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(FloorWFCTile)))
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
