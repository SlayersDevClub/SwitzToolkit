using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFader : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float fadeDuration = 0.5f; // Duration of the fade
    public float idleAlpha = 0.5f; // Alpha when idle
    public float exitDelay = 5f; // Delay before fading out
    private CanvasGroup canvasGroup;
    private Tween fadeTween; // Store the fade tween
    private Coroutine fadeOutCoroutine; // To manage the fade-out timing

    private void Awake()
    {
        // Get the CanvasGroup component
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = idleAlpha; // Start at the idle alpha
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cancel any ongoing fade-out coroutine
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }

        // Fade in to full visibility
        FadeToFull();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Start the fade-out coroutine after a delay
        fadeOutCoroutine = StartCoroutine(FadeOutAfterDelay());
    }

    private IEnumerator FadeOutAfterDelay()
    {
        yield return new WaitForSeconds(exitDelay); // Wait for the specified delay
        FadeToIdle();
    }

    private void FadeToIdle()
    {
        if (canvasGroup.alpha > idleAlpha)
        {
            // Cancel any ongoing fade tween
            if (fadeTween != null)
            {
                fadeTween.Kill();
            }

            // Start fading to idle alpha
            fadeTween = canvasGroup.DOFade(idleAlpha, fadeDuration);
        }
    }

    private void FadeToFull()
    {
        if (canvasGroup.alpha < 1f)
        {
            // Cancel any ongoing fade tween
            if (fadeTween != null)
            {
                fadeTween.Kill();
            }

            // Start fading to full visibility
            fadeTween = canvasGroup.DOFade(1f, fadeDuration);
        }
    }
}
