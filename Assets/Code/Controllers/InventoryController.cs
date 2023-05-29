using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public List<Item> currentItems = new List<Item>();
    public List<GameObject> currentItemsImagePanels = new List<GameObject>();
    public GameObject inventoryPanel;
    private Transform inventoryImagesPanel;
    public GameObject inventoryButtonPrefab;
    public GameObject dragItem;

    void Start() {
        inventoryImagesPanel = inventoryPanel.transform.Find("InventoryImages");
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            if(PlayerStateController.Instance.GetPlayerState() == PlayerState.Playing || PlayerStateController.Instance.GetPlayerState() == PlayerState.DraggingInventory) {
                inventoryPanel.SetActive(true); 
                PlayerStateController.Instance.UpdatePlayerState(PlayerState.InInventory);
                dragItem.GetComponent<DragItemController>().StopDragging();
            } else if(PlayerStateController.Instance.GetPlayerState() == PlayerState.InInventory) {
                inventoryPanel.SetActive(false);
                PlayerStateController.Instance.UpdatePlayerState(PlayerState.Playing);
            }
        }
    }
    
    public void AddItem(Item newItem) {
        currentItems.Add(newItem);
        refreshInventoryPanel();
    }

    public void RemoveItem(Item itemToRemove) {
        currentItems.Remove(itemToRemove);
        refreshInventoryPanel();
    }

    public bool CheckIfPlayerHasItem(Item checkItem) {
        foreach(Item myItem in currentItems) {
            if(myItem.id == checkItem.id) {
                return true;
            }
        }
        return false;
    }

    private void refreshInventoryPanel() {
        foreach(GameObject imagePanel in currentItemsImagePanels) {
            Destroy(imagePanel);
        }
        currentItemsImagePanels.Clear();
        foreach(Item myItem in currentItems) {
            GameObject newInventoryButton = Instantiate(inventoryButtonPrefab, inventoryImagesPanel);
            newInventoryButton.GetComponentInChildren<InventoryButton>().setItem(myItem, this);
            currentItemsImagePanels.Add(newInventoryButton);
        }
    }

    public void IsDraggingInventory() {
        inventoryPanel.SetActive(false);
        PlayerStateController.Instance.UpdatePlayerState(PlayerState.DraggingInventory);
    }
}
