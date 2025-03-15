using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class SliderVisualControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string Postfix = "%";

    private Slider slider;
    private RectTransform handleRect;
    TextMeshProUGUI updateText;
    bool isClicked;
    Shadow _shadow;

    void Start()
    {
        slider = GetComponent<Slider>();
        if (slider != null && slider.handleRect != null)
        {
            handleRect = slider.handleRect;
            updateText = GetComponentInChildren<TextMeshProUGUI>();
            updateText.alpha = 0;
        }
        else
        {
            Debug.LogError("Slider or handleRect is not assigned.");
        }

        _shadow = handleRect.GetComponent<Shadow>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(handleRect, eventData.position, eventData.pressEventCamera))
        {
            // Perform actions on the handle here
            //Debug.Log("Handle clicked!");
            HandleClicked();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HandleUpClick();
    }

    private void HandleClicked()
    {
        isClicked = true;
        updateText.DOKill();
        updateText.DOFade(.5f, 1);
        Image handleImage = handleRect.GetComponent<Image>();
        if (handleImage != null)
        {

        }
        _shadow.enabled = false;

    }

    void HandleUpClick()
    {
        isClicked = false;
        updateText.DOFade(0, 1).SetDelay(1);
        _shadow.enabled = true;
    }

    private void Update()
    {
        if (isClicked)
        {
            updateText.SetText(slider.value.ToString() + Postfix);
        }
    }
}
