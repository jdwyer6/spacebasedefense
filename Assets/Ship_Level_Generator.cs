using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ship_Level_Generator : MonoBehaviour
{
    public GameObject[] rooms;
    private Vector2 currentSpawnPoint;
    public int numOfRoomsToGenerate = 20;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSettings();
        for (int i = 0; i < numOfRoomsToGenerate; i++)
        {
            GenerateLevel();
        }

    }

    public void GenerateLevel() {
        PlaceRoom(currentSpawnPoint);
    }

    private void InitializeSettings() {
        currentSpawnPoint = new Vector2(0, 0);
    }

    private void PlaceRoom(Vector2 currentSpawnPoint) {
        GameObject currentRoom = rooms[UnityEngine.Random.Range(0, rooms.Length)];
        var newRoom = Instantiate(currentRoom, currentSpawnPoint, Quaternion.identity);
        UpdateSpawnPoint(newRoom);
    }

    private void UpdateSpawnPoint(GameObject currentRoom) {
        Transform nextSpawnPoint = currentRoom.GetComponentsInChildren<Transform>().FirstOrDefault(child => child.CompareTag("Doorway"));

        if (nextSpawnPoint != null)
        {
            currentSpawnPoint = nextSpawnPoint.position;
            Debug.Log(nextSpawnPoint.position);
        }
    }
}
