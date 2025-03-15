using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DockingZone : MonoBehaviour, IPointerExitHandler
{
    public UIDraggableElement elementSocketType;
    Color highlightColor = new Color();
    Image img;
    bool highlight;

    private void Start()
    {
        img = GetComponent<Image>();
        highlightColor = img.color;
        img.color = Color.clear;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(highlight)
        {
            img.DOColor(Color.clear, .25f);
            highlight = false;
        }
    }

    public void ToggleHighlight(bool b)
    {
        if(b)
        {
            highlight = true;
            img.DOColor(highlightColor, .25f);
        }
        else
        {
            highlight = false;
            img.DOColor(Color.clear, .25f);
        }

    }
}

