using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generator Tileset", menuName = "Generator/Generator Tileset")]
public class Tileset : ScriptableObject
{
    public List<SnappablePiece> startPieces = new();
    public List<SnappablePiece> endPieces = new();
    public List<SnappablePiece> standardPieces = new();

}
