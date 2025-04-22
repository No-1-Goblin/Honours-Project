using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateOnLoad : MonoBehaviour
{
    [SerializeField] LevelGenerator levelGenerator;
    [SerializeField] PersistentVariables persistentVariables;
    // Start is called before the first frame update
    void Start()
    {
        levelGenerator.generateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            persistentVariables.resetVariables();
            SceneManager.LoadScene("Edit Generator Settings");
        }
    }
}
