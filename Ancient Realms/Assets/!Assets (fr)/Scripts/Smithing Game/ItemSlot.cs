using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private string acceptedPiece;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform droppedPiece = eventData.pointerDrag.GetComponent<RectTransform>();
            DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();

            if (dragDrop != null && eventData.pointerDrag.name == acceptedPiece)
            {
                // Snap the piece to the slot
                droppedPiece.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                AudioManager.GetInstance().PlayAudio(SoundType.GREEN);

                // Notify the PuzzleManager
                PuzzleManager.instance.PiecePlaced(acceptedPiece);
            }
            else
            {
                AudioManager.GetInstance().PlayAudio(SoundType.RED);
            }
        }
    }
}
