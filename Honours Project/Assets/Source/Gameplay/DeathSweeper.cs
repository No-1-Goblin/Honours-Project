using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSweeper : MonoBehaviour
{
    public float sweepRadius = 3;
    public float sweepSpeed = 3;
    public PersistentVariables persistentVariables;
    float direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        persistentVariables.playerRespawn.AddListener(respawn);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x + direction * (sweepSpeed * Time.deltaTime), 1, 0);
        if (Mathf.Sqrt(transform.localPosition.x * transform.localPosition.x) > sweepRadius)
        {
            transform.localPosition = new Vector3(sweepRadius * direction, 1, 0);
            direction = -direction;
        }
    }

    void respawn()
    {
        transform.localPosition = new Vector3(0, 1, 0);
        direction = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            persistentVariables.playerRespawn.Invoke();
        }
    }
}
