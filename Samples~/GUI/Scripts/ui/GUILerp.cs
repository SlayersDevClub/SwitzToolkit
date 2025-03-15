using DG.Tweening;
using UnityEngine;

public class GUILerp : MonoBehaviour
{
    public bool MoveAnchoredPosition = false;
    public float speed = 0.25f;
    public Vector2 startPosition, endPosition;
    RectTransform rect;

    public Ease easeOption =  Ease.Linear; // Directly using DOTween's Ease enum

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void ShowWindow(bool b)
    {
        if (!b)
        {
            if(!MoveAnchoredPosition)
                rect.DOLocalMove(startPosition, speed).SetEase(easeOption);
            else
                rect.DOAnchorPos(startPosition, speed).SetEase(easeOption);
        }
        else
        {
            if(!MoveAnchoredPosition)
                rect.DOLocalMove(endPosition, speed).SetEase(easeOption);
            else
                rect.DOAnchorPos(endPosition, speed).SetEase(easeOption);        
        }

    }
}
