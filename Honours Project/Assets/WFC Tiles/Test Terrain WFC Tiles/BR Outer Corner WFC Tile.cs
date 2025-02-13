using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BROuterCornerWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Right must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.right], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(FloorWFCTile)))
            return false;
        // Bottom must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.down], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(FloorWFCTile)))
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
