using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationQueue : MonoBehaviour
{
    public bool isActive = false;
    private bool isInvoking;
    [SerializeField] GameObject questStartGO;
    [SerializeField] GameObject questCompleteGO;
    [SerializeField] GameObject achievementGO;
    [SerializeField] GameObject newEquipmentGO;
    [SerializeField] GameObject newCharacterGO;
    [SerializeField] GameObject newEventGO;
    public Queue<Notification> queue;
    public void Awake(){
        queue = new Queue<Notification>();
    }
    public void Start(){
        StartContinuousInvocation();
    }
    private void StartContinuousInvocation()
    {
        if (!isInvoking)
        {
            isInvoking = true; // Set the flag to true
            StartCoroutine(InvokeContinuously());
        }
    }
    private IEnumerator InvokeContinuously()
    {
        while (isInvoking) // Loop while isInvoking is true
        {
            if(!isActive){
                StartCoroutine(ProcessQueue());
            }
             // Call your desired method
            yield return new WaitForSecondsRealtime(1f); // Wait for the specified interval
        }
    }
    public void AddQueue(Notification data){
        queue.Enqueue(data);
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
                case NotifType.Equipment:
                    yield return StartCoroutine(newEquipmentGO.GetComponent<NotificationPopup>().PopAnim(currentNotification));
                    break;
                case NotifType.Character:
                    yield return StartCoroutine(newCharacterGO.GetComponent<NotificationPopup>().PopAnim(currentNotification));
                    break;
                case NotifType.Event:
                    yield return StartCoroutine(newEventGO.GetComponent<NotificationPopup>().PopAnim(currentNotification));
                    break;
            }
        }
    }
}
