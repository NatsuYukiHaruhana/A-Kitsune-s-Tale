using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour {
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject displayMenu;

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
    }

    public void LoadGame() {
        Utils.InitGameData();
        SceneManager.LoadScene("Battle Scene");
    }

    public static void LoadMainMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
