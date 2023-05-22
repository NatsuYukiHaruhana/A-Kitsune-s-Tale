using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour {
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject displayMenu;
    [SerializeField] private GameObject saveMenu;

    public void EnterSettings() {
        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        displayMenu.SetActive(false);
        settingsMenu.SetActive(true);
        PlayerPrefs.Save();
    }

    public void EnterAudioMenu() {
        audioMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void EnterDisplayMenu() {
        displayMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void EnterMainMenu() {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        saveMenu.SetActive(false);
    }
    
    public void EnterSaveMenu() {
        saveMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public static void LoadMainMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
