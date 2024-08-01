using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
[CreateAssetMenu(fileName = "New Event", menuName = "SO/Event")]
public class EventSO : ScriptableObject
{
    public string eventTitle;
    public string eventDescription;
    public string link;
}
