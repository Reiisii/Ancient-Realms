using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Solana.Unity.SDK;
using TMPro;
using System.Threading.Tasks;
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

    public async void Purchase(){ 
        bool resp = await SolanaUtility.TransferSols();
        if(resp){
            PlayerStats.GetInstance().AddGold(1000);
            await AccountManager.SaveData(PlayerStats.GetInstance().localPlayerData);
            PlayerUIManager.GetInstance().SpawnMessage(MType.Success, "Check the Web Console (F12 Console) for your receipt");
        }
    }


}
