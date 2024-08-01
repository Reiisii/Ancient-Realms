using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    [SerializeField] TriviaPrefab trPrefab;
    [SerializeField] RectTransform triviaPanel;
    [SerializeField] Events evtPrefab;
    [SerializeField] RectTransform eventPanel;
    [SerializeField] Locations locPrefab;
    [SerializeField] RectTransform locationsPanel;
    [SerializeField] NFTPortrait nfPrefab;
    [SerializeField] RectTransform nftPanel;
    [SerializeField] GameObject EncycPanel;
    
    private CharacterSO[] characterArray;
    private EquipmentSO[] equipmentArray;
    private ArtifactsSO[] artifactArray;
    private LocationSO[] locationArray;
    private TriviaSO[] triviaArray;
    IEnumerator Start()
    {
        InitializeCharacters();
        InitializeEquipments();
        InitializeArtifacts();
        InitializeTrivia();
        yield return InitializeEvents();
        InitializeLocations();
        yield return InitializeNFTs();

    }
    // Character Initialize
    public void InitializeCharacters()
    {
        characterArray = Resources.LoadAll<CharacterSO>("CharacterSO").OrderBy(character => character.id).ToArray();
        foreach (CharacterSO character in characterArray)
        {
                CharacterPortrait characterPrefab = Instantiate(chPrefab, Vector3.zero, Quaternion.identity);
                characterPrefab.transform.SetParent(characterPanel);
                characterPrefab.transform.localScale = Vector3.one;
                characterPrefab.setGameObject(EncycPanel);
                characterPrefab.character = character;
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
    public void InitializeLocations()
    {
        locationArray = Resources.LoadAll<LocationSO>("LocationSO").OrderBy(location => location.culture).ToArray();
        foreach(LocationSO locationSO in locationArray)
        {
                Locations locationPrefab = Instantiate(locPrefab, Vector3.zero, Quaternion.identity);
                locationPrefab.transform.SetParent(locationsPanel);
                locationPrefab.transform.localScale = Vector3.one;
                locationPrefab.setGameObject(EncycPanel);
                locationPrefab.setData(locationSO);
        }
    }
    // Equipment Initialize
    public void InitializeEquipments()
    {
        equipmentArray = Resources.LoadAll<EquipmentSO>("EquipmentSO").OrderBy(equipment => equipment.equipmentType).ToArray();
        foreach(EquipmentSO equipmentSO in equipmentArray)
        {
                Equipments equipmentPrefab = Instantiate(eqPrefab, Vector3.zero, Quaternion.identity);
                equipmentPrefab.transform.SetParent(equipmentPanel);
                equipmentPrefab.transform.localScale = Vector3.one;
                equipmentPrefab.setGameObject(EncycPanel);
                equipmentPrefab.setData(equipmentSO);
        }
    }
    // Artifacts Initialize
    public void InitializeArtifacts()
    {
        artifactArray = Resources.LoadAll<ArtifactsSO>("ArtifactSO").OrderBy(artifact => artifact.culture).ToArray();
        foreach(ArtifactsSO artifactSO in artifactArray)
        {
                Artifacts artifactPrefab = Instantiate(artPrefab, Vector3.zero, Quaternion.identity);
                artifactPrefab.transform.SetParent(artifactsPanel);
                artifactPrefab.transform.localScale = Vector3.one;
                artifactPrefab.setGameObject(EncycPanel);
                artifactPrefab.setData(artifactSO);
        }
    }
    // Trivia Initialize
    public void InitializeTrivia()
    {
        triviaArray = Resources.LoadAll<TriviaSO>("TriviaSO").ToArray();
        foreach(TriviaSO triviaSO in triviaArray)
        {
                TriviaPrefab triviaPrefab = Instantiate(trPrefab, Vector3.zero, Quaternion.identity);
                triviaPrefab.transform.SetParent(triviaPanel);
                triviaPrefab.transform.localScale = Vector3.one;
                triviaPrefab.setData(triviaSO);
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
                nftPrefab.setGameObject(EncycPanel);
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