using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public static CursorChanger Instance { get; private set; }

    public Texture2D resizeHorizontalCursor;
    public Texture2D resizeVerticalCursor;
    public Texture2D resizeDiagonalLeftCursor;
    public Texture2D resizeDiagonalRightCursor;
    public Texture2D noCursor;
    public Texture2D marqueeSelectCursor;
    public Texture2D normalCursor;
    public Texture2D moveCursor, loadingCursor;
    public Vector2 _hotspot = new Vector2(16, 16);

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetCursor(CursorType.Normal);
    }

    public void SetCursor(CursorType cursorType)
    {
        Texture2D cursorTexture = null;

        switch (cursorType)
        {
            case CursorType.ResizeHorizontal:
                cursorTexture = resizeHorizontalCursor;
                //_hotspot = new Vector2(0.5f,0.5f);
                break;
            case CursorType.ResizeVertical:
                cursorTexture = resizeVerticalCursor;
                //_hotspot = new Vector2(0.5f,0.5f);
                break;
            case CursorType.ResizeDiagonalLeft:
                cursorTexture = resizeDiagonalLeftCursor;
                //_hotspot = new Vector2(0.5f,0.5f);
                break;
            case CursorType.ResizeDiagonalRight:
                cursorTexture = resizeDiagonalRightCursor;
                //_hotspot = new Vector2(0.5f,0.5f);
                break;
            case CursorType.No:
                cursorTexture = noCursor;
                break;
            case CursorType.MarqueeSelect:
                cursorTexture = marqueeSelectCursor;
                break;
            case CursorType.Normal:
                cursorTexture = normalCursor;
                //_hotspot = Vector2.zero;
                break;
            case CursorType.Move:
                cursorTexture = moveCursor;
                break;
            case CursorType.Loading:
                cursorTexture = loadingCursor;
                break;
        }

#if UNITY_WEBGL
        Cursor.SetCursor(cursorTexture, _hotspot, CursorMode.ForceSoftware);
#else
        Cursor.SetCursor(cursorTexture, _hotspot, CursorMode.Auto);
#endif

    }
}
