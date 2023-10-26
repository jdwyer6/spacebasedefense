#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class PlayerPrefsManager : EditorWindow
{
    [MenuItem("Window/PlayerPrefs Viewer")]
    static void OpenWindow()
    {
        PlayerPrefsViewer window = (PlayerPrefsViewer)GetWindow(typeof(PlayerPrefsViewer));
        window.Show();
    }

    void OnGUI()
    {
        // Show a simple text area with all PlayerPrefs keys/values
        foreach (string key in PlayerPrefs.GetString("PlayerPrefsKeys", "").Split(';'))
        {
            if (string.IsNullOrEmpty(key)) continue;
            string value = PlayerPrefs.GetString(key, "Key does not exist");
            EditorGUILayout.LabelField(key, value);
        }
    }
}
#endif
