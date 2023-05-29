using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public List<Item> currentItems = new List<Item>();
    public GameObject inventoryPanel;
    private Transform inventoryImagesPanel;
    public GameObject inventoryImagePrefab;

    void Start() {
        inventoryImagesPanel = inventoryPanel.transform.Find("InventoryImages");
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            if(PlayerStateController.Instance.GetPlayerState() == PlayerState.Playing) {
                inventoryPanel.SetActive(true); 
                PlayerStateController.Instance.UpdatePlayerState(PlayerState.InInventory);
            } else if(PlayerStateController.Instance.GetPlayerState() == PlayerState.InInventory) {
                inventoryPanel.SetActive(false);
                PlayerStateController.Instance.UpdatePlayerState(PlayerState.Playing);
            }
        }
    }
    
    public void AddItem(Item newItem) {
        currentItems.Add(newItem);
        GameObject newImagePanel = Instantiate(inventoryImagePrefab, inventoryImagesPanel);
        newImagePanel.GetComponent<Image>().sprite = newItem.image;
    }
}
