using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class ResizableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum ResizeZone
    {
        None,
        Top,
        Bottom,
        Left,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    [Header("Resizable Zone Parent")]
    [Tooltip("Transform that has all the zones setup as its children. Make sure each one has ResizeZone component on it.")]
    public Transform resizableZones;
    RectTransform windowRectTransform;
    RectTransform topZone;
    RectTransform bottomZone;
    RectTransform leftZone;
    RectTransform rightZone;
    RectTransform topLeftZone;
    RectTransform topRightZone;
    RectTransform bottomLeftZone;
    RectTransform bottomRightZone;

    float dragSensitivity = 1.0f;

    private ResizeZone currentZone = ResizeZone.None;
    private Vector2 originalMousePosition;
    WindowBase SSWindowBase;

    [Space]
    [Header("Window Size Fit")]
    [Tooltip("Rect whom content is a child of")]
    public RectTransform bodyRectTransform;
    [Tooltip("Combined height of the header and description panel")]
    public float headerAndDescriptionHeight = 96f;
    [Tooltip("Border size")]
    public float windowBorderSize = 8;
    [Tooltip("Restrict the minimum window size to not be smaller (total height) than the content.")]
    public bool ForceContentFit = false;

    Vector2 previousSize, originalSize;
    float minHeight;

    private void Start()
    {
        if (windowRectTransform == null)
        {
            windowRectTransform = GetComponent<RectTransform>();
        }
        //store original size 
        originalSize = windowRectTransform.sizeDelta;
        SSWindowBase = GetComponent<WindowBase>();

        // Find the ResizableZones parent object and get its children
        resizableZones = transform.Find("ResizableZones");
        if (resizableZones != null)
        {
            topZone = resizableZones.Find("Top").GetComponent<RectTransform>();
            bottomZone = resizableZones.Find("Bottom").GetComponent<RectTransform>();
            leftZone = resizableZones.Find("Left").GetComponent<RectTransform>();
            rightZone = resizableZones.Find("Right").GetComponent<RectTransform>();
            topLeftZone = resizableZones.Find("TopLeft").GetComponent<RectTransform>();
            topRightZone = resizableZones.Find("TopRight").GetComponent<RectTransform>();
            bottomLeftZone = resizableZones.Find("BottomLeft").GetComponent<RectTransform>();
            bottomRightZone = resizableZones.Find("BottomRight").GetComponent<RectTransform>();
        }

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

    //This is only used for manually resizing the window to fit its children from the inspector. It can also be called publicly if needed.
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

    public float BodyContentHeight()
    {
        float totalHeight = 0;

                // Loop through each child of the Body RectTransform
        foreach (Transform child in bodyRectTransform)
        {
            // Make sure the child has a RectTransform
            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect != null && child.gameObject.activeSelf)
            {
                totalHeight += childRect.rect.height;
            }
        }
        totalHeight += headerAndDescriptionHeight + windowBorderSize;
        return totalHeight;
    }

    public void MinimizeWindow(bool toMinimize)
    {
        //toMinimize =! toMinimize;
        if(toMinimize)
        {
            SetPivotForResizeZone(ResizeZone.Bottom);
            previousSize = windowRectTransform.sizeDelta;
            //float totalHeight = headerAndDescriptionHeight + windowBorderSize;
            float totalHeight = 64;
            windowRectTransform.DOSizeDelta(new Vector2(previousSize.x, totalHeight), .25f);
            resizableZones.gameObject.SetActive(false);
        }
        else
        {
            SetPivotForResizeZone(ResizeZone.Bottom);
            windowRectTransform.DOSizeDelta(previousSize, .25f);
            resizableZones.gameObject.SetActive(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentZone = GetResizeZone(eventData.position);
        originalMousePosition = eventData.position;
        SetPivotForResizeZone(currentZone);

        //prevent the user from moving the window while scaling it
        if(currentZone != ResizeZone.None) SSWindowBase.enabled = false;

        //Let's prevent the window from being smaller than its content when resizing the window.
        if(ForceContentFit)
            minHeight = BodyContentHeight();
        else
            minHeight = headerAndDescriptionHeight + windowBorderSize;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentZone == ResizeZone.None) return;

        Vector2 delta = eventData.position - originalMousePosition;

        switch (currentZone)
        {
            case ResizeZone.Top:
                ResizeTop(delta);
                break;
            case ResizeZone.Bottom:
                ResizeBottom(delta);
                break;
            case ResizeZone.Left:
                ResizeLeft(delta);
                break;
            case ResizeZone.Right:
                ResizeRight(delta);
                break;
            case ResizeZone.TopLeft:
                ResizeTopLeft(delta);
                break;
            case ResizeZone.TopRight:
                ResizeTopRight(delta);
                break;
            case ResizeZone.BottomLeft:
                ResizeBottomLeft(delta);
                break;
            case ResizeZone.BottomRight:
                ResizeBottomRight(delta);
                break;
        }

        originalMousePosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //let the user maniuplate the window again
        SSWindowBase.enabled = true;

        // Reset the current zone
        currentZone = ResizeZone.None;

        // Optionally reset the pivot to the default (center) when dragging ends
        // To avoid jumping, calculate the offset before resetting the pivot
        Vector2 oldPivot = windowRectTransform.pivot;
        Vector2 newPivot = new Vector2(0.5f, 0.5f); // Default pivot

        if (oldPivot != newPivot)
        {
            Vector2 size = windowRectTransform.sizeDelta;
            Vector2 position = windowRectTransform.anchoredPosition;

            // Calculate the offset to maintain the position
            Vector2 pivotOffset = newPivot - oldPivot;
            position += new Vector2(pivotOffset.x * size.x, pivotOffset.y * size.y);

            // Set the new pivot and position
            windowRectTransform.pivot = newPivot;
            windowRectTransform.anchoredPosition = position;
        }
    }

    private ResizeZone GetResizeZone(Vector2 mousePosition)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(topZone, mousePosition, null))
            return ResizeZone.Top;
        if (RectTransformUtility.RectangleContainsScreenPoint(bottomZone, mousePosition, null))
            return ResizeZone.Bottom;
        if (RectTransformUtility.RectangleContainsScreenPoint(leftZone, mousePosition, null))
            return ResizeZone.Left;
        if (RectTransformUtility.RectangleContainsScreenPoint(rightZone, mousePosition, null))
            return ResizeZone.Right;
        if (RectTransformUtility.RectangleContainsScreenPoint(topLeftZone, mousePosition, null))
            return ResizeZone.TopLeft;
        if (RectTransformUtility.RectangleContainsScreenPoint(topRightZone, mousePosition, null))
            return ResizeZone.TopRight;
        if (RectTransformUtility.RectangleContainsScreenPoint(bottomLeftZone, mousePosition, null))
            return ResizeZone.BottomLeft;
        if (RectTransformUtility.RectangleContainsScreenPoint(bottomRightZone, mousePosition, null))
            return ResizeZone.BottomRight;

        return ResizeZone.None;
    }

    private void SetPivotForResizeZone(ResizeZone zone)
{
    Vector2 oldPivot = windowRectTransform.pivot;
    Vector2 newPivot = Vector2.zero;

    switch (zone)
    {
        case ResizeZone.Top:
            newPivot = new Vector2(0.5f, 0);
            break;
        case ResizeZone.Bottom:
            newPivot = new Vector2(0.5f, 1);
            break;
        case ResizeZone.Left:
            newPivot = new Vector2(1, 0.5f);
            break;
        case ResizeZone.Right:
            newPivot = new Vector2(0, 0.5f);
            break;
        case ResizeZone.TopLeft:
            newPivot = new Vector2(1, 0);
            break;
        case ResizeZone.TopRight:
            newPivot = new Vector2(0, 0);
            break;
        case ResizeZone.BottomLeft:
            newPivot = new Vector2(1, 1);
            break;
        case ResizeZone.BottomRight:
            newPivot = new Vector2(0, 1);
            break;
    }

    // Calculate the offset based on the pivot change
    Vector2 size = windowRectTransform.sizeDelta;
    Vector2 position = windowRectTransform.anchoredPosition;

    // Calculate the offset to maintain the position
    Vector2 pivotOffset = newPivot - oldPivot;
    position += new Vector2(pivotOffset.x * size.x, pivotOffset.y * size.y);

    // Set the new pivot and position
    windowRectTransform.pivot = newPivot;
    windowRectTransform.anchoredPosition = position;
    }

    Vector2 ClampWindowSize(Vector2 vector)
    {
        float clampedX = Mathf.Clamp(vector.x, 300, 1200);
        float clampedY = Mathf.Clamp(vector.y, minHeight, 1200);
        return new Vector2(clampedX, clampedY);
    }

    private void ResizeTop(Vector2 delta)
    {
        windowRectTransform.sizeDelta += new Vector2(0, delta.y * dragSensitivity);
        windowRectTransform.sizeDelta = ClampWindowSize(windowRectTransform.sizeDelta);
    }

    private void ResizeBottom(Vector2 delta)
    {
        windowRectTransform.sizeDelta -= new Vector2(0, delta.y * dragSensitivity);
        windowRectTransform.sizeDelta = ClampWindowSize(windowRectTransform.sizeDelta);
    }

    private void ResizeLeft(Vector2 delta)
    {
        windowRectTransform.sizeDelta -= new Vector2(delta.x * dragSensitivity, 0);
        windowRectTransform.sizeDelta = ClampWindowSize(windowRectTransform.sizeDelta);
    }

    private void ResizeRight(Vector2 delta)
    {
        windowRectTransform.sizeDelta += new Vector2(delta.x * dragSensitivity, 0);
        windowRectTransform.sizeDelta = ClampWindowSize(windowRectTransform.sizeDelta);
    }

    private void ResizeTopLeft(Vector2 delta)
    {
        ResizeTop(delta);
        ResizeLeft(delta);
    }

    private void ResizeTopRight(Vector2 delta)
    {
        ResizeTop(delta);
        ResizeRight(delta);
    }

    private void ResizeBottomLeft(Vector2 delta)
    {
        ResizeBottom(delta);
        ResizeLeft(delta);
    }

    private void ResizeBottomRight(Vector2 delta)
    {
        ResizeBottom(delta);
        ResizeRight(delta);
    }
}
