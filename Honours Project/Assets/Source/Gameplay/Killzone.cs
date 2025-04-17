using KinematicCharacterController.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public ExampleCharacterController player;
    public int deaths = 0;
    public PersistentVariables persistentVariables;
    void Start()
    {
        deaths = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < -100)
        {
            deaths++;
            player.Motor.SetPositionAndRotation(persistentVariables.respawnPosition, persistentVariables.respawnRotation);
            player.Motor.BaseVelocity = Vector3.zero;
        }
    }
}
