using System;
using System.Collections;
using System.Collections.Generic;
using Org.BouncyCastle.Asn1.Cms;
using Solana.Unity.Dex.Orca.Math;
using Solana.Unity.Metaplex.NFT.Library;
using Solana.Unity.Metaplex.Utilities;
using Solana.Unity.Programs;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Messages;
using Solana.Unity.Rpc.Models;
using Solana.Unity.Rpc.Types;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using UnityEngine;

public class SolanaUtility : MonoBehaviour
{
    public PublicKey bank = new PublicKey("DuMTi4y6t9p3Zoa4HgZB9WBTzQYas2B2hVSQbZ8veppi");    
    public static ulong ConvertSolToLamports(decimal sol){
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
                    bank,
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
    public async void MintNFT()
    {
        var mint = new Account();
        var associatedTokenAccount = AssociatedTokenAccountProgram
            .DeriveAssociatedTokenAccount(Web3.Account, mint.PublicKey);
        

        var metadata = new Metadata()
        {
            name = "Legionnaire",
            symbol = "AR",
            uri = "https://5bhev6763ti6lldpizrqnt5lagaxaie37rj7dimvskwotmyaec4a.arweave.net/6E5K-_7c0eWsb0ZjBs-rAYFwIJv8U_GhlZKs6bMAILg",
            sellerFeeBasisPoints = 0,
            creators = new List<Creator> { new(Web3.Account.PublicKey, 100, true) }
        };
        // Prepare the transaction
        var blockHash = await Web3.Rpc.GetLatestBlockHashAsync();
        var minimumRent = await Web3.Rpc.GetMinimumBalanceForRentExemptionAsync(TokenProgram.MintAccountDataSize);
        var transaction = new TransactionBuilder()
            .SetRecentBlockHash(blockHash.Result.Value.Blockhash)
            .SetFeePayer(Web3.Account)
            .AddInstruction(
                SystemProgram.CreateAccount(
                    Web3.Account,
                    mint.PublicKey,
                    minimumRent.Result,
                    TokenProgram.MintAccountDataSize,
                    TokenProgram.ProgramIdKey))
            .AddInstruction(
                TokenProgram.InitializeMint(
                    mint.PublicKey,
                    0,
                    Web3.Account,
                    Web3.Account))
            .AddInstruction(
                AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
                    Web3.Account,
                    Web3.Account,
                    mint.PublicKey))
            .AddInstruction(
                TokenProgram.MintTo(
                    mint.PublicKey,
                    associatedTokenAccount,
                    1,
                    Web3.Account))
            .AddInstruction(MetadataProgram.CreateMetadataAccount(
                PDALookup.FindMetadataPDA(mint), 
                mint.PublicKey, 
                Web3.Account, 
                Web3.Account, 
                Web3.Account.PublicKey, 
                metadata,
                TokenStandard.NonFungible, 
                true, 
                true, 
                null,
                metadataVersion: MetadataVersion.V3))
            .AddInstruction(MetadataProgram.CreateMasterEdition(
                    maxSupply: null,
                    masterEditionKey: PDALookup.FindMasterEditionPDA(mint),
                    mintKey: mint,
                    updateAuthorityKey: Web3.Account,
                    mintAuthority: Web3.Account,
                    payer: Web3.Account,
                    metadataKey: PDALookup.FindMetadataPDA(mint),
                    version: CreateMasterEditionVersion.V3
                )
            );
        var tx = Transaction.Deserialize(transaction.Build(new List<Account> {Web3.Account, mint}));
        
        // Sign and Send the transaction
        var res = await Web3.Wallet.SignAndSendTransaction(tx);
        
        // Show Confirmation
        if (res?.Result != null){
            await Web3.Rpc.ConfirmTransaction(res.Result, Commitment.Confirmed);
            Debug.Log("Minting succeeded, see transaction at https://explorer.solana.com/tx/" 
                      + res.Result + "?cluster=" + Web3.Wallet.RpcCluster.ToString().ToLower());
        }


    }

}