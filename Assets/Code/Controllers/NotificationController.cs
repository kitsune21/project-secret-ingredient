using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationController : MonoBehaviour
{
    private TMP_Text notificationText;
    private RectTransform rectTransform;
    public GameObject dialogueOptionsPanel;
    public GameObject inventoryPanel;

    void Start() {
        notificationText = GetComponent<TMP_Text>();
        rectTransform = notificationText.rectTransform;
        ClearText();
    }

    public void UpdateNotificationText(string itemName, string notification) {
        if(dialogueOptionsPanel.activeSelf || inventoryPanel.activeSelf) {
            updateRectTransformPositionY(220);
        } else {
            updateRectTransformPositionY(25);
        }
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

    private void updateRectTransformPositionY(float y) {
        Vector2 currentPosition = rectTransform.anchoredPosition;
        currentPosition.y = y;
        rectTransform.anchoredPosition = currentPosition;
    }
}
