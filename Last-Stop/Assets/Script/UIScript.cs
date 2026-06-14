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

    [Header("Audio Settings")]
    public AudioSource audioSource; // Tempat menaruh komponen Audio Source
    public AudioClip clickSound;    // Tempat menaruh file suara klik (.mp3/.wav)

    // Fungsi baru untuk memutar suara
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
        PlayClickSound(); // Memanggil suara sebelum pindah scene
        SceneManager.LoadScene("lingga-femboy");
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