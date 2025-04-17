using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    public GameObject connectorPrefab;
    public int sizeX = 5;
    public int sizeY = 5;
    public WFCTileset tileset;
    public bool rejectFailedAttempts = true;
    public bool logDebugStatements = false;
    private List<int>[,] intMatrix;
    private int[,] floorMatrix;
    private List<GameObject> spawnedObjects;
    private SnappablePiece snappablePiece;

    public void generateRoom()
    {
        generateWFCMatrix();
        List<Vector2Int> edges = findEdges();
        if (edges == null)
        {
            Debug.Log("Failed to find edges");
            return;
        }
        snappablePiece = GetComponent<SnappablePiece>();
        if (snappablePiece == null )
        {
            Debug.Log("Snappable piece component required to generate room. Add snappable piece or generate matrix instead");
            return;
        }
        if (connectorPrefab == null)
        {
            Debug.Log("No connector prefab set. Set prefab in the editor");
            return;
        }
        GameObject topConnector = Instantiate(connectorPrefab, transform);
        GameObject bottomConnector = Instantiate(connectorPrefab, transform);
        snappablePiece.populateConnectorList();
        topConnector.transform.position = new Vector3(edges[0].x * tileset.tileSize.x + tileset.tileSize.x * 0.5f, 0,  edges[0].y * tileset.tileSize.y + tileset.tileSize.y * 0.5f);
        topConnector.transform.rotation = Quaternion.Euler(0, -90, 0);
        bottomConnector.transform.position = new Vector3(edges[1].x * tileset.tileSize.x + tileset.tileSize.x * 0.5f, 0, edges[1].y * tileset.tileSize.y);
        bottomConnector.transform.rotation = Quaternion.Euler(0, 90, 0);
        float colliderSizeX = MathF.Abs(edges[3].x - edges[2].x) * tileset.tileSize.x + tileset.tileSize.x / 2f;
        float colliderSizeY = 5;
        float colliderSizeZ = MathF.Abs(edges[1].y - edges[0].y) * tileset.tileSize.y + tileset.tileSize.y / 2f;
        snappablePiece.boxCollider.size = new Vector3(colliderSizeX, colliderSizeY, colliderSizeZ);
        snappablePiece.boxCollider.center = new Vector3(edges[2].x * tileset.tileSize.x + colliderSizeX / 2f, colliderSizeY / 2f, edges[1].y * tileset.tileSize.y + colliderSizeZ / 2f);
    }

    public List<Vector2Int> findEdges()
    {
        int edgeID;
        bool foundEdge;
        List<Vector2Int> edges;

        // Top edge
        edges = new();
        foundEdge = false;
        edgeID = findIndexForTileOfType(typeof(TopEdgeWFCTile));
        if (edgeID == -1)
        {
            Debug.Log("Failed to find top edge in tileset");
            return null;
        }
        for (int y = sizeY - 1; y >= 0; y--)
        {
            if (foundEdge)
                break;
            for (int x = 0; x < sizeX; x++)
            {
                if (intMatrix[x, y][0] == edgeID)
                {
                    foundEdge = true;
                    edges.Add(new(x, y));
                }
            }
        }
        if (!foundEdge)
        {
            Debug.Log("Failed to find a top edge in the generated matrix");
            return null;
        }
        Vector2Int topEdge = edges[UnityEngine.Random.Range(0, edges.Count)];

        // Bottom edge
        edges = new();
        foundEdge = false;
        edgeID = findIndexForTileOfType(typeof(BottomEdgeWFCTile));
        if (edgeID == -1)
        {
            Debug.Log("Failed to find bottom edge in tileset");
            return null;
        }
        for (int y = 0; y < sizeY; y++)
        {
            if (foundEdge)
                break;
            for (int x = 0; x < sizeX; x++)
            {
                if (intMatrix[x, y][0] == edgeID)
                {
                    foundEdge = true;
                    edges.Add(new(x, y));
                }
            }
        }
        if (!foundEdge)
        {
            Debug.Log("Failed to find a bottom edge in the generated matrix");
            return null;
        }
        Vector2Int bottomEdge = edges[UnityEngine.Random.Range(0, edges.Count)];

        // Left edge
        edges = new();
        foundEdge = false;
        edgeID = findIndexForTileOfType(typeof(LeftEdgeWFCTile));
        if (edgeID == -1)
        {
            Debug.Log("Failed to find left edge in tileset");
            return null;
        }
        for (int x = 0; x < sizeX; x++)
        {
            if (foundEdge)
                break;
            for (int y = 0; y < sizeY; y++)
            {
                if (intMatrix[x, y][0] == edgeID)
                {
                    foundEdge = true;
                    edges.Add(new(x, y));
                }
            }
        }
        if (!foundEdge)
        {
            Debug.Log("Failed to find a left edge in the generated matrix");
            return null;
        }
        Vector2Int leftEdge = edges[UnityEngine.Random.Range(0, edges.Count)];

        // Left edge
        edges = new();
        foundEdge = false;
        edgeID = findIndexForTileOfType(typeof(RightEdgeWFCTile));
        if (edgeID == -1)
        {
            Debug.Log("Failed to find right edge in tileset");
            return null;
        }
        for (int x = sizeX - 1; x >= 0; x--)
        {
            if (foundEdge)
                break;
            for (int y = 0; y < sizeY; y++)
            {
                if (intMatrix[x, y][0] == edgeID)
                {
                    foundEdge = true;
                    edges.Add(new(x, y));
                }
            }
        }
        if (!foundEdge)
        {
            Debug.Log("Failed to find a right edge in the generated matrix");
            return null;
        }
        Vector2Int rightEdge = edges[UnityEngine.Random.Range(0, edges.Count)];

        return new() { topEdge, bottomEdge, leftEdge, rightEdge };
    }
    public void deleteSpawnedObjects()
    {
        if (spawnedObjects == null)
        {
            spawnedObjects = new();
        }
        SnappablePiece snappablePiece = GetComponent<SnappablePiece>();
        if (snappablePiece != null)
        {
            snappablePiece.populateConnectorList();
            var connectors = snappablePiece.getConnectors();
            foreach (Connector connector in connectors)
            {
                if (connector != null)
                    DestroyImmediate(connector.gameObject);
            }
            snappablePiece.populateConnectorList();
        }
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
        floorMatrix = new int[sizeX, sizeY];
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
            if (logDebugStatements)
            {
                string output1 = "";
                for (int y = sizeY - 1; y >= 0; y--)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        if (intMatrix[x, y].Count == 1)
                        {
                            output1 += intMatrix[x, y][0].ToString() + ", ";
                        }
                        else
                        {
                            output1 += " ,  ";
                        }
                    }
                    output1 += "\n";
                }
                Debug.Log(output1);
            }
            // Check if we failed to generate the matrix
            if (undetermined == -1)
            {
                if (logDebugStatements)
                Debug.Log("It all went wrong");
                if (rejectFailedAttempts)
                {
                    generateWFCMatrix();
                    return;
                }
            }
        }
        if (logDebugStatements)
        {
            // This is also for debug, since later this matrix will be used to generate the actual room
            string output = "";
            for (int y = sizeY - 1; y >= 0; y--)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (intMatrix[x, y].Count == 1)
                    {
                        output += intMatrix[x, y][0].ToString() + ", ";
                    }
                    else
                    {
                        output += "X, ";
                    }
                }
                output += "\n";
            }
            Debug.Log(output);
        }

        populateFloorTiles();

        // Generate Level Pieces To Follow The Matrix
        for (int y = sizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if (intMatrix[x, y].Count > 0)
                    if (tileset.tiles[intMatrix[x, y][0]].GetType() != typeof(FloorWFCTile))
                        spawnedObjects.Add(Instantiate(tileset.tiles[intMatrix[x, y][0]], new Vector3(x * tileset.tileSize.x, 0, y * tileset.tileSize.y), Quaternion.identity, gameObject.transform).gameObject);
                    else
                        spawnedObjects.Add(Instantiate(getFloorTile(x, y), new Vector3(x * tileset.tileSize.x, 0, y * tileset.tileSize.y), Quaternion.identity, gameObject.transform).gameObject);
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
                if (possibilities.Count <= 1)
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
                    if (rejectFailedAttempts)
                    {
                        return -1;
                    }
                    else
                    {
                        continue;
                    }
                if (possibilities.Count > 1)
                    undeterminedTiles++;
                if (logDebugStatements)
                {
                    string output = "";
                    for (int a = sizeY - 1; a >= 0; a--)
                    {
                        for (int b = 0; b < sizeX; b++)
                        {
                            if (intMatrix[b, a].Count == 1)
                            {
                                output += intMatrix[b, a][0].ToString() + ", ";
                            }
                            else
                            {
                                output += "X, ";
                            }
                        }
                        output += "\n";
                    }
                    Debug.Log(output);
                }
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
        //if (entropyAtPosition.Count > 0)
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

    int findIndexForTileOfType(Type type)
    {
        int index = -1;
        for (int i = 0; i < tileset.tiles.Count; i++)
        {
            if (tileset.tiles[i].GetType() == type)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    void populateFloorTiles()
    {
        Vector2 checkpointPosition = Vector2.zero;
        int searchRadius = 0;
        bool checkpointPlaced = false;
        while (!checkpointPlaced)
        {
            for (int y = sizeY / 2 - searchRadius; y < sizeY / 2 + searchRadius; y++)
            {
                for (int x = sizeX / 2 - searchRadius; x < sizeX / 2 + searchRadius; x++)
                {
                    if (tileset.tiles[intMatrix[x, y][0]].GetType() == typeof(FloorWFCTile))
                    {
                        checkpointPlaced = true;
                        checkpointPosition = new Vector2(x, y);
                    }
                }
            }
            searchRadius++;
        }
        for (int y = sizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if (intMatrix[x, y].Count > 0)
                    if (tileset.tiles[intMatrix[x, y][0]].GetType() == typeof(FloorWFCTile))
                    {
                        if (x == checkpointPosition.x && y == checkpointPosition.y)
                            floorMatrix[x, y] = -2;
                        else
                            floorMatrix[x, y] = 0;
                    }
                    else
                        floorMatrix[x, y] = -1;
            }
        }
    }

    WFCTile getFloorTile(int x, int y)
    {
        WFCTile floorTile;
        if (floorMatrix[x, y] == -2)
        {
            floorTile = tileset.checkpointTile;
        } else
        {
            floorTile = tileset.floorTiles[floorMatrix[x, y]];
        }

        return floorTile;
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
        if (GUILayout.Button("Generate Room"))
        {
            waveFunctionCollapse.generateRoom();
        }
        if (GUILayout.Button("Delete"))
        {
            waveFunctionCollapse.deleteSpawnedObjects();
        }
    }
}
