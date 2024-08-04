using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] public Texture2D cursorDefault;
    [SerializeField] public Texture2D cursorHover;
    [SerializeField] public Texture2D cursorHoverWarning;
    [SerializeField] public Texture2D cursorClick;

    private Vector2 hotSpot = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;

    private static CursorManager Instance;
    
    private void Awake(){
        if(Instance != null){
            Debug.LogWarning("Found more than one Player Stats in the scene");
        }
        Instance = this;
    }
    public static CursorManager GetInstance(){
        return Instance;
    }
    void Start()
    {
        SetDefaultCursor();
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(cursorDefault, hotSpot, cursorMode);
    }

    public void SetHoverCursor(bool isWarning)
    {
        if(isWarning) {
            Cursor.SetCursor(cursorHoverWarning, hotSpot, cursorMode);
        }else Cursor.SetCursor(cursorHover, hotSpot, cursorMode);
    }

    public void SetClickCursor()
    {
        Cursor.SetCursor(cursorClick, hotSpot, cursorMode);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetClickCursor();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SetDefaultCursor();
        }
    }
}
