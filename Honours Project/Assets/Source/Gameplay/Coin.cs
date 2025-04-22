using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Coin : MonoBehaviour
{
    public BoxCollider boxCollider;
    public bool collected = false;
    public PersistentVariables persistentVariables;
    public GameObject coinMesh;
    public float rotation = 0;
    public float rotationSpeed = 180;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        rotation = UnityEngine.Random.Range(0, 360);
        coinMesh.transform.rotation = Quaternion.Euler(0, rotation, 90);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotation += Time.deltaTime * rotationSpeed;
        coinMesh.transform.rotation = Quaternion.Euler(0, rotation, 90);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!collected)
            {
                collected = true;
                persistentVariables.coins++;
                coinMesh.SetActive(false);
            }
        }
    }
}
