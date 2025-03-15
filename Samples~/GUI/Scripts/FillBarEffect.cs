using UnityEngine;
using UnityEngine.UI;

public class HealthBarEffect : MonoBehaviour
{
    public Image healthBarImage; // The UI Image (set to Filled)
    public RectTransform effectTransform; // The sprite or particle effect
    public RectTransform healthBarRect; // The full rect of the health bar

    void Update()
    {
        if (healthBarImage == null || effectTransform == null || healthBarRect == null)
            return;

        // Get fill amount (0 to 1)
        float fillAmount = healthBarImage.fillAmount;

        // Get the width of the health bar
        float barWidth = healthBarRect.rect.width;

        // Calculate the fill edge position in local space
        float fillEdgeX = Mathf.Lerp(-barWidth * 0.5f, barWidth * 0.5f, fillAmount);

        // Set the effect position
        effectTransform.anchoredPosition = new Vector2(fillEdgeX, effectTransform.anchoredPosition.y);
    }
}
