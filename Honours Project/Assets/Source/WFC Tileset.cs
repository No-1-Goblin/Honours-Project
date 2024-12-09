using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WFC Tileset", menuName = "Generator/WFC Tileset")]
public class WFCTileset : ScriptableObject
{
    public List<WFCTile> tiles = new();
}
