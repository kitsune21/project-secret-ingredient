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
    private PlayerStateController playerState;
    public NotificationController notificationController;

    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        inventoryImagesPanel = inventoryPanel.transform.Find("InventoryImages");
    }

    void Update() {
        if(playerState == null) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        }
        if (Input.GetMouseButtonDown(1)) {
            if(playerState.GetPlayerState() == PlayerState.Playing || playerState.GetPlayerState() == PlayerState.DraggingInventory) {
                inventoryPanel.SetActive(true); 
                playerState.UpdatePlayerState(PlayerState.InInventory);
                dragItem.GetComponent<DragItemController>().StopDragging();
            } else if(playerState.GetPlayerState() == PlayerState.InInventory) {
                inventoryPanel.SetActive(false);
                playerState.UpdatePlayerState(PlayerState.Playing);
            }
        }
        if(playerState.GetPlayerState() != PlayerState.InInventory) {
            CloseInventory();
        }
    }
    
    public void AddItem(Item newItem, string notificationText) {
        currentItems.Add(newItem);
        refreshInventoryPanel();
        notificationController.GetItemNotification(newItem.name, notificationText);
    }

    public void RemoveItem(Item itemToRemove, string notificationText) {
        currentItems.Remove(itemToRemove);
        refreshInventoryPanel();
        notificationController.RemoveItemNotification(itemToRemove.name, notificationText);
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
        playerState.UpdatePlayerState(PlayerState.DraggingInventory);
    }

    public void CloseInventory() {
        inventoryPanel.SetActive(false);
    }
}
