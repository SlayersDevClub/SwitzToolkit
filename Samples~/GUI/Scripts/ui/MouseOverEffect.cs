using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // The color to change to when the mouse is over the button
    public Color hoverColor = Color.yellow;
    // The original color of the button
    private Color originalColor;
    // Reference to the button's Image component
    private Image buttonImage;

    void Start()
    {
        // Get the Image component attached to the button
        buttonImage = GetComponent<Image>();
        // Store the original color of the button
        if (buttonImage != null)
        {
            //originalColor = buttonImage.color;
        }
    }

    // This method is called when the pointer enters the button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            // Change the button color to the hover color
            //buttonImage.color = hoverColor;
        }
    }

    // This method is called when the pointer exits the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            // Revert the button color to the original color
            //buttonImage.color = originalColor;
        }
    }
}
