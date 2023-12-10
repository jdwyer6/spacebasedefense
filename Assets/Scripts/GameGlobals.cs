using UnityEngine;

public class GameGlobals : MonoBehaviour
{
    // Singleton instance
    public static GameGlobals Instance { get; private set; }

    // Global variable
    public bool globalMenuOpen = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Ensuring our singleton survives scene changes
        }
        else
        {
            Destroy(gameObject);
        }

        globalMenuOpen = false;
    }
}
