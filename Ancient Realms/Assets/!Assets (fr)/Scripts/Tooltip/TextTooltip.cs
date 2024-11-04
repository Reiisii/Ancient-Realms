using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TextTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public string text;
    bool isHovering = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovering  && !string.IsNullOrEmpty(text))
        {
            isHovering = true;
            TooltipManager.GetInstance().ShowTextTooltip(text);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHovering)
        {
            isHovering = false;
            TooltipManager.GetInstance().HideTextTooltip();
        }

    }
    private void OnDisable(){
        if(isHovering){
            isHovering = false;
            TooltipManager.GetInstance().HideTextTooltip();
        }
    }
    private void OnDestroy(){
        if(isHovering){
            isHovering = false;
            TooltipManager.GetInstance().HideTextTooltip();
        }
        
    }
}
