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
    void Update(){
        UpdateBackgroundAlpha();
    }
    public void OnItemClick()
    {
        isSelected = true;
        JournalManager.GetInstance().ShowQuestDetails(questData);
        UpdateBackgroundAlpha();
    }
    public void SetData(QuestSO questSO){
        questData = questSO;
    }
    private void UpdateBackgroundAlpha()
    {
        if (backgroundImage != null)
        {
            // if (isSelected)
            // {
            //     // Animate the alpha of the background color to the target alpha
            //     backgroundImage.DOFade(targetAlpha, duration);
            // }
            // else
            // {
            //     // Optionally, you can reset the alpha to fully transparent or another value
            //     backgroundImage.DOFade(0f, duration);
            // }
        }
    }
}
