using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TrainMapText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color hoverColor;
    private TMP_Text myText;
    public TrainMap trainMap;
    public string myMachi;
    public ClipScript myClip;
    public Transform newCameraLocation;

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
        myText.color = defaultColor;
        trainMap.UpdateMachi(myMachi);
        Camera.main.GetComponent<CameraController>().UpdateStartLoction(newCameraLocation);
        if(myClip) {
            GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>().crossFadeClip(myClip.clipName);
        } else {
            GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>().stopClip();
        }
    }
}