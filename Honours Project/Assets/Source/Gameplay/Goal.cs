using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public PersistentVariables persistentVariables;
    public GameObject starMesh;
    public float rotation = 0;
    public float rotationSpeed = 180;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Generate Level On Load")
            {
                SceneManager.LoadScene("Edit Generator Settings");
            }
            else
            {
                SceneManager.LoadScene("BetweenLevels");
            }
        }
    }

    private void FixedUpdate()
    {
        rotation += Time.deltaTime * rotationSpeed;
        starMesh.transform.rotation = Quaternion.Euler(0, rotation, 0);
    }
}
