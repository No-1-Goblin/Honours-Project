using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    public Text text;
    public PersistentVariables persistentVariables;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "Coins: 0\n\nDeaths: 0";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Coins: " + persistentVariables.coins + "\n\nDeaths: " + persistentVariables.deaths;
    }
}
