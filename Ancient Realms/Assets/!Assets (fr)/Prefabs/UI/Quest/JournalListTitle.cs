using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ESDatabase.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalListTitle : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI titleText;
    [SerializeField] public Image backgroundImage;
    public float targetAlpha = 0.6f; // The target alpha value when isActive is true
    public float duration = 1f;
    public bool isSelected = false;
    public QuestSO questData;
    void Start(){
        titleText.SetText(questData.questTitle);
        UpdateBackgroundAlpha();
    }
    public void Update(){
        UpdateBackgroundAlpha();
    }
    public void OnItemClick()
    {
        JournalManager.GetInstance().DeselectAllQuests();
        JournalManager.GetInstance().ShowQuestDetails(questData);
        
        isSelected = true;  
    }
    public void SetData(QuestSO questSO){
        questData = questSO;
    }
    private void UpdateBackgroundAlpha()
    {
        if (backgroundImage != null)
        {
            float alpha = isSelected ? targetAlpha : 0f;
            Color currentColor = backgroundImage.color;
            currentColor.a = alpha;
            backgroundImage.color = currentColor;
        }
    }


    public void Deselect()
    {
        isSelected = false;
        UpdateBackgroundAlpha();
    }

}
