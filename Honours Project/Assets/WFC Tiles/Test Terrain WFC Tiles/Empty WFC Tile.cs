using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        // Check up
        if (isDeterminedAsType(adjacents[WFCDirections.up], typeof(BLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BRInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(FloorWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TRInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TROuterCornerWFCTile)))
            return false;
        // Check down
        if (isDeterminedAsType(adjacents[WFCDirections.down], typeof(BLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BRInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(FloorWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TRInnerCornerWFCTile)))
            return false;
        // Check left
        if (isDeterminedAsType(adjacents[WFCDirections.left], typeof(BLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BRInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(FloorWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TRInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TopEdgeWFCTile)))
            return false;
        // Check right
        if (isDeterminedAsType(adjacents[WFCDirections.right], typeof(BLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BRInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(FloorWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TRInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TROuterCornerWFCTile)))
            return false;
        return true;
    }
}
