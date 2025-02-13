using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomEdgeWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Top must not be an edge tile
        if (isDeterminedAsType(adjacents[WFCDirections.up], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BottomEdgeWFCTile)))
            return false;
        // Bottom must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.down], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(FloorWFCTile)))
            return false;
        // Left must be able to be either another of this or BL corner
        if (cannotBeOfType(adjacents[WFCDirections.left], typeof(BottomEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.left], typeof(BLOuterCornerWFCTile)))
            return false;
        // Right must be able to be either another of this or BR corner
        if (cannotBeOfType(adjacents[WFCDirections.right], typeof(BottomEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.right], typeof(BROuterCornerWFCTile)))
            return false;

        return true;
    }
}
