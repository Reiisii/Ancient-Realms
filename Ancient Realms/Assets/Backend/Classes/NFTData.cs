using System;
using Unisave;

namespace ESDatabase.Classes
{
    [Serializable]
    public class NFTData
    {
        [Fillable] public int nftID { get; set; }
        [Fillable] public string mint { get; set; }
        public NFTData()
        {
            nftID = this.nftID;
            mint = this.mint;
        }
    }
}
