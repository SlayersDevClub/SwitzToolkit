using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI.Extensions;
using Unity.VisualScripting;


[RequireComponent(typeof(RectTransform))]
public class UIWindowBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isDraggable = true;
    public static bool ResetCoords = false;
    private Vector3 m_originalCoods = Vector3.zero;
    private Canvas m_canvas;
    private RectTransform m_canvasRectTransform;

    [Tooltip("Number of pixels of the window that must stay inside the canvas view.")]
    public int KeepWindowInCanvas = 5;

    [Tooltip("The transform that is moved when dragging, can be left empty in which case its own transform is used.")]
    public RectTransform RootTransform = null;

    // Fields for docking functionality
    private Transform originalParent;

    private CanvasGroup canvasGroup;

    [HideInInspector]public bool _isDragging = false;
    [HideInInspector]public bool IsDocked = false;
    

    // Use this for initialization
    void Start()
    {
        if (RootTransform == null)
        {
            RootTransform = GetComponent<RectTransform>();
        }

        m_originalCoods = RootTransform.position;
        m_canvas = GetComponentInParent<Canvas>();
        m_canvasRectTransform = m_canvas.GetComponent<RectTransform>();

        // Save the original parent and size so we can restore when undocking.
        originalParent = RootTransform.parent;

        // Get or add CanvasGroup for controlling raycast blocking
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void ShowWindow()
    {
        gameObject.SetActive(true);
        RootTransform.localScale = Vector3.zero;
        RootTransform.DOScale(Vector3.one, .25f).SetEase(Ease.InOutSine);
    }

    public void CloseWindow()
    {
        RootTransform.DOScale(Vector3.zero, .25f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        }).SetEase(Ease.InOutSine);
    }

    void Update()
    {
        if (ResetCoords)
            resetCoordinatePosition();

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDragging)
        {
            var delta = ScreenToCanvas(eventData.position) - ScreenToCanvas(eventData.position - eventData.delta);
            RootTransform.localPosition += delta;

            DockingZone zone = null;
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                zone = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<DockingZone>();
            }
            if (zone == null)
            {
                foreach (GameObject go in eventData.hovered)
                {
                    zone = go.GetComponent<DockingZone>();
                    if (zone != null)
                        break;
                }
            }
            if(zone != null)
            {
                zone.ToggleHighlight(true);
            }

        }
    }


    // When starting a drag, if the window is docked then undock it.
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isDraggable) 
            return;
        if (eventData.pointerCurrentRaycast.gameObject == null)
            return;

        // If the window is docked, undock it so that it can be moved freely.
        if (IsDocked)
        {
            Undock(eventData);
        }

        canvasGroup.blocksRaycasts = false;

        // Make sure we are dragging the correct object.
        if (eventData.pointerCurrentRaycast.gameObject.name == name || eventData.pointerCurrentRaycast.gameObject.name == "DockingZone")
        {
            _isDragging = true;
            canvasGroup.DOFade(0.25f, .25f);
        }

        // Bring this window to the front.
        transform.SetAsLastSibling();

        CursorChanger.Instance.SetCursor(CursorType.Move);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

        canvasGroup.blocksRaycasts = true;

        canvasGroup.DOFade(1, .25f);

        DockingZone zone = null;
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            zone = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<DockingZone>();
        }
        if (zone == null)
        {
            foreach (GameObject go in eventData.hovered)
            {
                zone = go.GetComponentInParent<DockingZone>();
                if (zone != null)
                    break;
            }
        }
        

        if (zone != null)
        {
            Dock(zone);
            zone.ToggleHighlight(false);
        }

        CursorChanger.Instance.SetCursor(CursorType.Normal);

    }

    void resetCoordinatePosition()
    {
        RootTransform.position = m_originalCoods;
        ResetCoords = false;
    }

private Vector3 ScreenToCanvas(Vector3 screenPosition)
{
    Vector2 localPoint;
    // Use the appropriate camera based on the render mode.
    Camera cam = (m_canvas.renderMode == RenderMode.ScreenSpaceOverlay || m_canvas.worldCamera == null) 
                    ? null 
                    : m_canvas.worldCamera;

    // Convert the screen position to a local point in the canvas using Unity’s utility method.
    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvasRectTransform, screenPosition, cam, out localPoint))
    {
        // If you still need to clamp the position so that a window stays within the canvas bounds,
        // calculate the min and max values based on the actual rect size.
        Vector2 canvasSize = m_canvasRectTransform.rect.size;
        Vector2 min = -Vector2.Scale(canvasSize, m_canvasRectTransform.pivot);
        Vector2 max = Vector2.Scale(canvasSize, (Vector2.one - m_canvasRectTransform.pivot));
        
        localPoint.x = Mathf.Clamp(localPoint.x, min.x + KeepWindowInCanvas, max.x - KeepWindowInCanvas);
        localPoint.y = Mathf.Clamp(localPoint.y, min.y + KeepWindowInCanvas, max.y - KeepWindowInCanvas);
        
        return new Vector3(localPoint.x, localPoint.y, screenPosition.z);
    }
    else
    {
        throw new Exception("Failed to convert screen point to canvas local point.");
    }
}

    /// <summary>
    /// Docks the window into the provided docking zone.
    /// This re-parents the window, sets it to a docked size (e.g. a small tab graphic),
    /// and marks it as docked.
    /// </summary>
    /// <param name="dockingZone">The DockingZone to dock into.</param>
    public virtual void Dock(DockingZone dockingZone)
    {
        if (dockingZone == null)
            return;

        // Save current parent and size if not already docked.
        if (!IsDocked)
        {
            originalParent = RootTransform.parent;
        }

        // Re-parent the window to the docking zone.
        RootTransform.SetParent(dockingZone.transform, false);

        IsDocked = true;
    }

    /// <summary>
    /// Undocks the window – restoring its original parent.
    /// </summary>
    public virtual void Undock(PointerEventData data)
    {
        if (!IsDocked)
            return;

        RootTransform.SetParent(originalParent, false);
        //RootTransform.position = ScreenToCanvas(data.position) - new Vector3(RootTransform.sizeDelta.x/2, RootTransform.sizeDelta.y-16,0);
        RootTransform.position = data.position - new Vector2(RootTransform.sizeDelta.x / 2, RootTransform.sizeDelta.y - 16);
        IsDocked = false;
    }
}
