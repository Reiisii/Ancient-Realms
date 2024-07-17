using System;
using System.Collections;
using System.Collections.Generic;
using Solana.Unity.Dex.Orca.Math;
using Solana.Unity.Programs;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Messages;
using Solana.Unity.Rpc.Models;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using UnityEngine;

public class SolanaUtility : MonoBehaviour
{

        public static ulong ConvertSolToLamports(decimal sol)
        {
            decimal lamportsPerSol = 1_000_000_000m;
            return (ulong)(sol * lamportsPerSol);
        }
        public async void TransferSols(){
         decimal sol = 1m; 
        ulong lamports = ConvertSolToLamports(sol);
        var transaction = new Transaction
        {
            RecentBlockHash = await Web3.Instance.WalletBase.GetBlockHash(),
            FeePayer = Web3.Instance.WalletBase.Account.PublicKey,
            Instructions = new List<TransactionInstruction>
            {

                SystemProgram.Transfer(
                    Web3.Instance.WalletBase.Account.PublicKey,
                    new PublicKey("DuMTi4y6t9p3Zoa4HgZB9WBTzQYas2B2hVSQbZ8veppi"),
                    lamports),
            },
                Signatures = new List<SignaturePubKeyPair>()
            };

            var result = await Web3.Instance.WalletBase.SignAndSendTransaction(transaction);
            if(result.WasSuccessful){
                Debug.Log("Transaction Success!");
                Debug.Log("Signature: " + result.Result);
            }
        }

}