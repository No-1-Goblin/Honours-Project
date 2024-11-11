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
        populateConnectorList();
        boxCollider.isTrigger = true;
    }

    private void populateConnectorList()
    {
        Connector[] temp = GetComponentsInChildren<Connector>();
        connectors.Clear();
        foreach (Connector connector in temp)
        {
            connectors.Add(connector);
        }
    }

    public List<Connector> getConnectors()
    {
        populateConnectorList();
        return connectors;
    }

    public List<Tuple<Tuple<int, int>, Vector3>> getConnectorDifferences()
    {
        List<Tuple<Tuple<int, int>, Vector3>> connectorDifferences = new();
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
                Tuple<Tuple<int, int>, Vector3> connectorDifference = new(new(i, j), difference);
                connectorDifferences.Add(connectorDifference);
            }
        }
        return connectorDifferences;
    }
}
