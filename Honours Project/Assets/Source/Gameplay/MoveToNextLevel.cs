using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{
    public PersistentVariables persistentVariables;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            persistentVariables.currentLevel++;
            if (persistentVariables.currentLevel < persistentVariables.levels.Count)
            {
                Cursor.visible = false;
                persistentVariables.resetVariables();
                SceneManager.LoadScene(persistentVariables.levels[persistentVariables.currentLevel]);
            }
        }
    }
}
