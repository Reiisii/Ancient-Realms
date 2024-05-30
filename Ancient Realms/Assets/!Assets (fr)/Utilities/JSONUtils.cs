using System;
using System.Collections;
using System.Collections.Generic;
using Solana.Unity.SDK.Nft;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JSONUtils : MonoBehaviour
{
    public IEnumerator LoadImage(string imagePath, CharacterPortrait characterPrefab){
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                characterPrefab.setImage(texture);
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }
}