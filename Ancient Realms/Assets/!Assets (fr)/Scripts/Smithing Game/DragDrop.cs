using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    public PieceType pieceType;
    public bool placed = false;
    Vector3 oldPos;
    private void Awake(){
       rectTransform = GetComponent<RectTransform>();
       canvasGroup = GetComponent<CanvasGroup>();
       oldPos = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData){
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData){
        if(placed) return;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
    public void ResetPostion(){
        placed = false;
        gameObject.transform.position = oldPos;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void CheckReset(){
        if(placed) return;
        placed = false;
        gameObject.transform.position = oldPos;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void OnPointerDown(PointerEventData eventData){
    }
}
