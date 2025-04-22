using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBetweenLevelMessage : MonoBehaviour
{
    public Text text;
    public PersistentVariables persistentVariables;

    private void Start()
    {
        if (persistentVariables.currentLevel < persistentVariables.levels.Count - 1)
            text.text = "You have just completed level: " + (persistentVariables.currentLevel + 1) + "\nPlease fill out the next section\nof the survey before continuing\nYou died " + persistentVariables.deaths + " times, and collected " + persistentVariables.coins + " coins\nPress [Enter] to continue to the next level";
        else
            text.text = "You have just completed level: " + (persistentVariables.currentLevel + 1) + "\nPlease fill out the next section\nof the survey before continuing\nYou died " + persistentVariables.deaths + " times, and collected " + persistentVariables.coins + " coins\nYou have now completed every level\nThank you for contributing to this research";
    }
}
