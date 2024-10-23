using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [SerializeField] private bool connected = false;
    
    public void setConnected(bool isConnected)
    {
        connected = isConnected;
    }

    public bool isConnected()
    {
        return connected;
    }
}
