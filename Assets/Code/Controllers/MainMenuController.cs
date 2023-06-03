using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject inventoryPanel;
    private Animator inventoryPanelAnimator;
    private bool isInventoryPanelOpen = false;
    private bool hasShownHintOnce = false;
    public GameObject hintText;
    public GameObject grassText;
    public GameObject inventoryIcon;
    public Sprite playImage;
    public Sprite settingsImage;
    public Sprite exitImage;
    public Color blankColor;
    private bool isPlay;
    private bool isSettings;
    private bool isExit;
    public GameObject settingsPanel;
    public GameObject musicSetting;
     
    void Start()
    {
        inventoryPanelAnimator = inventoryPanel.GetComponent<Animator>();
        StartCoroutine(HintTimer(2f));
        inventoryIcon.GetComponent<Image>().color = blankColor;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1)) {
            isInventoryPanelOpen = !isInventoryPanelOpen;
            hintText.SetActive(false);
            hasShownHintOnce = true;
            StopAllCoroutines();
            inventoryPanelAnimator.SetBool("open-menu-inventory", isInventoryPanelOpen);
        }
        inventoryIcon.GetComponent<RectTransform>().position = (Vector2)Input.mousePosition;
    }

    private IEnumerator HintTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        if(!isInventoryPanelOpen) {
            hintText.SetActive(!hintText.activeSelf);
            if(!hasShownHintOnce) {
                StartCoroutine(HintTimer(2.8f));
                hasShownHintOnce = true;
            }
        }
    }

    public void ShowGrassText() {
        grassText.SetActive(true);
    }

    public void HideGrassText() {
        grassText.SetActive(false);
    }

    public void SelectPlay() {
        inventoryIcon.GetComponent<Image>().sprite = playImage;
        inventoryIcon.GetComponent<Image>().color = Color.white;
        isPlay = true;
    }

    public void SelectSettings() {
        inventoryIcon.GetComponent<Image>().sprite = settingsImage;
        inventoryIcon.GetComponent<Image>().color = Color.white;
        isSettings = true;
    }

    public void SelectExit() {
        inventoryIcon.GetComponent<Image>().sprite = exitImage;
        inventoryIcon.GetComponent<Image>().color = Color.white;
        isExit = true;
    }

    public void SelectOption() {
        isInventoryPanelOpen = !isInventoryPanelOpen;
        inventoryPanelAnimator.SetBool("open-menu-inventory", isInventoryPanelOpen);
        inventoryIcon.GetComponent<Image>().color = blankColor;
        if(isExit) {
            Application.Quit();
        }
        if(isPlay) {
            SceneManager.LoadScene("TestingStuff");
        }
        if(isSettings) {
            settingsPanel.SetActive(true);
        }
        isPlay = false;
        isSettings = false;
        isExit = false;
    }

    public void CloseSettings() {
        settingsPanel.SetActive(false);
    }
}
