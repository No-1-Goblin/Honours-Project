using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GeneratorSettings settings;
    private List<GameObject> generatedObjects = new();
    
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
        populateConnections(startPiece, settings.maxParts);
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
        GameObject startPiece = Instantiate(getRandomPiece(settings.tileset.startPieces)).gameObject;
        float rotation = getRandomRotation();
        startPiece.transform.Rotate(new Vector3(0, rotation, 0));
        generatedObjects.Add(startPiece);
        return startPiece.GetComponent<SnappablePiece>();
    }

    private void populateConnections(SnappablePiece startPiece, int piecesLeft)
    {
        if (piecesLeft <= 0)
            return;
        List<Connector> connectors = startPiece.getConnectors();
        foreach (Connector connector in connectors)
        {
            if (connector.isConnected())
                continue;
            bool success = false;
            SnappablePiece newPiece;
            int loops = 0;
            do
            {
                // THIS NEEDS REPLACED AS IT CAN CAUSE INFINITE LOOPS BTW SUPER REMEMBER TO FIX THIS PLEASE
                newPiece = Instantiate(getRandomPiece(settings.tileset.standardPieces));
                List<Connector> newPieceConnectors = new(newPiece.getConnectors());
                // I should also randomise this tbh
                while (newPieceConnectors.Count > 0)
                {
                    Connector newConnector = newPieceConnectors[Random.Range(0, newPieceConnectors.Count)];
                    newPieceConnectors.Remove(newConnector);
                    for (int i = 0; i < 4; i++)
                    {
                        // Find amount to move by to make connectors touch
                        Vector3 moveAmount = connector.transform.position - newConnector.transform.position;
                        newPiece.gameObject.transform.position += moveAmount;
                        Debug.Log(newPiece.boxCollider.bounds);
                        Debug.Log(startPiece.boxCollider.bounds);
                        if (!intersects(newPiece.boxCollider.transform.position, newPiece.boxCollider.size * 0.49f, startPiece.boxCollider.transform.position, startPiece.boxCollider.size * 0.49f))
                        {
                            success = true;
                            newConnector.setConnected(true);
                            connector.setConnected(true);
                            generatedObjects.Add(newPiece.gameObject);
                            break;
                        } else
                        {
                            Debug.Log("Failed to place");
                        }
                        newPiece.gameObject.transform.Rotate(0, 90, 0);
                    }
                    if (success)
                    {
                        break;
                    }
                }
                if (!success)
                {
                    return;
                    DestroyImmediate(newPiece.gameObject);
                }
                loops++;
                // REALLY NEED TO IMPLEMENT SOMETHING FOR IF NO SUCCESS
                // JUST LIKE ONE MORE COMMENT TO EXPRESS HOW IMPORTANT THIS IS
                if (loops > 10)
                {
                    break;
                }
            } while (!success);
            piecesLeft--;
            if (newPiece)
            {
                populateConnections(newPiece, piecesLeft);
            }
        }
        return;
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
        int index = Random.Range(0, list.Count);
        piece = list[index];
        return piece;
    }
    private float getRandomRotation()
    {
        int rotID = Random.Range(0, 4);
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
