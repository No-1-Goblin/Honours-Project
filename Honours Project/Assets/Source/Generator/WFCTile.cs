using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WFCTile : MonoBehaviour
{
    // This is useful for writing rules without constantly having to remember how 2D arrays work
    // Also it's entirely possible my implementation of the matrix is bad, but this list is at least correct
    protected static class WFCDirections
    {
        public static readonly int
        downLeft = 0,
        down = 1,
        downRight = 2,
        left = 3,
        right = 4,
        upLeft = 5,
        up = 6,
        upRight = 7;
    }
    // This will be overriden by children, but just defaulting to "yes I can be this type" is good cause there's probably tiles that don't care about surroundings
    public virtual bool checkTileRule(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        return true;
    }

    // Get list of adjacents, in order to the "enum" above
    public List<List<WFCTile>> getAdjacents(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position)
    {
        List<List<WFCTile>> adjacents = new();
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                // Skip yourself (0 offset)
                if (x == 0 && y == 0)
                    continue;
                adjacents.Add(getRelative(matrix, sizeX, sizeY, tileset, position, new Tuple<int, int>(x, y)));
            }
        }
        return adjacents;
    }

    // Get tile possibilities relative to a position
    public List<WFCTile> getRelative(List<int>[,] matrix, int sizeX, int sizeY, WFCTileset tileset, Tuple<int, int> position, Tuple<int, int> relativePosition)
    {
        List<WFCTile> possiblities = new();
        // Combine our position and offset, society if tuples of same types could just be added
        Tuple<int, int> positionToCheck = new Tuple<int, int>(position.Item1 + relativePosition.Item1, position.Item2 + relativePosition.Item2);
        // Check we're within the bounds of our matrix
        if (positionToCheck.Item1 < 0 || positionToCheck.Item1 >= sizeX || positionToCheck.Item2 < 0 || positionToCheck.Item2 >= sizeY)
            return possiblities;
        foreach (int i in matrix[positionToCheck.Item1, positionToCheck.Item2])
        {
            // Convert from int to tile object, this is for checking what type of tile lies where, and allows our rules to be tileset agnostic
            possiblities.Add(tileset.tiles[i]);
        }
        return possiblities;
    }

    // Check whether the tile is determined as that type
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

    // Check whether the tile is determined as one of the types
    public bool isDeterminedAsOneOfTypes(List<WFCTile> possibilities, List<Type> types)
    {
        if (possibilities == null)
            return false;
        if (possibilities.Count != 1)
            return false;
        if (types == null)
            return false;
        if (types.Count < 1)
            return false;
        foreach (Type type in types)
        {
            if (possibilities[0].GetType() == type)
                return true;
        }
        return false;
    }

    // Check whether the tile can be of that type
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

    // Check whether tile cannot be of that type
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
