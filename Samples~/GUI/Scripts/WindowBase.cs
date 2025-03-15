using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI.Extensions;
using TMPro;


[RequireComponent(typeof(RectTransform))]
public class WindowBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string windowName
    {
        get
        {
            return _windowName;
        }
        set 
        {
            _windowName = value;
            titleText.SetText(value);
            titleText.ForceMeshUpdate();
        }
    }
    private string _windowName = "New Window";
    public TextMeshProUGUI titleText;
    public bool isDraggable = true;
    public static bool ResetCoords = false;
    private Vector3 m_originalCoods = Vector3.zero;
    private Canvas m_canvas;
    private RectTransform m_canvasRectTransform;
    public int KeepWindowInCanvas = 5;
    RectTransform RootTransform = null;
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    Vector2 originalSizeDelta;
    public UIDraggableElement elementSocketType = UIDraggableElement.Window;

    [HideInInspector]public bool _isDragging = false;
    [HideInInspector]public bool IsDocked = false;

    

    // Use this for initialization
    public virtual void Start()
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
                var potentialZone = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<DockingZone>();
                if (potentialZone != null)
                {
                    zone = potentialZone;
                }
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
            if(zone != null && elementSocketType == zone.elementSocketType)
            {
                zone.ToggleHighlight(true);
            }

        }
    }
    Vector2 currentMousePosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isDraggable) 
            return;
        if (eventData.pointerCurrentRaycast.gameObject == null)
            return;

        if (IsDocked)
        {
            currentMousePosition = eventData.position;
            Undock();
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
        

        if (zone != null && elementSocketType == zone.elementSocketType)       
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

    public virtual void Dock(DockingZone dockingZone)
    {
        if (dockingZone == null)
            return;

        // Save current parent and size if not already docked.
        if (!IsDocked)
        {
            originalParent = RootTransform.parent;
        }

        originalSizeDelta = RootTransform.sizeDelta;

        // Re-parent the window to the docking zone.
        if(!dockingZone.transform.Find("Scroll View/Viewport/Content"))
            RootTransform.SetParent(dockingZone.transform, false);
        else
            RootTransform.SetParent(dockingZone.transform.Find("Scroll View/Viewport/Content"), false);

        IsDocked = true;
    }

    public virtual void Undock()
    {
        if (!IsDocked)
            return;

        RootTransform.SetParent(originalParent, false);
        //RootTransform.position = ScreenToCanvas(Input.mousePosition);
        RootTransform.position = currentMousePosition - new Vector2(0, originalSizeDelta.y /2.35f);
        RootTransform.sizeDelta = originalSizeDelta;


        IsDocked = false;
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
}
