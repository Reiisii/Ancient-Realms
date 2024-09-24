using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFTDisplay : MonoBehaviour
{
    [SerializeField] Image nftImage;
    [SerializeField] TextMeshProUGUI nameText;
    public NFTSO nft;
    private void Start()
    {
        nftImage.sprite = nft.image;
        nameText.SetText(nft.nftName);
    }

    public void SetData(NFTSO nftSO){
        nft = nftSO;
    }
}
