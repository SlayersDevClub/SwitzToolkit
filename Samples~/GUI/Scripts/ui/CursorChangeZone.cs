using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChangeZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CursorType cursorType; // Use the public enum directly

    private void Start()
    {
        // Set default cursor
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change the cursor icon when the mouse enters the zone
        CursorChanger.Instance.SetCursor(cursorType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorChanger.Instance.SetCursor(CursorType.Normal);
    }


    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
