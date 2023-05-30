using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationController : MonoBehaviour
{
    private TMP_Text notificationText;

    void Start() {
        notificationText = GetComponent<TMP_Text>();
        ClearText();
    }

    public void UpdateNotificationText(string newText) {
        notificationText.text = newText;
    }

    public void GetItemNotification(string itemName, string notification) {
        notificationText.text = notification + itemName;
        StartCoroutine(StartTimer(3.0f));
    }

    public void RemoveItemNotification(string itemName, string notification) {
        notificationText.text = notification + itemName;
        StartCoroutine(StartTimer(3.0f));
    }

    private IEnumerator StartTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        ClearText();
    }

    private void ClearText() {
        notificationText.text = "";
    }
}
