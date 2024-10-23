using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappablePiece : MonoBehaviour
{
    [SerializeField] private List<Connector> connectors = new List<Connector>();
    // Start is called before the first frame update
    void Start()
    {
        populateConnectorList();
        
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
        return connectors;
    }
}
