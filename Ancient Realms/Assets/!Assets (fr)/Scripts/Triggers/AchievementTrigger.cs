using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image image;
    
    public void ShowAchievement(ArtifactsSO artifact){
        title.SetText(artifact.artifactName);
        description.SetText(artifact.description);
        image.sprite = artifact.image;
    } 
}
