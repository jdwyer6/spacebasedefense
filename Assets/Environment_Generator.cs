using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Generator : MonoBehaviour
{
    public int numberOfTimesToPlaceBlocks = 10;
    public GameObject blockPrefab;
    private float[] possibleXValues = new float[] {0, -4};
    private float[] possibleYValues = new float[] {4, -4};
    private List<Vector2> blockPositions = new List<Vector2>();
    private Color selectedColor;

    private Color[] colors = new Color[]
    {
        new Color(1.0f, 0.2f, 0.2f), // Bright Red
        new Color(0.2f, 1.0f, 0.2f), // Bright Green
        new Color(0.2f, 0.2f, 1.0f), // Bright Blue
        new Color(1.0f, 0.5f, 0.0f), // Orange
        new Color(0.6f, 0.0f, 0.8f), // Purple
        new Color(1.0f, 1.0f, 0.2f), // Yellow
        new Color(0.0f, 0.8f, 0.8f), // Turquoise
        new Color(0.8f, 0.4f, 0.0f), // Dark Orange
        new Color(0.9f, 0.0f, 0.5f), // Pink
        new Color(0.4f, 0.6f, 0.0f), // Olive Green
        new Color(0.7f, 0.3f, 0.7f), // Magenta
        new Color(0.3f, 0.3f, 0.9f), // Royal Blue
        new Color(0.8f, 0.8f, 0.0f), // Gold
        new Color(0.0f, 0.5f, 0.5f), // Teal
        new Color(0.5f, 0.2f, 0.1f), // Brown
        new Color(0.9f, 0.6f, 0.7f), // Light Pink
        new Color(0.2f, 0.7f, 0.3f), // Sea Green
        new Color(0.6f, 0.4f, 0.8f), // Lavender
        new Color(1.0f, 0.7f, 0.3f), // Peach
        new Color(0.4f, 0.8f, 0.4f)  // Lime Green
    };


    // Start is called before the first frame update
    void Start()
    {
        //color
        GenerateLevel();
    }

    private void GenerateLevel() {
        SelectColor();
        CreateBaseDesign();
        MirrorDesignOnXAxis();
    }

    private void CreateBaseDesign() {
        Vector2 currentPos = new Vector2(0, 0);


        for (int i = 0; i < numberOfTimesToPlaceBlocks; i++)
        {
            var block = Instantiate(blockPrefab, currentPos, Quaternion.identity);
            // ChangeBlockColor(block);
            blockPositions.Add(currentPos);
            currentPos = GetNewPos(currentPos);
        }
    }

    
    private void MirrorDesignOnXAxis() 
    {
        foreach (Vector2 pos in blockPositions)
        {
            Vector2 mirroredPos = new Vector2(-pos.x, pos.y); // Mirroring on the X-axis
            var block = Instantiate(blockPrefab, mirroredPos, Quaternion.identity);
            // ChangeBlockColor(block);
        }
    }

    private int GetRandomNum(int min, int max) {
        return UnityEngine.Random.Range(min, max);
    }

    private Vector2 GetNewPos(Vector2 currentPos) {
        float newXValue = possibleXValues[GetRandomNum(0, 2)];
        float newYValue = possibleYValues[GetRandomNum(0, 2)];
        return new Vector2(currentPos.x + newXValue, currentPos.y + newYValue);
    }

    private void SelectColor() {
        selectedColor = colors[GetRandomNum(0, colors.Length)];
    }

    private void ChangeBlockColor(GameObject block) {
        List<SpriteRenderer> foundSpriteRenderers = new List<SpriteRenderer>();

        foreach (Transform child in block.transform)
        {
            if (child.name == "Block")
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    foundSpriteRenderers.Add(spriteRenderer);
                }
            }
        }

        foreach (var sr in foundSpriteRenderers)
        {
            sr.color = selectedColor;
        }
    }
}
