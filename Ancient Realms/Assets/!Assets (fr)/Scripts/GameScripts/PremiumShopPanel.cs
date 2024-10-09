using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Solana.Unity.SDK;
using TMPro;
using UnityEngine;

public class PremiumShopPanel : MonoBehaviour
{
    [Header("Store Data")]
    [SerializeField] public TextMeshProUGUI solBalance;
    public double previousBalance = 0;
    private void OnEnable(){
        Web3.OnBalanceChange += OnBalanceChange;
    }
    private void OnDisable(){
        Web3.OnBalanceChange -= OnBalanceChange;
    }
    private void OnBalanceChange(double sb)
    {
            double oldBalance = previousBalance;
            previousBalance = sb;

            AnimateSolChange(oldBalance, sb);
    }
    private void AnimateSolChange(double startValue, double endValue)
    {
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            solBalance.SetText(Utilities.FormatSolana(startValue));
        }, endValue, 1f).SetUpdate(true).SetEase(Ease.Linear);
    }
}
