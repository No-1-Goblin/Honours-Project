using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditSettings : MonoBehaviour
{
    [SerializeField] GeneratorSettings settings;
    [SerializeField] GeneratorSettings defaults;
    [SerializeField] TMPro.TMP_InputField roomCount;
    [SerializeField] TMPro.TMP_InputField minRoomDistanceX;
    [SerializeField] TMPro.TMP_InputField maxRoomDistanceX;
    [SerializeField] TMPro.TMP_InputField minRoomDistanceY;
    [SerializeField] TMPro.TMP_InputField maxRoomDistanceY;
    [SerializeField] TMPro.TMP_InputField minRoomDistanceZ;
    [SerializeField] TMPro.TMP_InputField maxRoomDistanceZ;
    [SerializeField] TMPro.TMP_InputField roomDistanceTolerance;
    [SerializeField] Button generate;
    [SerializeField] Button loadDefaults;
    [SerializeField] Button exit;
    void Start()
    {
        generate.onClick.AddListener(saveSettingsAndGenerate);
        loadDefaults.onClick.AddListener(loadDefaultSettings);
        exit.onClick.AddListener(exitGame);
        setText();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setText()
    {
        roomCount.text = settings.roomCount.ToString();
        minRoomDistanceX.text = settings.minRoomDistanceX.ToString();
        maxRoomDistanceX.text = settings.maxRoomDistanceX.ToString();
        minRoomDistanceY.text = settings.minRoomDistanceY.ToString();
        maxRoomDistanceY.text = settings.maxRoomDistanceY.ToString();
        minRoomDistanceZ.text = settings.minRoomDistanceZ.ToString();
        maxRoomDistanceZ.text = settings.maxRoomDistanceZ.ToString();
        roomDistanceTolerance.text = settings.roomDistanceTolerance.ToString();
    }

    void saveSettingsAndGenerate()
    {
        settings.roomCount = int.Parse(roomCount.text);
        settings.minRoomDistanceX = float.Parse(minRoomDistanceX.text);
        settings.maxRoomDistanceX = float.Parse(maxRoomDistanceX.text);
        settings.minRoomDistanceY = float.Parse(minRoomDistanceY.text);
        settings.maxRoomDistanceY = float.Parse(maxRoomDistanceY.text);
        settings.minRoomDistanceZ = float.Parse(minRoomDistanceZ.text);
        settings.maxRoomDistanceZ = float.Parse(maxRoomDistanceZ.text);
        settings.roomDistanceTolerance = float.Parse(roomDistanceTolerance.text);
        SceneManager.LoadScene("Generating Level");
    }

    void loadDefaultSettings()
    {
        settings.roomCount = defaults.roomCount;
        settings.minRoomDistanceX = defaults.minRoomDistanceX;
        settings.maxRoomDistanceX = defaults.maxRoomDistanceX;
        settings.minRoomDistanceY = defaults.minRoomDistanceY;
        settings.maxRoomDistanceY = defaults.maxRoomDistanceY;
        settings.minRoomDistanceZ = defaults.minRoomDistanceZ;
        settings.maxRoomDistanceZ = defaults.maxRoomDistanceZ;
        settings.roomDistanceTolerance = defaults.roomDistanceTolerance;
        setText();
    }

    void exitGame()
    {
        Application.Quit();
    }
}
