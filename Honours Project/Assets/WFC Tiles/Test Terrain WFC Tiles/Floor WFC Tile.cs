using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorWFCTile : WFCTile
{
    public override bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        var adjacents = getAdjacents(matrix, sizeX, sizeY, tileset, position);
        if (isDeterminedAsType(adjacents[WFCDirections.up], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.up], typeof(BRInnerCornerWFCTile)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.down], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.down], typeof(TRInnerCornerWFCTile)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.left], typeof(RightEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(BRInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.left], typeof(TRInnerCornerWFCTile)))
            return false;
        if (isDeterminedAsType(adjacents[WFCDirections.right], typeof(LeftEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TopEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BLOuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BROuterCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BottomEdgeWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(BLInnerCornerWFCTile)) || isDeterminedAsType(adjacents[WFCDirections.right], typeof(TLInnerCornerWFCTile)))
            return false;
        if (position.Item1 == 0 || position.Item1 == sizeX - 1 || position.Item2 == 0 || position.Item2 == sizeY - 1)
            return false;
        return true;

    }
}