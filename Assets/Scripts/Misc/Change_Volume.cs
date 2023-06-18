using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Change_Volume : MonoBehaviour {
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    [SerializeField] private bool forBGM, forSFX, forVoice;

    private bool firstTime = true;

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
        } else if (forVoice) {
            if (slider != null) {
                slider.value = PlayerPrefs.GetFloat("VoiceVolume", 1f);
            }
            mixer.SetFloat("VoiceVolume", Mathf.Log10(PlayerPrefs.GetFloat("VoiceVolume", 1f)) * 20);
            firstTime = false;
        }
    }

    public void SetLevel(float sliderValue) {
        if (forBGM) {
            mixer.SetFloat("BGMVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("BGMVolume", sliderValue);
        } else if (forSFX) {
            mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("SFXVolume", sliderValue);
        } else if (forVoice) {
            if (!firstTime) {
                GetComponent<Sound_Manager>().PlaySound("あ");
            }

            mixer.SetFloat("VoiceVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("VoiceVolume", sliderValue);
        }
    }
}
