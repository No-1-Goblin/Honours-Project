using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WFC Tileset", menuName = "Generator/WFC Tileset")]
public class WFCTileset : ScriptableObject
{
    // Regular tileset
    public List<WFCTile> tiles = new();
    // Floor tileset. Does not include checkpoint
    public List<WFCTile> floorTiles = new();
    // Weightings for floor tileset
    public List<int> floorWeightings = new();
    // Size of the tiles in the tileset
    public Vector2 tileSize = Vector2.one;
    // The checkpoint tile
    public WFCTile checkpointTile;
}
