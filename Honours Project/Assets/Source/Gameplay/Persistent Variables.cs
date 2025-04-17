using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Persistent Variables", menuName = "ScriptableObjects/PersistentVariables")]
public class PersistentVariables : ScriptableObject
{
    public Vector3 respawnPosition = Vector3.zero;
    public Quaternion respawnRotation = Quaternion.identity;
    private void OnEnable()
    {
        respawnPosition = Vector3.zero;
        respawnRotation = Quaternion.identity;
    }
}
