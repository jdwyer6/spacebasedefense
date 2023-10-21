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
    public Color selectedColor;
    public GameObject[] buildingBlocks;

    private float[] initialBaseEnvironmentRotationOptions = new float[] {0, 90, -90, 180};
    private float initialBaseEnvironmentRotation;
    public GameObject[] baseEnvironments;

    private Color[] colors = new Color[]
    {
        new Color(0.8f, 0.7f, 0.7f), // Muted Pastel Red
        new Color(0.7f, 0.8f, 0.7f), // Muted Pastel Green
        new Color(0.8f, 0.75f, 0.7f), // Muted Pastel Orange
        new Color(0.75f, 0.7f, 0.8f),  // Muted Pastel Purple
        new Color(0.8f, 0.8f, 0.7f), // Muted Pastel Yellow
        new Color(0.7f, 0.8f, 0.8f), // Muted Pastel Turquoise
        new Color(0.8f, 0.7f, 0.75f),  // Muted Pastel Pink
        new Color(0.7f, 0.8f, 0.75f),  // Muted Pastel Mint
        new Color(0.7f, 0.75f, 0.8f),  // Muted Pastel Blue
        new Color(0.8f, 0.75f, 0.7f),  // Muted Pastel Peach
        new Color(0.7f, 0.75f, 0.8f),  // Muted Pastel Sky Blue
        new Color(0.8f, 0.7f, 0.7f), // Muted Pastel Coral
        new Color(0.75f, 0.8f, 0.7f),  // Muted Pastel Lime Green
        new Color(0.75f, 0.75f, 0.7f)    // Muted Pastel Beige
    };


    // Start is called before the first frame update
    void Start()
    {
        initialBaseEnvironmentRotation = initialBaseEnvironmentRotationOptions[GetRandomNum(0, initialBaseEnvironmentRotationOptions.Length)];
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
            ChangeBlockColor(block);
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
            ChangeBlockColor(block);
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
        foreach (var block in buildingBlocks)
        {
            SpriteRenderer renderer = block.GetComponent<SpriteRenderer>();
            if(renderer != null) {
                renderer.color = selectedColor;
            }
        }
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

    private void SetBase() {
        // Instantiate(baseEnvironments[GetRandomNum(0, baseEnvironments.Length)], new Vector2(0, 0), Euler.initialBaseEnvironmentRotation);
    }
}
