using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generator Tileset", menuName = "Generator/Generator Tileset")]
public class Tileset : ScriptableObject
{
    // List of references to all the start pieces
    public List<SnappablePiece> startPieces = new();
    // List of references to all the end pieces
    public List<SnappablePiece> endPieces = new();
    // List of references to all the pieces to be placed between
    public List<SnappablePiece> standardPieces = new();

}
