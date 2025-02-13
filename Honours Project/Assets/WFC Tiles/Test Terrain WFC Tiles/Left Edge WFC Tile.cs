using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftEdgeWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Right must not be an edge tile
        if (isDeterminedAsType(adjacents[WFCDirections.right], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BottomEdgeWFCTile)))
            return false;
        // Left must not be an edge tile or floor
        if (isDeterminedAsType(adjacents[WFCDirections.left], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(FloorWFCTile)))
            return false;
        // Top must be able to be either another of this or TL corner
        if (cannotBeOfType(adjacents[WFCDirections.up], typeof(LeftEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.up], typeof(TLOuterCornerWFCTile)))
            return false;
        // Bottom must be able to be either another of this or BL corner
        if (cannotBeOfType(adjacents[WFCDirections.down], typeof(LeftEdgeWFCTile)) && cannotBeOfType(adjacents[WFCDirections.down], typeof(BLOuterCornerWFCTile)))
            return false;

        return true;
    }
}
