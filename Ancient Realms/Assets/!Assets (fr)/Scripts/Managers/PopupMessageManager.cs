using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessageManager : MonoBehaviour
{
    [Header("Message")]
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] Image image;

    [Header("Message Settings")]
    public float popDuration = 0.3f;   // Time it takes for the pop animation
    public float waitDuration = 1.0f;  // Default time to wait before disappearing
    private float finalScale = 0.8f;
    private Vector3 originalScale;

    [Header("Message Icons")]
    [SerializeField] Sprite info;
    [SerializeField] Sprite success;
    [SerializeField] Sprite error;
    [SerializeField] Sprite warning;

    private static PopupMessageManager currentPopupInstance = null;
    private bool isDisplayed = false;
    private bool isClosing = false; // New flag to check if popup is in the process of closing
    private Tween lifetimeTween; // Store the delayed call tween

    // Create or update the popup
    public static PopupMessageManager CreatePopup(PopupMessageManager popupPrefab, Transform parent, MType msgType, string msg)
    {
        if (currentPopupInstance != null && !currentPopupInstance.isClosing)
        {
            // Update existing popup's message
            currentPopupInstance.SetMessage(msgType, msg);
            currentPopupInstance.ResetLifetime(); // Reset its lifetime to 1 second
            currentPopupInstance.ShowPopup(true); // Force the popup to replay the animation
            return currentPopupInstance;
        }

        // Instantiate new popup and set it as the active instance
        PopupMessageManager newPopup = Instantiate(popupPrefab, Vector3.zero, Quaternion.identity);
        newPopup.transform.SetParent(parent, false);
        newPopup.transform.localScale = Vector3.one;
        newPopup.transform.localPosition = new Vector3(0f, 340f);
        newPopup.SetMessage(msgType, msg);
        newPopup.ShowPopup(false); // Display the popup without forcing animation

        currentPopupInstance = newPopup;
        return newPopup;
    }

    // Show the popup with animation (forceAnimation = true will replay the pop animation)
    public async void ShowPopup(bool forceAnimation)
    {
        if (isDisplayed && !forceAnimation) return; // Prevent re-triggering unless forced

        // Reset the scale to zero if replaying the animation
        if (forceAnimation)
        {
            transform.localScale = Vector3.zero;
        }

        isDisplayed = true;
        isClosing = false; // Reset the closing flag

        // Pop animation: Scale from zero to finalScale
        await transform.DOScale(finalScale, popDuration).SetEase(Ease.OutBack).SetUpdate(true).AsyncWaitForCompletion();

        // Start the lifetime countdown
        ResetLifetime();
    }

    // Set the message type and text
    public void SetMessage(MType msgType, string msg)
    {
        switch (msgType)
        {
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

    // Reset the popup's lifetime
    public void ResetLifetime()
    {
        // If the previous lifetime tween exists, kill it to reset the timer
        if (lifetimeTween != null && lifetimeTween.IsActive())
        {
            lifetimeTween.Kill();
        }

        // Start a new lifetime countdown
        lifetimeTween = DOVirtual.DelayedCall(waitDuration, async () =>
        {
            await FadeOutAndDestroy();
        }).SetUpdate(true);
    }

    // Fade out and destroy the popup
    private async Task FadeOutAndDestroy()
    {
        isClosing = true; // Set the flag to indicate the popup is closing
        await transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetUpdate(true).AsyncWaitForCompletion();

        // Reset state
        isDisplayed = false;
        currentPopupInstance = null;

        // Optionally destroy or deactivate (deactivate instead of immediate destroy)
        gameObject.SetActive(false); // Instead of Destroy(gameObject)
    }
}
