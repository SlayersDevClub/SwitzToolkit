using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;

    void Awake()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
        canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        canvas = transform.root.GetComponent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
        originalPanelLocalPosition = rectTransform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out var localPointerPosition))
        {
            var offset = localPointerPosition - originalLocalPointerPosition;
            rectTransform.localPosition = originalPanelLocalPosition + new Vector3(offset.x, offset.y, originalPanelLocalPosition.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
}
