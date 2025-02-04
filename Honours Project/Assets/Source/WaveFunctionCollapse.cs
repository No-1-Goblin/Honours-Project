using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    public int sizeX = 5;
    public int sizeY = 5;
    public WFCTileset tileset;
    private List<int>[,] intMatrix;
    private List<GameObject> spawnedObjects;
    public void deleteSpawnedObjects()
    {
        // Destroy all generated level elements
        while (spawnedObjects.Count > 0)
        {
            if (spawnedObjects[0])
            {
                // Destroy immediate so it works in editor
                DestroyImmediate(spawnedObjects[0]);
            }
            spawnedObjects.RemoveAt(0);
        }
        // Create matrix
        intMatrix = new List<int>[sizeX, sizeY];
    }

    public void generateWFCMatrix()
    {
        deleteSpawnedObjects();
        // Add lists
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                intMatrix[x, y] = new();
            }
        }
        // Add every possibility to every location
        foreach (List<int> list in intMatrix)
        {
            for (int i = 0; i < tileset.tiles.Count; i++)
            {
                list.Add(i);
            }
        }
        int undetermined = updatePossibilities();
        while (undetermined != 0)
        {
            collapseLowestEntropy();
            undetermined = updatePossibilities();
            // This here is for debugging purposes, remove this later
            string output1 = "";
            for (int y = sizeY - 1; y >= 0; y--)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (intMatrix[x, y].Count == 1)
                    {
                        output1 += intMatrix[x, y][0].ToString() + ", ";
                    } else
                    {
                        output1 += " ,  ";
                    }
                }
                output1 += "\n";
            }
            Debug.Log(output1);
            // Check if we failed to generate the matrix
            if (undetermined == -1)
            {
                Debug.Log("It all went wrong");
                generateWFCMatrix();
                return;
            }
        }
        // This is also for debug, since later this matrix will be used to generate the actual room
        string output = "";
        for (int y = sizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < sizeX; x++)
            {
                output += intMatrix[x, y][0].ToString() + ", ";
            }
            output += "\n";
        }
        Debug.Log(output);
        for (int y = sizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < sizeX; x++)
            {
                spawnedObjects.Add(Instantiate(tileset.tiles[intMatrix[x, y][0]], new Vector3(x * tileset.tileSize.x, 0, y * tileset.tileSize.y), Quaternion.identity, gameObject.transform).gameObject);
            }
        }
    }

    int updatePossibilities()
    {
        // Used for tracking if the whole matrix is populated
        int undeterminedTiles = 0;
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                List<int> possibilities = intMatrix[x, y];
                // Skip if already determined
                if (possibilities.Count == 1)
                    continue;
                for (int i = possibilities.Count - 1; i >= 0; i--)
                {
                    // Check each rule to make sure all previous possibilities are still possible
                    if (!tileset.tiles[possibilities[i]].checkTileRule(intMatrix, sizeX, sizeY, tileset, new Tuple<int, int>(x, y)))
                        // Remove possibility from the list if it's no longer possible
                        possibilities.RemoveAt(i);
                }
                // This means we failed to generate the matrix, I could add proper handling here but for now we just reroll the whole thing
                // A.k.a make sure the rules *can* produce a possible outcome
                if (possibilities.Count == 0)
                    return -1;
                if (possibilities.Count > 1)
                    undeterminedTiles++;
            }
        }
        return undeterminedTiles;
    }

    void collapseLowestEntropy()
    {
        List<Tuple<int, int, int>> entropyAtPosition = new();
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                // Only add to our list if isn't determined
                if (intMatrix[x, y].Count > 1)
                {
                    // Just using possibilities as entropy value
                    entropyAtPosition.Add(new Tuple<int, int, int>(x, y, intMatrix[x, y].Count));
                }
            }
        }
        // Shuffle the list so we do it randomly
        // I should really do this after removing all but the lowest entropy values for efficiency
        entropyAtPosition.OrderByDescending(x => UnityEngine.Random.Range(0f, 1f));
        // Sort by entropy (but since we randomised the list this won't give us the same order of collapse every time)
        entropyAtPosition.OrderBy(x => x.Item3);
        // Collapse the lowest entropy
        collapse(new Tuple<int, int>(entropyAtPosition[0].Item1, entropyAtPosition[0].Item2));
    }

    void collapse(Tuple<int, int> position)
    {
        //Debug.Log("Collapsing " + position.Item1.ToString() + ", " + position.Item2.ToString());
        List<int> possibilities = intMatrix[position.Item1, position.Item2];
        while (possibilities.Count > 1)
        {
            // Just remove random possibilities until there's only one left
            int randomInt = UnityEngine.Random.Range(0, possibilities.Count);
            possibilities.RemoveAt(randomInt);
        }
    }
}

[CustomEditor(typeof(WaveFunctionCollapse))]
public class WaveFunctionCollapseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var waveFunctionCollapse = (WaveFunctionCollapse)target;
        if (GUILayout.Button("Generate WFC Matrix"))
        {
            waveFunctionCollapse.generateWFCMatrix();
        }
        if (GUILayout.Button("Delete WFC Matrix"))
        {
            waveFunctionCollapse.deleteSpawnedObjects();
        }
    }
}