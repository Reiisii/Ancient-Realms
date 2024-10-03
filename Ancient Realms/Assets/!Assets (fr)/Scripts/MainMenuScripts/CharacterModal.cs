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
    private EventSO[] eventArray;
    private NFTSO[] nftArray;
    void Start()
    {
        InitializeCharacters();
        InitializeEquipments();
        InitializeArtifacts();
        InitializeTrivia();
        InitializeEvents();
        InitializeLocations();
        InitializeNFTs();
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
                characterPrefab.setData(character);
        }

    }
    public void InitializeNFTs()
    {
        nftArray = Resources.LoadAll<NFTSO>("NFTSO").OrderBy(nft => nft.id).ToArray();
        foreach(NFTSO nftSO in nftArray)
        {
                NFTPortrait nftPrefab = Instantiate(nfPrefab, Vector3.zero, Quaternion.identity);
                nftPrefab.transform.SetParent(nftPanel);
                nftPrefab.transform.localScale = Vector3.one;
                nftPrefab.setGameObject(EncycPanel);
                nftPrefab.setNFT(nftSO);
        }
    }
    public void InitializeLocations()
    {
        locationArray = Resources.LoadAll<LocationSO>("LocationSO").OrderBy(location => location.culture).ToArray();
        foreach(LocationSO locationSO in locationArray)
        {
                if(locationSO.visibleEncyc){
                    Locations locationPrefab = Instantiate(locPrefab, Vector3.zero, Quaternion.identity);
                    locationPrefab.transform.SetParent(locationsPanel);
                    locationPrefab.transform.localScale = Vector3.one;
                    locationPrefab.setGameObject(EncycPanel);
                    locationPrefab.setData(locationSO);
                }
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
        artifactArray = Resources.LoadAll<ArtifactsSO>("ArtifactSO").OrderBy(events => events.name).ToArray();
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
    public void InitializeEvents()
    {
        eventArray = Resources.LoadAll<EventSO>("EventSO").OrderByDescending(events => events.name).ToArray();
        foreach(EventSO eventSO in eventArray)
        {
                Events eventPrefab = Instantiate(evtPrefab, Vector3.zero, Quaternion.identity);
                eventPrefab.transform.SetParent(eventPanel);
                eventPrefab.transform.localScale = Vector3.one;
                eventPrefab.setData(eventSO);
        }
    }
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