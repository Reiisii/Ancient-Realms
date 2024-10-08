using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;
using System.Threading.Tasks;
using LightJson;
using ESDatabase.Classes;

public class SolanaExchangeService : Facet
{
    /// <summary>
    /// Client can call this facet method and receive a greeting
    /// Replace this with your own server-side code
    /// </summary>

    public PriceData GetPrice()
    {
        string usdToSolUrl = "http://23.88.54.33:3443/nft-price"; // Use a crypto price API
        decimal solPriceInUSD = decimal.Parse(Http.Get(usdToSolUrl)["data"].AsString);
        DateTime fetchedDate = DateTime.Parse(Http.Get(usdToSolUrl)["lastUpdate"].AsString);
        PriceData priceData = new PriceData(){
            price = solPriceInUSD,
            date = fetchedDate
        };
        return priceData;
    }
}
