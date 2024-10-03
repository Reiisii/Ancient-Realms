using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationQueue : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField] GameObject questStartGO;
    [SerializeField] GameObject questCompleteGO;
    [SerializeField] GameObject achievementGO;
    public Queue<Notification> queue;
    public void Awake(){
        queue = new Queue<Notification>();
    }
    public void AddQueue(Notification data){
        queue.Enqueue(data);
        if (!isActive)
        {
            StartCoroutine(ProcessQueue());
        }
    }
    private IEnumerator ProcessQueue()
    {

        while (queue.Count > 0)
        {
            Notification currentNotification = queue.Dequeue();
            switch (currentNotification.notifType)
            {
                case NotifType.QuestStart:
                    yield return StartCoroutine(questStartGO.GetComponent<NotificationPopup>().PopAnim(currentNotification));
                    break;

                case NotifType.QuestComplete:
                    yield return StartCoroutine(questCompleteGO.GetComponent<NotificationPopup>().PopAnim(currentNotification));
                    break;

                case NotifType.Achievement:
                    yield return StartCoroutine(achievementGO.GetComponent<NotificationPopup>().PopAnim(currentNotification));
                    break;
            }
        }
    }
}
