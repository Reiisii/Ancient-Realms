using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PopAnimation : MonoBehaviour
{
    public float popDuration = 0.3f;   // Time it takes for the pop animation
    public float waitDuration = 2.0f;  // Time to wait before disappearing
    public float finalScale = 1f;
    private Vector3 originalScale;
    void Start(){
        originalScale = transform.localScale;
    }
    private void OnEnable(){
        PopAnim();
    }
    public void PopAnim()
    {
        // Reset scale to zero initially
        transform.localScale = Vector3.zero;

        // Pop animation: Scale from zero to finalScale
        transform.DOScale(finalScale, popDuration).SetEase(Ease.OutBack);

        // Wait for the pop and then disappear
        DOVirtual.DelayedCall(popDuration + waitDuration, () => 
        {
            // Disappear by fading out (optional)
            FadeOutAndDestroy();
        });
    }

    private void FadeOutAndDestroy()
    {
        // Scale down and destroy the object
        transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            transform.localScale = originalScale;
            gameObject.SetActive(false);
        });
    }

}
