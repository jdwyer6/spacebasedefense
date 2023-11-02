using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float initialBaseEnvironmentRotation;
    public GameObject[] baseEnvironments;
    public GameObject[] chestRooms;

    private Color[] colors = new Color[]
    {
        new Color(0.8f, 0.7f, 0.7f), // Pastel Red
        new Color(0.7f, 0.8f, 0.7f), // Pastel Green
        new Color(0.8f, 0.75f, 0.7f), // Pastel Orange
        new Color(0.75f, 0.7f, 0.8f),  // Pastel Purple
        new Color(0.8f, 0.8f, 0.7f), // Pastel Yellow
        new Color(0.7f, 0.8f, 0.8f), // Pastel Turquoise
        new Color(0.8f, 0.7f, 0.75f),  // Pastel Pink
        new Color(0.7f, 0.8f, 0.75f),  // Pastel Mint
        new Color(0.7f, 0.75f, 0.8f),  // Pastel Blue
        new Color(0.8f, 0.75f, 0.7f),  // Pastel Peach
        new Color(0.7f, 0.75f, 0.8f),  // Pastel Sky Blue
        new Color(0.8f, 0.7f, 0.7f), // Pastel Coral
        new Color(0.75f, 0.8f, 0.7f),  // Pastel Lime Green
        new Color(0.75f, 0.75f, 0.7f)    // Pastel Beige
    };

    private Transform[] specialRoomSpawnLocations;
    public GameObject specialRoomPrefab;

    private void Awake() {
        initialBaseEnvironmentRotation = initialBaseEnvironmentRotationOptions[GetRandomNum(0, initialBaseEnvironmentRotationOptions.Length)];
    }


    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex >= 3 ) {
            GenerateLevel();
        }
       
    }

    private void GenerateLevel() {
        SelectColor();
        CreateBaseDesign();
        MirrorDesignOnXAxis();
        SetBase();
        GenerateSpecialRooms();
    }

    private void CreateBaseDesign() {
        Vector2 currentPos = new Vector2(-.5f, 0);


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
        GameObject basePrefab = Instantiate(baseEnvironments[GetRandomNum(0, baseEnvironments.Length)], new Vector2(0, 0), Quaternion.Euler(0, 0, initialBaseEnvironmentRotation));
        Transform chestRoomSpawnPoint = basePrefab.transform.Find("ChestRoomSpawnPoint");
        Instantiate(chestRooms[GetRandomNum(0, chestRooms.Length)], chestRoomSpawnPoint.position, Quaternion.Euler(0, 0, initialBaseEnvironmentRotation));
        SetSpecialRoomSpawnLocations(basePrefab);
    }

    private void ResetEditorPreferences() {
        Camera mainCam = Camera.main;
        mainCam.backgroundColor = new Color(255f/255f, 73f/255f, 73f/255f, 1f);
    }

    private void GenerateSpecialRooms() {
        int numOfRoomsToGenerate = GetRandomNum(1, 3);

        for (int i = 0; i < numOfRoomsToGenerate; i++)
        {
            GameObject specialRoomItem = GetComponent<Data>().specialRoomItems[GetRandomNum(0, GetComponent<Data>().specialRoomItems.Length)];
            Transform specialRoomSpawnPos = specialRoomSpawnLocations[GetRandomNum(0, specialRoomSpawnLocations.Length)];
            float randomRotation = initialBaseEnvironmentRotationOptions[GetRandomNum(0, initialBaseEnvironmentRotationOptions.Length)];
            var room = Instantiate(specialRoomPrefab, specialRoomSpawnPos.position, Quaternion.Euler(0, 0, randomRotation));
            Transform itemSpawnLocation = room.transform.Find("ItemSpawnLocation");
            Instantiate(specialRoomItem, itemSpawnLocation.position, Quaternion.identity);
        }
    }

    private void SetSpecialRoomSpawnLocations(GameObject basePrefab) {
        List<Transform> foundSpawnLocations = new List<Transform>();
        foreach (Transform child in basePrefab.transform) {
            if (child.CompareTag("SpecialRoomSpawnLocation")) {
                foundSpawnLocations.Add(child);
            }
        }
        specialRoomSpawnLocations = foundSpawnLocations.ToArray();
    }
}
