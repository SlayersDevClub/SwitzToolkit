using UnityEditor;
using UnityEngine;

public class WelcomeWindow : EditorWindow
{
    [MenuItem("Window/My Package Samples")]
    public static void ShowWindow()
    {
        var window = GetWindow<WelcomeWindow>("My Package Samples");
        window.minSize = new Vector2(300, 150);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("My Package Samples", EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.Label("Here you can find sample scenes and scripts to help you get started.", EditorStyles.wordWrappedLabel);

        GUILayout.Space(20);
        if (GUILayout.Button("Open Samples Folder"))
        {
            // Replace with the actual path to your samples folder, if needed.
            EditorUtility.RevealInFinder("Assets/YourSamplesFolder");
        }
    }
}
