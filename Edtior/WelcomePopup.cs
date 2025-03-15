using UnityEditor;
using UnityEngine;

namespace SwitzToolkit{
    [InitializeOnLoad]
    public static class WelcomePopup
    {
        private const string PREF_KEY = "MyPackage_WelcomePopupShown";
        private const string PREF_SHOW_AGAIN = "MyPackage_ShowAgainOnStartup";

        static WelcomePopup()
        {
            EditorApplication.delayCall += ShowWelcomePopup;
        }

        private static void ShowWelcomePopup()
        {
            // Check if the user has disabled the welcome popup.
            bool showAgain = EditorPrefs.GetBool(PREF_SHOW_AGAIN, true);
            if (!showAgain)
                return;

            // Show the custom welcome window.
            MyPackageWelcomeWindow.ShowWindow();
        }
    }

    public class MyPackageWelcomeWindow : EditorWindow
    {
        private bool showAgain = true;

        public static void ShowWindow()
        {
            var window = GetWindow<MyPackageWelcomeWindow>("Welcome to My Package!");
            window.minSize = new Vector2(300, 150);
            window.ShowUtility(); // Utility window appears without taking focus from your workflow.
        }

        private void OnEnable()
        {
            // Load the preference for showing the popup.
            showAgain = EditorPrefs.GetBool("MyPackage_ShowAgainOnStartup", true);
        }

        private void OnGUI()
        {
            GUILayout.Label("Welcome to My Package!", EditorStyles.boldLabel);
            GUILayout.Space(10);
            GUILayout.Label("Thank you for installing My Package. Would you like to view sample content to help you get started?", EditorStyles.wordWrappedLabel);

            GUILayout.Space(20);
            if (GUILayout.Button("Open Samples"))
            {
                // Open your samples window or folder.
                MyPackageSamplesWindow.ShowWindow();
                Close();
            }

            GUILayout.Space(10);
            // Checkbox to control whether the popup should show on startup.
            showAgain = EditorGUILayout.Toggle("Show on startup", showAgain);

            GUILayout.Space(10);
            if (GUILayout.Button("Close"))
            {
                // Save the preference.
                EditorPrefs.SetBool("MyPackage_ShowAgainOnStartup", showAgain);
                Close();
            }
        }
    }

    public class MyPackageSamplesWindow : EditorWindow
    {
        [MenuItem("Window/My Package Samples")]
        public static void ShowWindow()
        {
            var window = GetWindow<MyPackageSamplesWindow>("My Package Samples");
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
                // Adjust the path as needed.
                EditorUtility.RevealInFinder("Assets/YourSamplesFolder");
            }
        }
    }
}
