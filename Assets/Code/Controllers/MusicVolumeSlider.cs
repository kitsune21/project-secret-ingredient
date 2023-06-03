using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    private MusicController mc;
    private Slider mySlider;

    void Start()
    {
        mc = GameObject.Find("MusicController").GetComponent<MusicController>();
        mySlider = gameObject.GetComponent<Slider>();
        mySlider.value = mc.volume * 10;
    }

    public void updateMCVolume() {
        mc.updateVolume(mySlider.value);
    }
}
