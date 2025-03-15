using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;
using TMPro; // Make sure to include this for TextMeshPro

public class ContextMenuManager : MonoBehaviour
{
    public RectTransform contextMenuPrefab; // Assign your context menu prefab here
    public GameObject buttonPrefab; // Assign your button prefab here
    public Canvas canvas; // Assign your Canvas here

    private RectTransform contextMenuInstance;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            ShowContextMenu();
        }

        // Check for left mouse button click to close the context menu
        if (contextMenuInstance != null && Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(contextMenuInstance as RectTransform, Input.mousePosition))
            {
                Destroy(contextMenuInstance.gameObject); // Close the context menu
            }
        }
    }


        private void ShowContextMenu()
        {
            if (contextMenuInstance != null)
            {
                Destroy(contextMenuInstance.gameObject);
            }

            Vector2 mousePosition = Input.mousePosition;
            GameObject clickedObject = GetClickedObject(mousePosition);

            if (clickedObject != null)
            {
                BaseContextMenu contextMenuScript = clickedObject.GetComponent<BaseContextMenu>();

                if (contextMenuScript != null)
                {
                    // Instantiate the context menu under the Canvas
                    contextMenuInstance = Instantiate(contextMenuPrefab, canvas.transform);


                    PopulateContextMenu(contextMenuScript);
                    contextMenuInstance.position = mousePosition;
                }
            }
        }

        private GameObject GetClickedObject(Vector2 mousePosition)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = mousePosition };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            
            return results.Count > 0 ? results[0].gameObject : null;
        }

    private void PopulateContextMenu(BaseContextMenu contextMenuSource)
    {
        // Clear existing buttons if needed
        foreach (Transform child in contextMenuInstance)
        {
            Destroy(child.gameObject);
        }

        // Populate the context menu with buttons for each action
        foreach (var method in contextMenuSource.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
        {
            if (method.ReturnType == typeof(void) && method.GetParameters().Length == 0)
            {
                // Exclude methods that are part of MonoBehaviour
                if (method.DeclaringType != typeof(MonoBehaviour))
                {
                    CreateButton(method.Name, contextMenuSource, method);
                    contextMenuInstance.sizeDelta += new Vector2(0,40);
                }
            }
        }
    }


    private void CreateButton(string buttonText, BaseContextMenu contextMenuSource, MethodInfo methodInfo)
    {
        GameObject buttonObject = Instantiate(buttonPrefab);
        buttonObject.transform.SetParent(contextMenuInstance, false);

        // Format the button text and set it
        string formattedText = FormatMethodName(buttonText);
        TextMeshProUGUI buttonTextComponent = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonTextComponent != null)
        {
            buttonTextComponent.text = formattedText;
        }

        // Directly invoke the method when the button is clicked and close the context menu
        buttonObject.GetComponent<Button>().onClick.AddListener(() => 
        {
            methodInfo.Invoke(contextMenuSource, null);
            Destroy(contextMenuInstance.gameObject); // Close the context menu
        });
    }

    private string FormatMethodName(string methodName)
    {
        return System.Text.RegularExpressions.Regex.Replace(methodName, "(?<=[a-z])(?=[A-Z])", " ");
    }


}
