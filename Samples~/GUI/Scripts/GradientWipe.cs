using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GradientWipe : MonoBehaviour
{
    public RectTransform gradientTransform; // The gradient Image (child of the mask)
    public RectTransform maskTransform; // The masked parent (health bar background)
    public float wipeDuration = 1.5f; // How long the wipe takes
    public Ease wipeEase = Ease.Linear; // The easing style

    private Vector2 startPosition;
    private Vector2 endPosition;

    void Start()
    {
        if (gradientTransform == null || maskTransform == null)
        {
            Debug.LogError("Assign Gradient and Mask RectTransforms in the inspector!");
            return;
        }

        // Set up start and end positions based on the mask's width
        float maskWidth = maskTransform.rect.width;
        startPosition = new Vector2(-maskWidth, gradientTransform.anchoredPosition.y);
        endPosition = new Vector2(maskWidth, gradientTransform.anchoredPosition.y);

        // Place the gradient at the start position
        gradientTransform.anchoredPosition = startPosition;

        StartGradientWipe();
    }

    public void StartGradientWipe()
    {
        // Reset position and animate across
        gradientTransform.anchoredPosition = startPosition;
        gradientTransform.DOAnchorPos(endPosition, wipeDuration).SetEase(wipeEase).SetLoops(-1,LoopType.Restart).SetAutoKill(false);
    }
}
