using UnityEngine;
using UnityEngine.EventSystems;

public class CursorTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CursorManager cursorManager;
    [SerializeField] bool isWarning;

    void Start()
    {
        cursorManager = CursorManager.GetInstance();
        if (cursorManager == null)
        {
            Debug.LogError("CursorManager not found in the scene.");
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorManager.SetHoverCursor(isWarning);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursorManager.SetDefaultCursor();
    }
}
