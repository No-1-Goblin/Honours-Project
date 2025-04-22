using KinematicCharacterController.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public PersistentVariables persistentVariables;
    public GameObject player;
    public Rigidbody rigidBody;
    public Vector3 spawnPoint;
    public bool canSeePlayer = false;
    public float speed = 20;
    public float spotDistance = 7;
    public float loseDistance = 11;
    // Start is called before the first frame update
    void Start()
    {
        persistentVariables.playerRespawn.AddListener(respawn);
        player = GameObject.FindWithTag("Player");
        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 displacementToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = displacementToPlayer.sqrMagnitude;
        if (canSeePlayer)
        {
            if (distanceToPlayer > loseDistance * loseDistance)
            {
                canSeePlayer = false;
            }
        } else
        {
            if (distanceToPlayer < spotDistance * spotDistance)
            {
                canSeePlayer = true;
            }
        }
        if (canSeePlayer)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();
            Vector3 velocity = direction * speed;
            rigidBody.velocity = velocity;
        } else
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.transform.position.y - 0.25f > gameObject.transform.position.y)
            {
                gameObject.SetActive(false);
                other.GetComponent<ExampleCharacterController>().Motor.BaseVelocity.y = other.GetComponent<ExampleCharacterController>().JumpUpSpeed;
            }
            else
            {
                persistentVariables.playerRespawn.Invoke();
            }
        }
    }

    public void respawn()
    {
        if (gameObject.activeSelf)
        {
            transform.position = spawnPoint;
            canSeePlayer = false;
        }
    }
}
