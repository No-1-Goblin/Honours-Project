using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    // Reference to the prefab for a connector
    public GameObject connectorPrefab;
    // Size of the room
    public int sizeX = 5;
    public int sizeY = 5;
    // Tileset to use
    public WFCTileset tileset;
    // Allow failed generation attempts. For debugging
    public bool rejectFailedAttempts = true;
    // Log debug statements. Massively increases time to generate room
    public bool logDebugStatements = false;
    // The generated matrix for the room. Is a list because it will hold every possibility for each tile
    private List<int>[,] intMatrix;
    // The matrix generated for populating floor tiles
    private int[,] floorMatrix;
    // List of spawned objects, used for cleanup
    private List<GameObject> spawnedObjects;
    // Reference to our snappable piece component
    private SnappablePiece snappablePiece;

    /// <summary>
    /// Generate the room with connectors
    /// </summary>
    public void generateRoom()
    {
        // Generate the matrix for the room and instantiate objects
        while (!generateWFCMatrix());
        // Find the edges of the room
        List<Vector2Int> edges = findEdges();
        if (edges == null)
        {
            Debug.Log("Failed to find edges");
            return;
        }
        // Get our snappable piece component
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
        // Instantiate our two connectors and populate the list
        GameObject topConnector = Instantiate(connectorPrefab, transform);
        GameObject bottomConnector = Instantiate(connectorPrefab, transform);
        snappablePiece.populateConnectorList();
        // Position our connectors at the top and bottom edges of the room and rotate to face away from the room
        topConnector.transform.position = new Vector3(edges[0].x * tileset.tileSize.x + tileset.tileSize.x * 0.75f, 0,  edges[0].y * tileset.tileSize.y + tileset.tileSize.y * 0.25f);
        topConnector.transform.rotation = Quaternion.Euler(0, -90, 0);
        bottomConnector.transform.position = new Vector3(edges[1].x * tileset.tileSize.x + tileset.tileSize.x * 0.75f, 0, edges[1].y * tileset.tileSize.y + tileset.tileSize.y * 0.25f);
        bottomConnector.transform.rotation = Quaternion.Euler(0, 90, 0);
        // Setup our snappable piece collider for checking collision against the rest of the level
        float colliderSizeX = MathF.Abs(edges[3].x - edges[2].x) * tileset.tileSize.x + tileset.tileSize.x / 4f;
        float colliderSizeY = 5;
        float colliderSizeZ = MathF.Abs(edges[1].y - edges[0].y) * tileset.tileSize.y + tileset.tileSize.y / 4f;
        snappablePiece.boxCollider.size = new Vector3(colliderSizeX, colliderSizeY, colliderSizeZ);
        snappablePiece.boxCollider.center = new Vector3(edges[2].x * tileset.tileSize.x + colliderSizeX / 2f, colliderSizeY / 2f, edges[1].y * tileset.tileSize.y + colliderSizeZ / 2f);
    }

    /// <summary>
    /// Find the edges of the room since we might not have solid floor at the very edges of our matrix
    /// </summary>
    /// <returns></returns>
    public List<Vector2Int> findEdges()
    {
        // Which tile in our tileset is the edge type we are looking for
        int edgeID;
        // Whether we have found a tile of that type
        bool foundEdge;
        // The positions in the int matrix of our edges
        List<Vector2Int> edges;

        // Top edge
        // List of every top edge position
        edges = new();
        foundEdge = false;
        // Find which tile in the tileset is a top edge
        edgeID = findIndexForTileOfType(typeof(TopEdgeWFCTile));
        if (edgeID == -1)
        {
            Debug.Log("Failed to find top edge in tileset");
            return null;
        }
        // Search the int matrix from top to bottom
        for (int y = sizeY - 1; y >= 0; y--)
        {
            // Break here because we have found the top row
            if (foundEdge)
                break;
            for (int x = 0; x < sizeX; x++)
            {
                // If there is a top edge at this position, add it to the list
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
        // Choose one of the top edges at random to be where the connector generates
        Vector2Int topEdge = edges[UnityEngine.Random.Range(0, edges.Count)];

        // Bottom edge
        // List of every bottom edge we find
        edges = new();
        foundEdge = false;
        // Find which tile in our tileset is a bottom edge
        edgeID = findIndexForTileOfType(typeof(BottomEdgeWFCTile));
        if (edgeID == -1)
        {
            Debug.Log("Failed to find bottom edge in tileset");
            return null;
        }
        // Search the int matrix from bottom to top
        for (int y = 0; y < sizeY; y++)
        {
            // Break here because we found the bottom row
            if (foundEdge)
                break;
            for (int x = 0; x < sizeX; x++)
            {
                // If there is a bottom edge at this position, add it to the list
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
        // Choose one of the bottom edges at random to be where the connector generates
        Vector2Int bottomEdge = edges[UnityEngine.Random.Range(0, edges.Count)];

        // Left edge
        // List of every left edge we find
        edges = new();
        foundEdge = false;
        // Find which tile in our tileset is a left edge
        edgeID = findIndexForTileOfType(typeof(LeftEdgeWFCTile));
        if (edgeID == -1)
        {
            Debug.Log("Failed to find left edge in tileset");
            return null;
        }
        // Search through int matrix from left to right
        for (int x = 0; x < sizeX; x++)
        {
            // Break here because we found the leftmost column
            if (foundEdge)
                break;
            for (int y = 0; y < sizeY; y++)
            {
                // If there is a left edge at this position, add it to the list
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
        // Choose a random left edge to be our left edge
        Vector2Int leftEdge = edges[UnityEngine.Random.Range(0, edges.Count)];

        // Right edge
        // List of all the right edges we find
        edges = new();
        foundEdge = false;
        // Find which tile in our tileset is a right edge
        edgeID = findIndexForTileOfType(typeof(RightEdgeWFCTile));
        if (edgeID == -1)
        {
            Debug.Log("Failed to find right edge in tileset");
            return null;
        }
        // Search through the int matrix from right to left
        for (int x = sizeX - 1; x >= 0; x--)
        {
            // Break here because we found the rightmost edge
            if (foundEdge)
                break;
            for (int y = 0; y < sizeY; y++)
            {
                // If there's a right edge here, add it to the list
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
        // Choose a random right edge to be our right edge
        Vector2Int rightEdge = edges[UnityEngine.Random.Range(0, edges.Count)];

        // Return all the edges we found
        return new() { topEdge, bottomEdge, leftEdge, rightEdge };
    }

    /// <summary>
    /// Delete every spawned object and clear the matrices
    /// </summary>
    public void deleteSpawnedObjects()
    {
        if (spawnedObjects == null)
        {
            spawnedObjects = new();
        }
        // Delete any generated connectors
        SnappablePiece snappablePiece = GetComponent<SnappablePiece>();
        if (snappablePiece != null)
        {
            // Get all connectors
            snappablePiece.populateConnectorList();
            var connectors = snappablePiece.getConnectors();
            foreach (Connector connector in connectors)
            {
                if (connector != null)
                    // Destroy immediate so it works in editor
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
        // Create matrices
        intMatrix = new List<int>[sizeX, sizeY];
        floorMatrix = new int[sizeX, sizeY];
    }

  /// <summary>
  /// Generate the matrix and instantiate the tiles
  /// </summary>
  /// <returns></returns>
    public bool generateWFCMatrix()
    {
        // Do cleanup
        deleteSpawnedObjects();
        // Add lists
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                // Each position in matrix needs a blank list
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
        // Keep track of how many positions in the matrix still need generated
        int undetermined = updatePossibilities();
        while (undetermined != 0)
        {
            // Collapse the position with the least possibilities
            collapseLowestEntropy();
            // Collapse anything that was updated by the collapse and track how many positions still need updated
            undetermined = updatePossibilities();
            // This is for debugging purposes. Enable/disable in editor. Will massively slow generation
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
                    return false;
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

        // Populate the floor tiles with more interesting tiles
        populateFloorTiles();

        // Generate Level Pieces To Follow The Matrix
        for (int y = sizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < sizeX; x++)
            {
                // Should always be > 0 unless reject failed attempts is disabled
                if (intMatrix[x, y].Count > 0)
                    // Non-floor tiles use regular tileset
                    if (tileset.tiles[intMatrix[x, y][0]].GetType() != typeof(FloorWFCTile))
                        spawnedObjects.Add(Instantiate(tileset.tiles[intMatrix[x, y][0]], new Vector3(x * tileset.tileSize.x, 0, y * tileset.tileSize.y), Quaternion.identity, gameObject.transform).gameObject);
                    // Floor tiles use the floor matrix and floor tileset
                    else
                        spawnedObjects.Add(Instantiate(getFloorTile(x, y), new Vector3(x * tileset.tileSize.x, 0, y * tileset.tileSize.y), Quaternion.identity, gameObject.transform).gameObject);
            }
        }
        return true;
    }

    /// <summary>
    /// Updates all positions in the matrix. Called after collapsing the lowest entropy position
    /// </summary>
    /// <returns></returns>
    int updatePossibilities()
    {
        // Used for tracking if the whole matrix is populated
        int undeterminedTiles = 0;
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                // Get the possibility list for this position
                List<int> possibilities = intMatrix[x, y];
                // Skip if already determined
                if (possibilities.Count <= 1)
                    continue;
                // We're going in reverse to prevent issues with removing entries in the list as we go
                for (int i = possibilities.Count - 1; i >= 0; i--)
                {
                    // Check each rule to make sure all previous possibilities are still possible
                    if (!tileset.tiles[possibilities[i]].checkTileRule(intMatrix, sizeX, sizeY, tileset, new Tuple<int, int>(x, y)))
                        // Remove possibility from the list if it's no longer possible
                        possibilities.RemoveAt(i);
                }
                // This means we failed to generate the matrix as this position has no legal possibilities
                if (possibilities.Count == 0)
                    if (rejectFailedAttempts)
                    {
                        return -1;
                    }
                    else
                    {
                        continue;
                    }
                // If more than one possibility, then still undetermined
                if (possibilities.Count > 1)
                    undeterminedTiles++;
                // Print current state of matrix. Can be enabled/disabled in editor. Massively slows down generation
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

    /// <summary>
    /// Finds the position in the matrix with the fewest possibilities, and chooses one at random
    /// </summary>
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
                    // Just using number of possibilities as entropy value
                    entropyAtPosition.Add(new Tuple<int, int, int>(x, y, intMatrix[x, y].Count));
                }
            }
        }
        // Shuffle the list so we do it randomly
        entropyAtPosition.OrderByDescending(x => UnityEngine.Random.Range(0f, 1f));
        // Sort by entropy (but since we randomised the list before sorting this won't give us the same order of collapse every time)
        entropyAtPosition.OrderBy(x => x.Item3);
        // Collapse the lowest entropy
        collapse(new Tuple<int, int>(entropyAtPosition[0].Item1, entropyAtPosition[0].Item2));
    }

    /// <summary>
    /// Collapses the chosen position by choosing a random one of its valid possibilities to be its determined value
    /// </summary>
    /// <param name="position"></param>
    void collapse(Tuple<int, int> position)
    {
        List<int> possibilities = intMatrix[position.Item1, position.Item2];
        while (possibilities.Count > 1)
        {
            // Just remove random possibilities until there's only one left
            int randomInt = UnityEngine.Random.Range(0, possibilities.Count);
            possibilities.RemoveAt(randomInt);
        }
    }

    /// <summary>
    /// Finds the index of a tile in the tileset of the chosen type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    int findIndexForTileOfType(Type type)
    {
        int index = -1;
        // Iterate through tileset
        for (int i = 0; i < tileset.tiles.Count; i++)
        {
            // If we find a tile of matching type, record index and break
            if (tileset.tiles[i].GetType() == type)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    /// <summary>
    /// Populate the floor tiles of the generated int matrix by making a duplicate floor matrix with details of which floor tile will appear where
    /// </summary>
    void populateFloorTiles()
    {
        // Checkpoint generation

        // Position to generate a checkpoint
        Vector2 checkpointPosition = Vector2.zero;
        // Radius around the centre of the matrix to search for a floor position
        int searchRadius = 0;
        // Whether we have found a position for the checkpoint
        bool checkpointPlaced = false;
        while (!checkpointPlaced)
        {
            // Start from centre of the matrix, expanding search with the increasing radius
            for (int y = sizeY / 2 - searchRadius; y < sizeY / 2 + searchRadius; y++)
            {
                for (int x = sizeX / 2 - searchRadius; x < sizeX / 2 + searchRadius; x++)
                {
                    // If the tile at the position is a floor tile, it is our checkpoint location
                    if (tileset.tiles[intMatrix[x, y][0]].GetType() == typeof(FloorWFCTile))
                    {
                        checkpointPlaced = true;
                        checkpointPosition = new Vector2(x, y);
                    }
                }
            }
            searchRadius++;
        }

        // Populate floor matrix

        for (int y = sizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < sizeX; x++)
            {
                // Should always be > 0 unless reject failed attempts is disabled
                if (intMatrix[x, y].Count > 0)
                    // If a floor tile exists here in the int matrix
                    if (tileset.tiles[intMatrix[x, y][0]].GetType() == typeof(FloorWFCTile))
                    {
                        // If this is our checkpoint position, mark with -2
                        if (x == checkpointPosition.x && y == checkpointPosition.y)
                            floorMatrix[x, y] = -2;
                        // Otherwise generate a floor tile here
                        else
                            floorMatrix[x, y] = getRandomFloorTile();
                    }
                    // If not a floor tile, mark with -1
                    else
                        floorMatrix[x, y] = -1;
            }
        }
    }

    /// <summary>
    /// Gets the appropriate floor tile for the chosen position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    WFCTile getFloorTile(int x, int y)
    {
        WFCTile floorTile;
        // -2 means a checkpoint
        if (floorMatrix[x, y] == -2)
        {
            floorTile = tileset.checkpointTile;
        } else
        // Otherwise grab the correct floor tile from floor tileset according to floor matrix
        {
            floorTile = tileset.floorTiles[floorMatrix[x, y]];
        }

        return floorTile;
    }

    /// <summary>
    /// Chooses a random floor tile from the floor tileset to generate. Is generated with weighting
    /// </summary>
    /// <returns></returns>
    int getRandomFloorTile()
    {
        // Invalid floor tileset as it needs weightings for each tile
        if (tileset.floorTiles.Count != tileset.floorWeightings.Count)
            return 0;
        int piece = 0;
        // List of all our weightings
        List<int> weightingList = new();
        // Keep track of total weighting
        int weightingsTotal = 0;
        // For each floor weighting, add it to our list of weightings
        for (int i = 0; i < tileset.floorWeightings.Count; i++)
        {
            weightingList.Add(weightingsTotal + tileset.floorWeightings[i]);
            weightingsTotal += tileset.floorWeightings[i];
        }
        // Choose a random value between 0 and our total weighting. Unity random int is exclusive so add 1
        int randomValue = UnityEngine.Random.Range(0, weightingsTotal + 1);
        // For each floor tile in tileset
        for (int i = 0; i < weightingList.Count; i++)
        {
            // Check if it falls within the weighting for this tile
            if (randomValue < weightingList[i])
            {
                piece = i;
                break;
            }
        }
        return piece;
    }
}

#if UNITY_EDITOR
/// <summary>
/// Custom editor function for generating rooms and matrices
/// </summary>
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
#endif