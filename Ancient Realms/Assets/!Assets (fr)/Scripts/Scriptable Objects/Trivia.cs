using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
[CreateAssetMenu(fileName = "New Trivia", menuName = "SO/Trivia")]
public class TriviaSO : ScriptableObject
{
    public string triviaTitle;
    public string triviaDescription;
    public string link;
}
