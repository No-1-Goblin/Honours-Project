using KinematicCharacterController.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public ExampleCharacterController player;
    public PersistentVariables persistentVariables;
    void Start()
    {
        persistentVariables.playerRespawn.AddListener(killPlayer);
        player = GameObject.FindWithTag("Player").GetComponent<ExampleCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < -100)
        {
            persistentVariables.playerRespawn.Invoke();
        }
    }

    void killPlayer()
    {
        player.Motor.SetPositionAndRotation(persistentVariables.respawnPosition, persistentVariables.respawnRotation);
        player.Motor.BaseVelocity = Vector3.zero;
    }
}
