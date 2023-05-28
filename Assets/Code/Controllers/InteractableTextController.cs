using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableTextController : MonoBehaviour
{
    private TMP_Text myText;
    private RectTransform rectTransform;

    void Start() {
        myText = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 10f, Input.mousePosition.z);
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            mousePosition,
            null,
            out canvasPosition
        );
        rectTransform.localPosition = canvasPosition;
    }

    public void UpdateMyText(string newText) {
        myText.text = newText;
    }
}
