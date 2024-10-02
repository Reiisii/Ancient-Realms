using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image image;
    public float popDuration = 0.3f;   // Time it takes for the pop animation
    public float waitDuration = 2.0f;  // Time to wait before disappearing
    public float finalScale = 1f;
    private Vector3 originalScale;
    void Start(){
        originalScale = transform.localScale;
    }
    public IEnumerator PopAnim(Notification notifData)
    {
        
        switch(notifData.notifType){
            case NotifType.QuestStart:
                title.SetText(notifData.title);
            break;
            case NotifType.QuestComplete:
                title.SetText(notifData.title);
            break;
            case NotifType.Achievement:
                title.SetText(notifData.title);
                description.SetText(notifData.description);
                image.sprite = notifData.image;
            break;
        }
        gameObject.SetActive(true);
        // Reset scale to zero initially
        
        transform.localScale = Vector3.zero;

        // Pop animation: Scale from zero to finalScale
        transform.DOScale(finalScale, popDuration).SetEase(Ease.OutBack);

        // Wait for the pop and then disappear
        DOVirtual.DelayedCall(popDuration + waitDuration, async () => 
        {
            await FadeOutAndDestroy();
        });
        yield return new WaitForSeconds(4); 
    }

    private async Task FadeOutAndDestroy()
    {
        // Scale down and destroy the object
        await transform.DOScale(0, 0.5f).SetEase(Ease.InBack).AsyncWaitForCompletion();
            transform.localScale = originalScale;
            gameObject.SetActive(false);
            if(title != null)title.SetText("");
            if(description != null) description.SetText("");
            if(image != null) image.sprite = null;
    }
}
