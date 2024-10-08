using UnityEngine;
[CreateAssetMenu(fileName = "New Event", menuName = "SO/Event")]
public class EventSO : ScriptableObject
{
    public int eventID;
    public string eventTitle;
    public string eventDescription;
    public string link;
}
