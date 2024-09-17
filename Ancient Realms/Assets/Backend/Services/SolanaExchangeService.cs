using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;
using Unisave.Authentication.Middleware;
using System.Threading.Tasks;
using LightJson;

public class SolanaExchangeService : Facet
{
    /// <summary>
    /// Client can call this facet method and receive a greeting
    /// Replace this with your own server-side code
    /// </summary>
    public decimal ConvertPHPtoSOL(decimal amountInPHP)
    {
        decimal phpToUsdRate = GetPHPtoUSDRate();
        decimal usdToSolRate = GetUSDToSOLRate();

        // Convert PHP to USD, then USD to SOL
        decimal amountInUSD = amountInPHP * phpToUsdRate;
        decimal amountInSOL = amountInUSD / usdToSolRate;

        return amountInSOL;
    }
    private decimal GetPHPtoUSDRate()
    {
        string phpToUsdUrl = "https://api.exchangerate-api.com/v4/latest/PHP"; // Use a reliable exchange API
        decimal phpPriceInUSD = decimal.Parse(Http.Get(phpToUsdUrl)["rates"]["USD"].AsString);
        return phpPriceInUSD;
    }

    // Get USD to SOL rate
    private decimal GetUSDToSOLRate()
    {
        string usdToSolUrl = "https://api.coingecko.com/api/v3/simple/price?ids=solana&vs_currencies=usd"; // Use a crypto price API
        decimal solPriceInUSD = decimal.Parse(Http.Get(usdToSolUrl)["solana"]["usd"].AsString);
        return solPriceInUSD;
    }
}
