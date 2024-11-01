using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [SerializeField] private bool connected = false;
    [SerializeField] private GameObject internalConnector;
    
    public void setConnected(bool isConnected)
    {
        connected = isConnected;
    }

    public bool isConnected()
    {
        return connected;
    }

    public Vector3 getConnectorNormal()
    {
        return transform.position - internalConnector.transform.position;
    }
}
