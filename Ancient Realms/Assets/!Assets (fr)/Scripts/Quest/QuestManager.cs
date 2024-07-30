using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public TextAsset jsonFile;
    public QuestList questList;

    void Start()
    {
        if (jsonFile != null)
        {
            questList = JsonUtility.FromJson<QuestList>(jsonFile.text);
        }
    }

}
