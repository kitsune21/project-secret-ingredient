using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItemController : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool isDragging = false;
    private Vector2 offset;
    public Item myItem {get; set;}
    public Color invisibleColor;
    private Image myImage;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        myImage = GetComponent<Image>();
    }

    public void StartDragging() {
        isDragging = true;
        myImage.sprite = myItem.image;
        myImage.color = Color.white;
    }

    public void StopDragging() {
        isDragging = false;
        myItem = null;
        myImage.sprite = null;
        myImage.color = Color.clear;
    }

    private void Update()
    {
        if (isDragging)
        {
            rectTransform.position = (Vector2)Input.mousePosition + offset;
        }
    }
}
