using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Change_Volume : MonoBehaviour {
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    [SerializeField] private bool forBGM, forSFX;

    private void Start() {
        if (forBGM) {
            if (slider != null) {
                slider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
            }
            mixer.SetFloat("BGMVolume", Mathf.Log10(PlayerPrefs.GetFloat("BGMVolume", 1f)) * 20);
        } else if (forSFX) {
            if (slider != null) {
                slider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            }
            mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1f)) * 20);
        }
    }

    public void SetLevel(float sliderValue) {
        if (forBGM) {
            mixer.SetFloat("BGMVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("BGMVolume", sliderValue);
        } else if (forSFX) {
            mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("SFXVolume", sliderValue);
        }
    }
}
