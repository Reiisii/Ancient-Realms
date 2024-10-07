using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour
{
    [Header("Message")]
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] Image image;
    [Header("Message Settings")]
    public float popDuration = 0.3f;   // Time it takes for the pop animation
    public float waitDuration = 1.0f;  // Time to wait before disappearing
    public float finalScale = 1f;
    private Vector3 originalScale;
    [Header("Message Icons")]
    [SerializeField] Sprite info;
    [SerializeField] Sprite success;
    [SerializeField] Sprite error;
    [SerializeField] Sprite warning;
    
    async void OnEnable(){
        transform.localScale = Vector3.zero;

        // Pop animation: Scale from zero to finalScale
        transform.DOScale(finalScale, popDuration).SetEase(Ease.OutBack).SetUpdate(true);
        await DOVirtual.DelayedCall(popDuration + waitDuration, async () => 
        {
            await transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(()=> {
                Destroy(gameObject);
            }).AsyncWaitForCompletion();
        }).SetUpdate(true).AsyncWaitForCompletion();
    }
    void Start(){
        originalScale = transform.localScale;
    }

    public void SetMessage(MType msgType, string msg){
        
        switch(msgType){
            case MType.Info:
                image.sprite = info;
                image.color = Utilities.HexToColor("#FFFFFF");
            break;
            case MType.Success:
                image.sprite = success;
                image.color = Utilities.HexToColor("#72d874");
            break;
            case MType.Error:
                image.sprite = error;
                image.color = Utilities.HexToColor("#d43f3f");
            break;
            case MType.Warning:
                image.sprite = warning;
                image.color = Utilities.HexToColor("#e8b923");
            break;
        }
        message.SetText(msg);
        
    }

    private void FadeOutAndDestroy()
    {
        // Scale down and destroy the object
        
    }
}
