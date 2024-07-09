using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterModal : MonoBehaviour
{
    [SerializeField] CharacterPortrait chPrefab;
    [SerializeField] RectTransform characterPanel;
    [SerializeField] Equipments eqPrefab;
    [SerializeField] RectTransform equipmentPanel;
    [SerializeField] Artifacts artPrefab;
    [SerializeField] RectTransform artifactsPanel;
    [SerializeField] Trivia trPrefab;
    [SerializeField] RectTransform triviaPanel;
    [SerializeField] Events evtPrefab;
    [SerializeField] RectTransform eventPanel;
    [SerializeField] Locations locPrefab;
    [SerializeField] RectTransform locationsPanel;
    [SerializeField] NFTPortrait nfPrefab;
    [SerializeField] RectTransform nftPanel;

    
    IEnumerator Start()
    {
        yield return InitializeCharacters();
        yield return InitializeEquipments();
        yield return InitializeArtifacts();
        yield return InitializeTrivia();
        yield return InitializeEvents();
        yield return InitializeLocations();
        yield return InitializeNFTs();
    }
    // Character Initialize
    IEnumerator InitializeCharacters()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Characters.json"); 
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(jsonFilePath))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    string jsonString = www.downloadHandler.text;
                    yield return StartCoroutine(ProcessJSONData(jsonString));
                }
                else
                {
                    Debug.LogError("Failed to load JSON file: " + www.error);
                }
            }
        }
        else
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            yield return StartCoroutine(ProcessJSONData(jsonString));
        }
    }
    IEnumerator InitializeNFTs()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "NFT.json"); 
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(jsonFilePath))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    string jsonString = www.downloadHandler.text;
                    yield return StartCoroutine(ProcessJSONDataNFT(jsonString));
                }
                else
                {
                    Debug.LogError("Failed to load JSON file: " + www.error);
                }
            }
        }
        else
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            yield return StartCoroutine(ProcessJSONDataNFT(jsonString));
        }
    }
    IEnumerator InitializeLocations()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Locations.json"); 
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(jsonFilePath))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    string jsonString = www.downloadHandler.text;
                    yield return StartCoroutine(ProcessJSONDataLocations(jsonString));
                }
                else
                {
                    Debug.LogError("Failed to load JSON file: " + www.error);
                }
            }
        }
        else
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            yield return StartCoroutine(ProcessJSONDataLocations(jsonString));
        }
    }
    // Equipment Initialize
    IEnumerator InitializeEquipments()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Items.json"); 
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(jsonFilePath))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    string jsonString = www.downloadHandler.text;
                    yield return StartCoroutine(ProcessJSONDataEquipments(jsonString));
                }
                else
                {
                    Debug.LogError("Failed to load JSON file: " + www.error);
                }
            }
        }
        else
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            yield return StartCoroutine(ProcessJSONDataEquipments(jsonString));
        }
    }
    // Artifacts Initialize
    IEnumerator InitializeArtifacts()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Artifacts.json"); 
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(jsonFilePath))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    string jsonString = www.downloadHandler.text;
                    yield return StartCoroutine(ProcessJSONDataArtifacts(jsonString));
                }
                else
                {
                    Debug.LogError("Failed to load JSON file: " + www.error);
                }
            }
        }
        else
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            yield return StartCoroutine(ProcessJSONDataArtifacts(jsonString));
        }
    }
    // Trivia Initialize
    IEnumerator InitializeTrivia()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Trivia.json"); 
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(jsonFilePath))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    string jsonString = www.downloadHandler.text;
                    yield return StartCoroutine(ProcessJSONDataTrivia(jsonString));
                }
                else
                {
                    Debug.LogError("Failed to load JSON file: " + www.error);
                }
            }
        }
        else
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            yield return StartCoroutine(ProcessJSONDataTrivia(jsonString));
        }
    }
    // Events Initialize
    IEnumerator InitializeEvents()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Events.json"); 
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(jsonFilePath))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    string jsonString = www.downloadHandler.text;
                    yield return StartCoroutine(ProcessJSONDataEvents(jsonString));
                }
                else
                {
                    Debug.LogError("Failed to load JSON file: " + www.error);
                }
            }
        }
        else
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            yield return StartCoroutine(ProcessJSONDataEvents(jsonString));
        }
    }
    
    // CHARACTERS JSON
    IEnumerator ProcessJSONData(string jsonString)
    {
        Dictionary<string, List<Character>> characterDict = JsonConvert.DeserializeObject<Dictionary<string, List<Character>>>(jsonString);

        foreach (var category in characterDict)
        {
            foreach (var character in category.Value)
            {
                
                CharacterPortrait characterPrefab = Instantiate(chPrefab, Vector3.zero, Quaternion.identity);
                characterPrefab.transform.SetParent(characterPanel);
                characterPrefab.transform.localScale = Vector3.one;
                characterPrefab.setName(character.lastName.Equals("") ? character.firstName : character.firstName + " " + character.lastName);
                characterData charData = new characterData
                {
                    firstName = character.firstName,
                    lastName = character.lastName,
                    description = character.biography,
                    imagePath = Path.Combine(Application.streamingAssetsPath, character.portraitPath)
                };
                characterPrefab.setData(charData);
                yield return StartCoroutine(LoadImage(charData.imagePath, characterPrefab));
            }

        }
    }
    // NFT JSON
    IEnumerator ProcessJSONDataNFT(string jsonString)
    {
        Dictionary<string, List<NFTJson>> characterDict = JsonConvert.DeserializeObject<Dictionary<string, List<NFTJson>>>(jsonString);

        foreach (var category in characterDict)
        {
            foreach (var character in category.Value)
            {
                
                NFTPortrait nftPrefab = Instantiate(nfPrefab, Vector3.zero, Quaternion.identity);
                nftPrefab.transform.SetParent(nftPanel);
                nftPrefab.transform.localScale = Vector3.one;
                nftPrefab.setName(character.name);
                nftData charData = new nftData
                {
                    name = character.name,
                    description = character.description,
                    rarity = character.rarity,
                    imagePath = Path.Combine(Application.streamingAssetsPath, character.imagePath)
                };
                nftPrefab.setNFT(charData);
                yield return StartCoroutine(LoadImage(charData.imagePath, nftPrefab));
            }

        }
    }
    // Locations JSON
    IEnumerator ProcessJSONDataLocations(string jsonString)
    {
        Dictionary<string, List<locationsJson>> characterDict = JsonConvert.DeserializeObject<Dictionary<string, List<locationsJson>>>(jsonString);

        foreach (var category in characterDict)
        {
            foreach (var character in category.Value)
            {
                
                Locations locationsPrefab = Instantiate(locPrefab, Vector3.zero, Quaternion.identity);
                locationsPrefab.transform.SetParent(locationsPanel);
                locationsPrefab.transform.localScale = Vector3.one;
                locationsPrefab.setName(character.name);
                locationData charData = new locationData
                {
                    name = character.name,
                    description = character.description,
                    imagePath = Path.Combine(Application.streamingAssetsPath, character.imagePath)
                };
                locationsPrefab.setData(charData);
                yield return StartCoroutine(LoadImage(charData.imagePath, locationsPrefab));
            }
            yield return null;
        }
    }
    // TRIVA JSON
    IEnumerator ProcessJSONDataTrivia(string jsonString)
    {
        Dictionary<string, List<TriviaJson>> characterDict = JsonConvert.DeserializeObject<Dictionary<string, List<TriviaJson>>>(jsonString);

        foreach (var category in characterDict)
        {
            foreach (var character in category.Value)
            {
                
                Trivia triviaPrefab = Instantiate(trPrefab, Vector3.zero, Quaternion.identity);
                triviaPrefab.transform.SetParent(triviaPanel);
                triviaPrefab.transform.localScale = Vector3.one;
                triviaData charData = new triviaData
                {
                    title = character.title,
                    description = character.description,
                };
                triviaPrefab.setData(charData);
            }
            yield return null;
        }
    }
        // TRIVA JSON
    IEnumerator ProcessJSONDataEvents(string jsonString)
    {
        Dictionary<string, List<EventsJson>> characterDict = JsonConvert.DeserializeObject<Dictionary<string, List<EventsJson>>>(jsonString);

        foreach (var category in characterDict)
        {
            foreach (var character in category.Value)
            {
                
                Events eventsPrefab = Instantiate(evtPrefab, Vector3.zero, Quaternion.identity);
                eventsPrefab.transform.SetParent(eventPanel);
                eventsPrefab.transform.localScale = Vector3.one;
                eventsData charData = new eventsData
                {
                    title = character.title,
                    description = character.description,
                };
                eventsPrefab.setData(charData);
            }
            yield return null;
        }
    }
    // EQUIPMENTS JSON
    IEnumerator ProcessJSONDataEquipments(string jsonString)
    {
            var characterDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<EquipmentJson>>>>(jsonString);

        foreach (var equipment in characterDict["Equipment"]) 
        {
            foreach (var equipmentType in equipment.Value)
            {
                Equipments equipmentPrefab = Instantiate(eqPrefab, Vector3.zero, Quaternion.identity);
                equipmentPrefab.transform.SetParent(equipmentPanel);
                equipmentPrefab.transform.localScale = Vector3.one;
                equipmentPrefab.setName(equipmentType.name);
                equipmentData charData = new equipmentData
                {
                    name = equipmentType.name,
                    description = equipmentType.description,
                    imagePath = Path.Combine(Application.streamingAssetsPath, equipmentType.imagePath),
                    culture = equipmentType.culture
                };
                equipmentPrefab.setData(charData);
                yield return StartCoroutine(LoadImage(charData.imagePath, equipmentPrefab));
            }
        }
    }
    // ARTIFACTS JSON
    IEnumerator ProcessJSONDataArtifacts(string jsonString)
    {
        Dictionary<string, List<ArtifactsJson>> characterDict = JsonConvert.DeserializeObject<Dictionary<string, List<ArtifactsJson>>>(jsonString);

        foreach (var category in characterDict)
        {
            foreach (var character in category.Value)
            {
                
                Artifacts artifactPrefab = Instantiate(artPrefab, Vector3.zero, Quaternion.identity);
                artifactPrefab.transform.SetParent(artifactsPanel);
                artifactPrefab.transform.localScale = Vector3.one;
                artifactPrefab.setName(character.name);
                artifactsData charData = new artifactsData
                {
                    name = character.name,
                    description = character.description,
                    imagePath = Path.Combine(Application.streamingAssetsPath, character.imagePath),
                    culture = character.culture
                };
                artifactPrefab.setData(charData);
                yield return StartCoroutine(LoadImage(charData.imagePath, artifactPrefab));
            }

        }
    }
    IEnumerator LoadImage(string imagePath, CharacterPortrait characterPrefab)
    {
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
    IEnumerator LoadImage(string imagePath, Equipments equipmentPrefab)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                equipmentPrefab.setImage(texture);
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }
    IEnumerator LoadImage(string imagePath, Locations locationsPrefab)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                locationsPrefab.setImage(texture);
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }
    IEnumerator LoadImage(string imagePath, Artifacts artifactsPrefab)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                artifactsPrefab.setImage(texture);
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }
    
    IEnumerator LoadImage(string imagePath, NFTPortrait nftPrefab)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                nftPrefab.setImage(texture);
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }
}

[System.Serializable]
public class Character
{
    public int id;
    public string firstName;
    public string lastName;
    public string portraitPath;
    public string biography;
}
[System.Serializable]
public class EquipmentJson
{
    public int id;
    public string culture;
    public string name;
    public string description;
    public string imagePath;
}
[System.Serializable]
public class locationsJson
{
    public int id;
    public string culture;
    public string name;
    public string description;
    public string imagePath;
}
[System.Serializable]
public class ArtifactsJson
{
    public int id;
    public string culture;
    public string name;
    public string description;
    public string imagePath;
}
[System.Serializable]
public class TriviaJson
{
    public string title;
    public string description;
}
[System.Serializable]
public class EventsJson
{
    public string title;
    public string description;
}
[System.Serializable]
public class NFTJson
{
    public int id;
    public string name;
    public string description;
    public string rarity;
    public string imagePath;

}