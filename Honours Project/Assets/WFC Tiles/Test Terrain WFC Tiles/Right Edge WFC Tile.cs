using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightEdgeWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Left must not be an edge tile
        if (isDeterminedAsType(adjacents[WFCDirections.left], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BottomEdgeWFCTile)))
            return false;
        // Right must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.right], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(FloorWFCTile)))
            return false;
        // Top must be able to be either another of this or TR corner
        if (cannotBeOfType(adjacents[WFCDirections.up], typeof(RightEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.up], typeof(TROuterCornerWFCTile)))
            return false;
        // Bottom must be able to be either another of this or BR corner
        if (cannotBeOfType(adjacents[WFCDirections.down], typeof(RightEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.down], typeof(BROuterCornerWFCTile)))
            return false;

        return true;
    }
}
