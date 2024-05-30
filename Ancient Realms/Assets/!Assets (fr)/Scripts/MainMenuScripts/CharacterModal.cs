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
    // [SerializeField] Locations locPrefab;
    // [SerializeField] RectTransform locationsPanel;
    // [SerializeField] Trivia trPrefab;
    // [SerializeField] RectTransform artifactsPanel;
    // [SerializeField] Events evPrefab;
    // [SerializeField] RectTransform artifactsPanel;
    
    IEnumerator Start()
    {
        yield return InitializeCharacters();
        yield return InitializeEquipments();
        yield return InitializeArtifacts();
    }

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
public class ArtifactsJson
{
    public int id;
    public string culture;
    public string name;
    public string description;
    public string imagePath;
}