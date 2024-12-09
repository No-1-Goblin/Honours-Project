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

    public void generateWFCMatrix()
    {
        // Create matrix
        intMatrix = new List<int>[sizeX, sizeY];
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
            if (undetermined == -1)
            {
                Debug.Log("It all went wrong");
                return;
            }
        }
        string output = "";
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                output += intMatrix[x, y][0].ToString() + ", ";
            }
            output += "\n";
        }
        Debug.Log(output);
    }

    int updatePossibilities()
    {
        int undeterminedTiles = 0;
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                List<int> possibilities = intMatrix[x, y];
                for (int i = possibilities.Count - 1; i >= 0; i--)
                {
                    if (!tileset.tiles[possibilities[i]].checkTileRule(intMatrix, sizeX, sizeY, tileset, new Tuple<int, int>(x, y)))
                        possibilities.RemoveAt(i);
                }
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
                if (intMatrix[x, y].Count > 1)
                {
                    entropyAtPosition.Add(new Tuple<int, int, int>(x, y, intMatrix[x, y].Count));
                }
            }
        }
        entropyAtPosition.OrderByDescending(x => UnityEngine.Random.Range(0f, 1f));
        entropyAtPosition.OrderBy(x => x.Item3);
        collapse(new Tuple<int, int>(entropyAtPosition[0].Item1, entropyAtPosition[0].Item2));
    }

    void collapse(Tuple<int, int> position)
    {
        List<int> possibilities = intMatrix[position.Item1, position.Item2];
        while (possibilities.Count > 1)
        {
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
    }
}