using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SnappablePiece : MonoBehaviour
{
    [SerializeField] private List<Connector> connectors = new List<Connector>();
    public BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }
        populateConnectorList();
        boxCollider.isTrigger = true;
    }

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

    public List<Connector> getConnectors()
    {
        if (connectors.Count == 0)
            populateConnectorList();
        return new(connectors);
    }

    public List<Tuple<int, int, Vector3>> getConnectorDifferences()
    {
        List<Tuple<int, int, Vector3>> connectorDifferences = new();
        List<Connector> connectorList = getConnectors();
        for (int i = 0; i < connectorList.Count; i++)
        {
            Connector connector = connectorList[i];
            for (int j = 0; j < connectorList.Count; j++)
            {
                if (j == i)
                    continue;
                Connector connector2 = connectorList[j];
                Vector3 difference = connector2.transform.position - connector.transform.position;
                Tuple<int, int, Vector3> connectorDifference = new(connector.getIndex(), connector2.getIndex(), difference);
                connectorDifferences.Add(connectorDifference);
            }
        }
        return connectorDifferences;
    }

    public Tuple<int, int, Vector3> getConnectorDifference(int startConnectorIndex, int endConnectorIndex)
    {
        List<Connector> connectorList = getConnectors();
        Vector3 difference = connectorList[endConnectorIndex].transform.position - connectorList[startConnectorIndex].transform.position;
        return new(startConnectorIndex, endConnectorIndex, difference);
    }

    public Vector3 getProjectedPosition(int startConnectorIndex, int endConnectorIndex, Connector connectingConnector)
    {
        Vector3 difference = getConnectorDifference(startConnectorIndex, endConnectorIndex).Item3;
        Quaternion rotateAmount = Quaternion.FromToRotation(getConnectors()[startConnectorIndex].getConnectorNormal(), -connectingConnector.getConnectorNormal());
        return connectingConnector.transform.position + (rotateAmount * difference);
    }

    public Tuple<int, int, float> getOptimalConnectorLayoutForDistance(Connector previousConnector, Vector3 targetPosition)
    {
        Tuple<int, int, float> optimalLayout = new(-1, -1, float.MaxValue);
        List<Connector> connectorList = getConnectors();
        for (int i = 0; i < connectorList.Count; i++)
        {
            for (int j = 0; j < connectorList.Count; j++)
            {
                if (i == j)
                    continue;
                Vector3 projectedPosition = getProjectedPosition(i, j, previousConnector);
                float distance = (targetPosition - projectedPosition).sqrMagnitude;
                if (distance < optimalLayout.Item3)
                {
                    optimalLayout = new(i, j, distance);
                }
            }
        }
        return optimalLayout;
    }
}
