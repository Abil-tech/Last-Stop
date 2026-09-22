using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class UIscript : MonoBehaviour
{
    public GameObject Settings;
    public GameObject PauseMenu;
    public GameObject play;
    public GameObject Quit;
    public GameObject NamaGame;
    public GameObject SettingButton;

    [Header("Continue Mode (for returning players)")]
    public string gameSceneName = "lingga-fem"; // <-- isi nama scene gamemu persis seperti di Build Settings
    public GameObject continueButton;           // optional: tombol "Continue" di menu (drag di Inspector)

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    void Start()
    {
        // Kalau pemain sudah pernah menang/escape, ubah tampilan menu
        bool hasEscaped = PlayerPrefs.GetInt("HasEscaped", 0) == 1;
        if (hasEscaped && continueButton != null)
        {
            continueButton.SetActive(true);
        }
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    // MainMenu
    public void Play()
    {
        PlayClickSound();
        bool hasEscaped = PlayerPrefs.GetInt("HasEscaped", 0) == 1;
        if (hasEscaped)
        {
            // Sudah pernah main -> langsung ke game, skip cutscene
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            // Pemain baru -> tonton cutscene dulu
            SceneManager.LoadScene("cutscene");
        }
    }

    public void QuitFunction()
    {
        PlayClickSound();
        Application.Quit();
    }

    public void Setting()
    {
        PlayClickSound();
        Settings.SetActive(true);
        play.SetActive(false);
        Quit.SetActive(false);
        SettingButton.SetActive(false);
        NamaGame.SetActive(false);
    }

    public void SettingExit()
    {
        PlayClickSound();
        Settings.SetActive(false);
        play.SetActive(true);
        Quit.SetActive(true);
        SettingButton.SetActive(true);
        NamaGame.SetActive(true);
    }

    // PauseMenu
    public void Pause()
    {
        PlayClickSound();
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        PlayClickSound();
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameExit()
    {
        PlayClickSound();
        SceneManager.LoadScene("Main-Menu");
        Time.timeScale = 1f;
    }
}