using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLOuterCornerWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Left must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.left], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(FloorWFCTile)))
            return false;
        // Bottom must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.down], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(FloorWFCTile)))
            return false;
        // Top must be able to be left edge
        if (cannotBeOfType(adjacents[WFCDirections.up], typeof(LeftEdgeWFCTile)))
            return false;
        // Right must be able to be bottom edge
        if (cannotBeOfType(adjacents[WFCDirections.right], typeof(BottomEdgeWFCTile)))
            return false;

        return true;
    }
}
