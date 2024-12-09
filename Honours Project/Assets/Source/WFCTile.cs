using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WFCTile : MonoBehaviour
{
    protected static class WFCDirections
    {
        public static readonly int
        upLeft = 0,
        up = 1,
        upRight = 2,
        left = 3,
        right = 4,
        downLeft = 5,
        down = 6,
        downRight = 7;
    }
    public virtual bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        return true;
    }

    public List<List<WFCTile>> getAdjacents(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        List<List<WFCTile>> adjacents = new();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                adjacents.Add(getRelative(matrix, sizeX, sizeY, tileset, position, new Tuple<int, int>(x, y)));
            }
        }
        return adjacents;
    }

    public List<WFCTile> getRelative(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position, Tuple<int, int> relativePosition)
    {
        List<WFCTile> possiblities = new();
        Tuple<int, int> positionToCheck = new Tuple<int, int>(position.Item1 + relativePosition.Item1, position.Item2 + relativePosition.Item2);
        if (positionToCheck.Item1 < 0 || positionToCheck.Item1 >= sizeX || positionToCheck.Item2 < 0 || positionToCheck.Item2 >= sizeY)
            return possiblities;
        foreach (int i in matrix[positionToCheck.Item1, positionToCheck.Item2])
        {
            possiblities.Add(tileset.tiles[i]);
        }
        return possiblities;
    }

    public bool isDeterminedAsType(List<WFCTile> possibilities, Type type)
    {
        if (possibilities == null)
            return false;
        if (possibilities.Count != 1)
            return false;
        if (type == null)
            return false;
        if (possibilities[0].GetType() != type)
            return false;
        return true;
    }

    public bool canBeOfType(List<WFCTile> possibilities, Type type)
    {
        if (possibilities == null)
            return false;
        if (type == null)
            return false;
        foreach (WFCTile tile in possibilities)
        {
            if (tile.GetType() == type)
                return true;
        }
        return false;
    }

    public bool cannotBeOfType(List<WFCTile> possibilities, Type type)
    {
        if (possibilities == null)
            return true;
        if (type == null)
            return true;
        foreach (WFCTile tile in possibilities)
        {
            if (tile.GetType() == type)
                return false;
        }
        return true;
    }
}
