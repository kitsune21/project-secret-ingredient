using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class OptionTextController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color hoverColor;
    private TMP_Text myText;
    public int myOptionIndex {get; set;}
    private DialogueController dialogueController;

    void Start() {
        myText = gameObject.GetComponent<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myText.color = defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        dialogueController.selectOption(myOptionIndex);
    }

    public void setDialogueController(DialogueController newDialogueController) {
        dialogueController = newDialogueController;
    }
}