using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    // Whether the connector has been connected to another level piece's connector
    [SerializeField] private bool connected = false;
    // An extra empty sceneobject used to better visualise which way the connector is facing. Also used for direction calculations
    [SerializeField] private GameObject internalConnector;
    // Index of the connector in its level piece
    [SerializeField] private int index = -1;
    
    /// <summary>
    /// Set the connector as connected or disconnected
    /// </summary>
    /// <param name="isConnected"></param>
    public void setConnected(bool isConnected)
    {
        connected = isConnected;
    }

    /// <summary>
    /// Get whether the connector is connected
    /// </summary>
    /// <returns></returns>
    public bool isConnected()
    {
        return connected;
    }

    /// <summary>
    /// Get a vector normal to the edge of the level piece. This will be the direction the connector is facing
    /// </summary>
    /// <returns></returns>
    public Vector3 getConnectorNormal()
    {
        return transform.position - internalConnector.transform.position;
    }

    /// <summary>
    /// Set the index of the connector. It needs to know this for functions used to find the optimal connector layout for the level piece
    /// </summary>
    /// <param name="newIndex"></param>
    public void setIndex(int newIndex)
    { 
        index = newIndex;
    }

    /// <summary>
    /// Returns the index of the connector as set by setIndex
    /// </summary>
    /// <returns></returns>
    public int getIndex()
    {
        return index;
    }
}
