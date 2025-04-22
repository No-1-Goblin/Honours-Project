using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Persistent Variables", menuName = "ScriptableObjects/PersistentVariables")]
public class PersistentVariables : ScriptableObject
{
    public Vector3 respawnPosition = Vector3.zero;
    public Quaternion respawnRotation = Quaternion.identity;
    public int coins = 0;
    public int deaths = 0;
    public UnityEvent playerRespawn;
    public int currentLevel = 0;
    public List<string> levels;
    private void OnEnable()
    {
        resetVariables();
        currentLevel = 0;
    }

    public void resetVariables()
    {
        respawnPosition = Vector3.zero;
        respawnRotation = Quaternion.identity;
        coins = 0;
        deaths = 0;
        playerRespawn = new UnityEvent();
        playerRespawn.AddListener(incrementDeathCounter);
    }

    public void incrementDeathCounter()
    {
        deaths++;
    }
}
