using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GeneratorSettings settings;
    [SerializeField] private WaveFunctionCollapse roomPrefab;
    private List<GameObject> generatedObjects = new();
    private List<SnappablePiece> populationQueue = new();
    public Vector3 targetPos = new(0, 0, 0);

    public GeneratorSettings getSettings()
    {
        return settings;
    }

    public void setGeneratorSettings(GeneratorSettings newSettings)
    {
        settings = newSettings;
    }

    public void generateLevel()
    {
        // Clean up old level first
        deleteLevel();
        if (!validateGeneratorSettings())
        {
            Debug.Log("Invalid Generator Settings");
            return;
        }
        SnappablePiece startPiece = generateStartPoint();
        int generatedPieces = 0;
        populationQueue = new();
        populationQueue.Add(startPiece);
        while (populationQueue.Count != 0 && generatedPieces <= settings.maxParts)
        {
            populateConnections(populationQueue[0], targetPos);
            populationQueue.RemoveAt(0);
            generatedPieces++;
            if (populationQueue.Count == 0)
            {
                populationQueue.Add(generatedObjects[generatedObjects.Count - 1].GetComponent<SnappablePiece>());
            }
        }
        var connectors = populationQueue[0].getConnectors();
        foreach (var connector in connectors)
        {
            if (connector.isConnected())
                continue;
            GameObject room = Instantiate(roomPrefab.gameObject, transform);
            generatedObjects.Add(room);
            room.GetComponent<WaveFunctionCollapse>().generateRoom();
            SnappablePiece newPiece = room.GetComponent<SnappablePiece>();
            var newPieceConnectors = newPiece.getConnectors();
            while (newPieceConnectors.Count > 0)
            {
                Connector newConnector;
                // Need to fix this bit here to work with more than one available connector
                newConnector = newPiece.getConnectors()[0];
                newPieceConnectors.Remove(newConnector);
                // Figure out how to rotate piece to make connectors face each other
                Quaternion rotateAmount = getAmountToRotate(connector.getConnectorNormal(), newConnector.getConnectorNormal());
                if (rotateAmount.x != 0)
                {
                    Quaternion xFlipper = Quaternion.Euler(new(180, 180, 0));
                    rotateAmount *= xFlipper;

                }
                newPiece.gameObject.transform.rotation *= rotateAmount;
                // Find amount to move by to make connectors touch
                Vector3 moveAmount = connector.transform.position - newConnector.transform.position;
                newPiece.gameObject.transform.position += moveAmount;
                // This isn't actually doing what it should right now
                newConnector.setConnected(true);
                connector.setConnected(true);
                break;
            }
            populationQueue.Clear();
            populationQueue.Add(newPiece);
        }
        while (populationQueue.Count != 0 && generatedPieces <= settings.maxParts * 2)
        {
            populateConnections(populationQueue[0], targetPos);
            populationQueue.RemoveAt(0);
            generatedPieces++;
        }
    }

    private bool validateGeneratorSettings()
    {
        if (settings == null)
            return false;
        if (settings.tileset == null)
            return false;
        if (settings.minParts < 1) 
            return false;
        if (settings.maxParts < 1)
            return false;
        if (settings.tileset.startPieces.Count < 1)
            return false;
        if (settings.tileset.endPieces.Count < 1)
            return false;
        if (settings.tileset.standardPieces.Count < 1)
            return false;
        return true;
    }

    private SnappablePiece generateStartPoint()
    {
        GameObject startPiece = Instantiate(getRandomPiece(settings.tileset.startPieces), transform).gameObject;
        float rotation = getRandomRotation();
        startPiece.transform.Rotate(new Vector3(0, rotation, 0));
        generatedObjects.Add(startPiece);
        return startPiece.GetComponent<SnappablePiece>();
    }

    private bool populateConnections(SnappablePiece startPiece, Vector3 targetPosition)
    {
        bool gotCloser = false;
        List<Connector> connectors = startPiece.getConnectors();
        foreach (Connector connector in connectors)
        {
            if (connector.isConnected())
                continue;
            bool success = false;
            SnappablePiece newPiece;
            int loops = 0;
            var pieceList = new List<SnappablePiece>(settings.tileset.standardPieces);
            // THIS NEEDS REPLACED AS IT CAN CAUSE INFINITE LOOPS BTW SUPER REMEMBER TO FIX THIS PLEASE
            do
            {
                SnappablePiece pieceToGenerate = getRandomPieceWithLinearWeighting(getPieceListSortedByDistance(pieceList, connector, targetPosition));
                pieceList.Remove(pieceToGenerate);
                int optimalConnector = pieceToGenerate.getOptimalConnectorLayoutForDistance(connector, targetPosition).Item1;
                newPiece = Instantiate(pieceToGenerate, transform);
                List<Connector> newPieceConnectors = new(newPiece.getConnectors());
                // THIS ALSO NEEDS FIXED AS IT WILL JUST NOT WORK AT THE MOMENT IF MORE THAN ONE AVAILABLE CONNECTOR
                while (newPieceConnectors.Count > 0)
                {
                    Connector newConnector;
                    // Need to fix this bit here to work with more than one available connector
                    newConnector = newPiece.getConnectors()[optimalConnector];
                    newPieceConnectors.Remove(newConnector);
                    // Figure out how to rotate piece to make connectors face each other
                    Quaternion rotateAmount = getAmountToRotate(connector.getConnectorNormal(), newConnector.getConnectorNormal());
                    if (rotateAmount.x != 0)
                    {
                        Quaternion xFlipper = Quaternion.Euler(new(180, 180, 0));
                        rotateAmount *= xFlipper;

                    }
                    if (rotateAmount.z != 0)
                    {
                        rotateAmount.z = 0;
                    }
                    newPiece.gameObject.transform.rotation *= rotateAmount;
                    // Find amount to move by to make connectors touch
                    Vector3 moveAmount = connector.transform.position - newConnector.transform.position;
                    newPiece.gameObject.transform.position += moveAmount;
                    // This isn't actually doing what it should right now
                    bool collision = checkCollisionAgainstPreviousPieces(newPiece);
                    success = !collision;
                    if (collision)
                    {
                        Debug.Log("Collision");
                        break;
                    }
                    newConnector.setConnected(true);
                    connector.setConnected(true);
                    generatedObjects.Add(newPiece.gameObject);
                    break;
                }
                // Need to add actual error handling here
                if (!success)
                {
                    DestroyImmediate(newPiece.gameObject);
                    newPiece = null;
                }
                loops++;
                // REALLY NEED TO IMPLEMENT SOMETHING FOR IF NO SUCCESS
                // JUST LIKE ONE MORE COMMENT TO EXPRESS HOW IMPORTANT THIS IS
                if (loops > 10)
                {
                    break;
                }
            } while (!success && pieceList.Count > 0);
            if (newPiece)
            {
                float prevDistance = (targetPosition - startPiece.transform.position).sqrMagnitude;
                float newDistance = (targetPosition - newPiece.transform.position).sqrMagnitude;
                if (newDistance < prevDistance)
                    gotCloser = true;
                populationQueue.Add(newPiece);
            } else
            {
                newPiece = Instantiate<SnappablePiece>(settings.tileset.standardPieces[2], transform);
                var newPieceConnectors = newPiece.getConnectors();
                while (newPieceConnectors.Count > 0)
                {
                    Connector newConnector;
                    // Need to fix this bit here to work with more than one available connector
                    newConnector = newPiece.getConnectors()[0];
                    newPieceConnectors.Remove(newConnector);
                    // Figure out how to rotate piece to make connectors face each other
                    Quaternion rotateAmount = getAmountToRotate(connector.getConnectorNormal(), newConnector.getConnectorNormal());
                    if (rotateAmount.x != 0)
                    {
                        Quaternion xFlipper = Quaternion.Euler(new(180, 180, 0));
                        rotateAmount *= xFlipper;

                    }
                    newPiece.gameObject.transform.rotation *= rotateAmount;
                    // Find amount to move by to make connectors touch
                    Vector3 moveAmount = connector.transform.position - newConnector.transform.position;
                    newPiece.gameObject.transform.position += moveAmount;
                    // This isn't actually doing what it should right now
                    newConnector.setConnected(true);
                    connector.setConnected(true);
                    break;
                }
                populationQueue.Add(newPiece);
                generatedObjects.Add(newPiece.gameObject);
            }
        }
        return gotCloser;
    }

    private Quaternion getAmountToRotate(Vector3 firstConnectorNormal, Vector3 secondConnectorNormal)
    {
        Quaternion rotationAmount = Quaternion.FromToRotation(secondConnectorNormal, -firstConnectorNormal);
        return rotationAmount;
    }

    private bool checkCollisionAgainstPreviousPieces(SnappablePiece newPiece)
    {
        bool collision = false;
        for (int i = 0; i < generatedObjects.Count - 1; i++)
        {
            if (intersects(newPiece.GetComponent<BoxCollider>().center + newPiece.transform.position, newPiece.GetComponent<BoxCollider>().size * 0.98f, generatedObjects[i].GetComponent<BoxCollider>().center + generatedObjects[i].transform.position, generatedObjects[i].GetComponent<BoxCollider>().size * 0.98f))
            {
                collision = true;
                break;
            }
        }

        return collision;
    }

    // Unity's default intersection code considers two boxes that hug edges to be colliding, I don't want that
    // This collision check is somewhat bugged however, as it does not account for rotation. I need to speak to Gaz about how to fix this
    // Also because I'm feeding in 98% size box, y gets a lil messed up as well, need to fix that
    private bool intersects(Vector3 pos1, Vector3 extents1, Vector3 pos2, Vector3 extents2)
    {
        Vector3 min1 = pos1 - extents1;
        Vector3 max1 = pos1 + extents1;
        Vector3 min2 = pos2 - extents2;
        Vector3 max2 = pos2 + extents2;
        return min1.x < max2.x && max1.x > min2.x && min1.y < max2.y && max1.y > min2.y && min1.z < max2.z && max1.z > min2.z;
    }
    private SnappablePiece getRandomPiece(List<SnappablePiece> list)
    {
        SnappablePiece piece = null;
        int index = UnityEngine.Random.Range(0, list.Count);
        piece = list[index];
        return piece;
    }
    private SnappablePiece getRandomPieceWithLinearWeighting(List<SnappablePiece> list)
    {
        if (list.Count == 0)
            return null;
        SnappablePiece piece = null;
        List<int> weightingList = new();
        int weightingsTotal = 0;
        for (int i = 0; i < list.Count; i++)
        {
            weightingList.Add(i + weightingsTotal);
            weightingsTotal += i;
        }
        int randomValue = UnityEngine.Random.Range(0, weightingsTotal);
        for (int i = 0; i < weightingList.Count; i++)
        {
            if (randomValue <= weightingList[i])
            {
                piece = list[list.Count - 1 - i];
                break;
            }
        }
        if (piece == null)
            piece = list[0];
        return piece;
    }
    private float getRandomRotation()
    {
        int rotID = UnityEngine.Random.Range(0, 4);
        switch (rotID)
        {
            case 0:
                return 0;
            case 1:
                return 90;
            case 2:
                return 180;
            case 3:
                return 270;
        }
        return 0;
    }

    private List<Tuple<SnappablePiece, int, float>> getPieceListSortedByDistanceWithDetails(List<SnappablePiece> pieces, Connector lastConnector, Vector3 targetLocation)
    {
        List<Tuple<SnappablePiece, int, float>> sortedList = new();
        foreach (SnappablePiece piece in pieces)
        {
            Tuple<int, int, float> optimalLayout = piece.getOptimalConnectorLayoutForDistance(lastConnector, targetLocation);
            bool foundPosition = false;
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (optimalLayout.Item3 < sortedList[i].Item3)
                {
                    foundPosition = true;
                    sortedList.Insert(i, new(piece, optimalLayout.Item1, optimalLayout.Item3));
                    break;
                }
            }
            if (!foundPosition)
                sortedList.Add(new(piece, optimalLayout.Item1, optimalLayout.Item3));
        }
        return sortedList;
    }

    private List<SnappablePiece> getPieceListSortedByDistance(List<SnappablePiece> pieces, Connector lastConnector, Vector3 targetLocation)
    {
        List<Tuple<SnappablePiece, int, float>> sortedList = new();
        foreach (SnappablePiece piece in pieces)
        {
            Tuple<int, int, float> optimalLayout = piece.getOptimalConnectorLayoutForDistance(lastConnector, targetLocation);
            bool foundPosition = false;
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (optimalLayout.Item3 < sortedList[i].Item3)
                {
                    foundPosition = true;
                    sortedList.Insert(i, new(piece, optimalLayout.Item1, optimalLayout.Item3));
                    break;
                }
            }
            if (!foundPosition)
                sortedList.Add(new(piece, optimalLayout.Item1, optimalLayout.Item3));
        }
        List<SnappablePiece> pieceOnlyList = new();
        foreach (var item in sortedList)
        {
            pieceOnlyList.Add(item.Item1);
        }
        return pieceOnlyList;
    }

    public void deleteLevel()
    {
        // Destroy all generated level elements
        while (generatedObjects.Count > 0)
        {
            if (generatedObjects[0])
            {
                // Destroy immediate so it works in editor
                DestroyImmediate(generatedObjects[0]);
            }
            generatedObjects.RemoveAt(0);
        }
        while (populationQueue.Count > 0)
        {
            if (populationQueue[0])
            {
                DestroyImmediate(populationQueue[0]);
            }
            populationQueue.RemoveAt(0);
        }
    }
}

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var levelGenerator = (LevelGenerator)target;
        if (GUILayout.Button("Generate Level"))
        {
            levelGenerator.generateLevel();
        }
        if (GUILayout.Button("Delete Level"))
        {
            levelGenerator.deleteLevel();
        }
    }
}
