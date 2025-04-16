using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generator Settings", menuName = "Generator/Generator Settings")]
public class GeneratorSettings : ScriptableObject
{
    public Tileset tileset;
    public int minParts = 1;
    public int maxParts = 10;
    public int roomCount = 10;
    public float minRoomDistanceX = 100;
    public float maxRoomDistanceX = 300;
    public float minRoomDistanceY = -50;
    public float maxRoomDistanceY = 50;
    public float minRoomDistanceZ = -200;
    public float maxRoomDistanceZ = 200;
}
