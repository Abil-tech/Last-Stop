using UnityEngine;
using UnityEngine.SceneManagement;

public class UIscript : MonoBehaviour
{
    [Header("Menu")]
    public GameObject Settings;
    public GameObject PauseMenu;
    public GameObject play;
    public GameObject Quit;
    public GameObject NamaGame;
    public GameObject SettingButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    void PlaySound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    // ======================
    // MAIN MENU
    // ======================

    public void Play()
    {
        PlaySound();
        Invoke(nameof(LoadGame), 0.2f);
    }

    void LoadGame()
    {
        SceneManager.LoadScene("lingga-femboy");
    }

    public void QuitFunction()
    {
        PlaySound();
        Application.Quit();
    }

    public void Setting()
    {
        PlaySound();

        Settings.SetActive(true);
        play.SetActive(false);
        Quit.SetActive(false);
        SettingButton.SetActive(false);
        NamaGame.SetActive(false);
    }

    public void SettingExit()
    {
        PlaySound();

        Settings.SetActive(false);
        play.SetActive(true);
        Quit.SetActive(true);
        SettingButton.SetActive(true);
        NamaGame.SetActive(true);
    }

    // ======================
    // PAUSE MENU
    // ======================

    public void Pause()
    {
        PlaySound();

        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        PlaySound();

        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameExit()
    {
        PlaySound();

        Time.timeScale = 1f;
        Invoke(nameof(LoadMainMenu), 0.2f);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Main-Menu");
    }
}