using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generator Settings", menuName = "Generator/Generator Settings")]
public class GeneratorSettings : ScriptableObject
{
    public Tileset tileset;
    public int minParts = 1;
    public int maxParts = 10;
}
