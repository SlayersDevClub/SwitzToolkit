using UnityEngine;
using System.Collections;

public class WindowSizeFitter : MonoBehaviour
{
    // Reference to the Body RectTransform whose children will determine the total height.
    // You can assign this in the Inspector, or if the child is always named "Body" you could
    // automatically find it in Awake.
    public RectTransform bodyRectTransform;

    // Fixed height to account for header and description (default: 96 pixels)
    public float headerAndDescriptionHeight = 96f;
    public float windowBorderSize = 8;

    private RectTransform windowRectTransform;

    private void Awake()
    {
        windowRectTransform = GetComponent<RectTransform>();

        // Optionally, if you want to automatically find the Body RectTransform by name:
        if (bodyRectTransform == null)
        {
            Transform bodyTransform = transform.Find("Body/Scroll View/Viewport/Content");
            if (bodyTransform != null)
            {
                bodyRectTransform = bodyTransform.GetComponent<RectTransform>();
            }
            else
            {
                Debug.LogError("Body RectTransform not found! Make sure there's a child named 'Body' or assign it manually.");
            }
        }
    }

    private void Start()
    {
        //ResizeWindow();
    }

    /// <summary>
    /// Computes the total height of all children of the Body RectTransform and adjusts
    /// the window's height to include the header/description area.
    /// </summary>
    public void ResizeWindow()
    {
        if (windowRectTransform == null)
            windowRectTransform = GetComponent<RectTransform>();
        if (bodyRectTransform == null)
        {
            Debug.LogWarning("Body RectTransform is not assigned.");
            return;
        }

        float childrenTotalHeight = 0f;

        // Loop through each child of the Body RectTransform
        foreach (Transform child in bodyRectTransform)
        {
            // Make sure the child has a RectTransform
            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect != null && child.gameObject.activeSelf)
            {
                childrenTotalHeight += childRect.rect.height;
            }
        }

        // Calculate total height: header/description height + children heights
        float totalHeight = headerAndDescriptionHeight + childrenTotalHeight + windowBorderSize;

        // Update the window's RectTransform sizeDelta to match the computed total height
        Vector2 newSize = windowRectTransform.sizeDelta;
        newSize.y = totalHeight;
        windowRectTransform.sizeDelta = newSize;
    }

//#if UNITY_EDITOR
//    // Optionally update in the editor when values change.
//    private void OnValidate()
//    {
//        if (windowRectTransform == null)
//            windowRectTransform = GetComponent<RectTransform>();
//        ResizeWindow();
//    }
//#endif

}
