using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public Image buttonImage;
    private Item myItem;
    private InventoryController inventoryController;

    public void setItem(Item newItem, InventoryController newInventoryController) {
        myItem = newItem;
        inventoryController = newInventoryController;
        buttonImage.sprite = newItem.image;
    }

    public void StartDragging() {
        DragItemController dragItemController;
        dragItemController = GameObject.FindGameObjectWithTag("DragItem").GetComponent<DragItemController>();
        dragItemController.myItem = myItem;
        dragItemController.StartDragging();
        inventoryController.IsDraggingInventory();
    } 
}
