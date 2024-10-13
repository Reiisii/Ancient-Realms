using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New NFT", menuName = "SO/NFT")]
public class NFTSO : ScriptableObject
{
    public int id;
    public string nftName;
    public string description;
    public string artWeaveLink;
    public string mintKey;
    public RarityEnum rarity;
    public CultureEnum culture;
    public Sprite image;
    public List<StatBuff> buffList;
}
