using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SnappablePiece : MonoBehaviour
{
    // List of the level piece's connectors
    [SerializeField] private List<Connector> connectors = new List<Connector>();
    // The boxcollider that acts as the bounds of this level piece
    public BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        if (boxCollider == null)
        {
            // Find our collider if it wasn't assigned in editor
            boxCollider = GetComponent<BoxCollider>();
        }
        // Find all our connectors
        populateConnectorList();
        // Make sure the collider is set to trigger so players don't bounce off it
        boxCollider.isTrigger = true;
    }

    /// <summary>
    /// Updates the connector list
    /// </summary>
    public void populateConnectorList()
    {
        Connector[] temp = GetComponentsInChildren<Connector>();
        connectors.Clear();
        foreach (Connector connector in temp)
        {
            connector.setIndex(connectors.Count);
            connectors.Add(connector);
        }
    }

    /// <summary>
    /// Get a list of connectors in this level piece
    /// </summary>
    /// <returns></returns>
    public List<Connector> getConnectors()
    {
        // If our list isn't populated, populate it
        if (connectors.Count == 0)
            populateConnectorList();
        return new(connectors);
    }

    /// <summary>
    /// Get all connector orientations for this level piece, along with displacement between them
    /// </summary>
    /// <returns></returns>
    public List<Tuple<int, int, Vector3>> getConnectorDisplacements()
    {
        // List of all our layouts and displacements
        List<Tuple<int, int, Vector3>> connectorDisplacements = new();
        // Get all our connectors
        List<Connector> connectorList = getConnectors();
        // Iterate through every connector as start connector
        for (int i = 0; i < connectorList.Count; i++)
        {
            // Iterate through every connector as end connector
            Connector connector = connectorList[i];
            for (int j = 0; j < connectorList.Count; j++)
            {
                // We don't want to check displacement between a connector and itself
                if (j == i)
                    continue;
                Connector connector2 = connectorList[j];
                // Calculate displacement
                Vector3 Displacement = connector2.transform.position - connector.transform.position;
                // Pack all the info we need and add it to list
                Tuple<int, int, Vector3> connectorDisplacement = new(connector.getIndex(), connector2.getIndex(), Displacement);
                connectorDisplacements.Add(connectorDisplacement);
            }
        }
        return connectorDisplacements;
    }

    /// <summary>
    /// Get the displacement between the two connectors, along with the orientation of the connectors
    /// </summary>
    /// <param name="startConnectorIndex"></param>
    /// <param name="endConnectorIndex"></param>
    /// <returns></returns>
    public Tuple<int, int, Vector3> getConnectorDisplacement(int startConnectorIndex, int endConnectorIndex)
    {
        // Get all our connectors
        List<Connector> connectorList = getConnectors();
        // Calculate displacement between the two connectors
        Vector3 displacement = connectorList[endConnectorIndex].transform.position - connectorList[startConnectorIndex].transform.position;
        return new(startConnectorIndex, endConnectorIndex, displacement);
    }

    /// <summary>
    /// Get the projected position of the end connector if the level piece was connected to the passed in connector using the start connector
    /// </summary>
    /// <param name="startConnectorIndex">Connector to connect to previous level piece</param>
    /// <param name="endConnectorIndex">Connector that will be unconnected</param>
    /// <param name="connectingConnector">Connector from old level piece to connect to</param>
    /// <returns></returns>
    public Vector3 getProjectedPosition(int startConnectorIndex, int endConnectorIndex, Connector connectingConnector)
    {
        // Get connector displacement between the two chosen indexes
        Vector3 displacement = getConnectorDisplacement(startConnectorIndex, endConnectorIndex).Item3;
        // Figure out how to rotate the displacement to accurately reflect the displacement once the level piece was rotated
        Quaternion rotateAmount = Quaternion.FromToRotation(getConnectors()[startConnectorIndex].getConnectorNormal(), -connectingConnector.getConnectorNormal());
        // Add our rotated displacement to the old connector's position to find where the new position would be
        return connectingConnector.transform.position + (rotateAmount * displacement);
    }

    /// <summary>
    /// Find the optimal layout for this level piece to be connected to the previous level piece to get as close to the target position as possible
    /// </summary>
    /// <param name="previousConnector"></param>
    /// <param name="targetPosition"></param>
    /// <returns>Tuple where int 1 is the connector index we want to connect to the previous level piece, int 2 is the connector index where the connector will be unconnected, and the float is the distance from the target position that the unconnected connector will end up at</returns>
    public Tuple<int, int, float> getOptimalConnectorLayoutForDistance(Connector previousConnector, Vector3 targetPosition)
    {
        // Dummy values to be replaced
        Tuple<int, int, float> optimalLayout = new(-1, -1, float.MaxValue);
        // Get all our connectors
        List<Connector> connectorList = getConnectors();
        // Iterate through each connector as start connector
        for (int i = 0; i < connectorList.Count; i++)
        {
            // Iterate through each connector as end connector
            for (int j = 0; j < connectorList.Count; j++)
            {
                // We don't want to compare connectors to themselves
                if (i == j)
                    continue;
                // Get the projected position were we to use this layout
                Vector3 projectedPosition = getProjectedPosition(i, j, previousConnector);
                // Calculate the distance to the target position with this layout
                float distance = (targetPosition - projectedPosition).sqrMagnitude;
                // If this layout is better than the old one, replace it
                if (distance < optimalLayout.Item3)
                {
                    optimalLayout = new(i, j, distance);
                }
            }
        }
        return optimalLayout;
    }
}
