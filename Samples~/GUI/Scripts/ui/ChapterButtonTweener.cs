using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChapterButtonTweener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rect;
    public Vector2 maxSize;
    Vector2 minSize, newSize;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        minSize = rect.sizeDelta;
        newSize = minSize + maxSize;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOKill();

        rect.DOSizeDelta(newSize, .2f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOSizeDelta(minSize, 1f);
    }
}
