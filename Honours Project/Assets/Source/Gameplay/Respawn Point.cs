using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnPoint : MonoBehaviour
{
    public BoxCollider boxCollider;
    public bool triggered = false;
    public PersistentVariables persistentVariables;
    public GameObject flag;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!triggered)
            {
                triggered = true;
                persistentVariables.respawnPosition = this.transform.position;
                persistentVariables.respawnRotation = this.transform.rotation;
                flag.SetActive(false);
            }
        }
    }
}
