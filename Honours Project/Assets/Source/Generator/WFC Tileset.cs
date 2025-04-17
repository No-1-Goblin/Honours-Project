using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WFC Tileset", menuName = "Generator/WFC Tileset")]
public class WFCTileset : ScriptableObject
{
    public List<WFCTile> tiles = new();
    public List<WFCTile> floorTiles = new();
    public Vector2 tileSize = Vector2.one;
    public WFCTile checkpointTile;
}
