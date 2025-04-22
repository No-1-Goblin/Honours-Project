using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generator Settings", menuName = "Generator/Generator Settings")]
public class GeneratorSettings : ScriptableObject
{
    // Reference to the tileset the generator will use
    public Tileset tileset;
    // Number of rooms for the level generator to generate
    public int roomCount = 10;
    // Minimum increase in X coordinate between rooms
    public float minRoomDistanceX = 100;
    // Maximum increase in X coordinate between rooms
    public float maxRoomDistanceX = 300;
    // Minimum increase in Y coordinate between rooms
    public float minRoomDistanceY = -50;
    // Maximum increase in Y coordinate between rooms
    public float maxRoomDistanceY = 50;
    // Minimum increase in Z coordinate between rooms
    public float minRoomDistanceZ = -200;
    // Maximum increase in Z coordinate between rooms
    public float maxRoomDistanceZ = 200;
    // The distance from the designated room position that the level generator can place the rooms
    public float roomDistanceTolerance = 50;
}
