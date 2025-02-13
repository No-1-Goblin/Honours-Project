using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TROuterCornerWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Right must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.right], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(FloorWFCTile)))
            return false;
        // Top must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.up], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(FloorWFCTile)))
            return false;
        // Bottom must be able to be right edge
        if (cannotBeOfType(adjacents[WFCDirections.down], typeof(RightEdgeWFCTile)))
            return false;
        // Left must be able to be top edge
        if (cannotBeOfType(adjacents[WFCDirections.left], typeof(TopEdgeWFCTile)))
            return false;


        return true;
    }
}
