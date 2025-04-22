using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginResearch : MonoBehaviour
{
    public PersistentVariables persistentVariables;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (persistentVariables.currentLevel < persistentVariables.levels.Count)
            {
                Cursor.visible = false;
                persistentVariables.resetVariables();
                persistentVariables.currentLevel = 0;
                SceneManager.LoadScene(persistentVariables.levels[0]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Cursor.visible = false;
            persistentVariables.resetVariables();
            persistentVariables.currentLevel = 0;
            SceneManager.LoadScene(persistentVariables.levels[persistentVariables.currentLevel]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Cursor.visible = false;
            persistentVariables.resetVariables();
            persistentVariables.currentLevel = 1;
            SceneManager.LoadScene(persistentVariables.levels[persistentVariables.currentLevel]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Cursor.visible = false;
            persistentVariables.resetVariables();
            persistentVariables.currentLevel = 2;
            SceneManager.LoadScene(persistentVariables.levels[persistentVariables.currentLevel]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Cursor.visible = false;
            persistentVariables.resetVariables();
            persistentVariables.currentLevel = 3;
            SceneManager.LoadScene(persistentVariables.levels[persistentVariables.currentLevel]);
        }
    }
}
