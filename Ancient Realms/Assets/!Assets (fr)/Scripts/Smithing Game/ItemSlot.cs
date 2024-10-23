using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private PieceType acceptedPiece;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform droppedPiece = eventData.pointerDrag.GetComponent<RectTransform>();
            DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();

            if (dragDrop != null && eventData.pointerDrag.gameObject.GetComponent<DragDrop>().pieceType == acceptedPiece)
            {
                Debug.Log("Accepted piece: " + acceptedPiece + " | Dropped piece: " + eventData.pointerDrag.name);

                // Snap the piece to the slot
                droppedPiece.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                dragDrop.placed = true;
                dragDrop.canvasGroup.interactable = false;
                dragDrop.canvasGroup.blocksRaycasts = false;
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
