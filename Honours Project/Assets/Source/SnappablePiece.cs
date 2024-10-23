using System.Collections;
using System.Collections.Generic;
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
}
