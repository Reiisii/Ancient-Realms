using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Types")]
    [SerializeField] public Texture2D cursorDefault;
    [SerializeField] public Texture2D cursorHoverDefault;
    [SerializeField] public Texture2D cursorHoverWarning;
    [SerializeField] public Texture2D cursorHoverLink;
    [SerializeField] public Texture2D cursorHoverHorizontal;
    [SerializeField] public Texture2D cursorHoverVertical;
    [SerializeField] public Texture2D cursorHoverAccept;
    [SerializeField] public Texture2D cursorHoverInfo;
    [SerializeField] public Texture2D cursorHoverHelp;
    [SerializeField] public Texture2D cursorHoverTextBox;
    [SerializeField] public Texture2D cursorDragClick;
    [SerializeField] public Texture2D cursorClick;

    private Vector2 hotSpot = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;

    private static CursorManager Instance;
    
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Found more than one Cursor Manager in the scene");
            Destroy(gameObject);
        }
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

    public void SetHoverCursor(MouseEnum mouseType)
    {
        switch(mouseType){
            case MouseEnum.Default:
                Cursor.SetCursor(cursorHoverDefault, hotSpot, cursorMode);
            break;
            case MouseEnum.Warning:
                Cursor.SetCursor(cursorHoverWarning, hotSpot, cursorMode);
            break;
            case MouseEnum.Link:
                Cursor.SetCursor(cursorHoverLink, hotSpot, cursorMode);
            break;
            case MouseEnum.Accept:
                Cursor.SetCursor(cursorHoverAccept, hotSpot, cursorMode);
            break;
            case MouseEnum.SlideVertical:
                Cursor.SetCursor(cursorHoverVertical, hotSpot, cursorMode);
            break;
            case MouseEnum.SlideHorizontal:
                Cursor.SetCursor(cursorHoverHorizontal, hotSpot, cursorMode);
            break;
            case MouseEnum.Info:
                Cursor.SetCursor(cursorHoverInfo, hotSpot, cursorMode);
            break;
            case MouseEnum.Help:
                Cursor.SetCursor(cursorHoverHelp, hotSpot, cursorMode);
            break;
            case MouseEnum.TextBox:
                Cursor.SetCursor(cursorHoverTextBox, hotSpot, cursorMode);
            break;
            default:
                Cursor.SetCursor(cursorHoverDefault, hotSpot, cursorMode);
            break;
        }
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
