using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] GeneratorSettings settings;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Generate Level On Load");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
