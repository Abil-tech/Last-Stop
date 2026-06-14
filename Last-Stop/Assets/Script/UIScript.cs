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

    //MainMenu
    public void Play()
    {
        SceneManager.LoadScene("lingga-femboy");
    }
    public void QuitFunction()
    {
        Application.Quit();
    }
    public void Setting()
    {
        Settings.SetActive(true);
        play.SetActive(false);
        Quit.SetActive(false);
        SettingButton.SetActive(false);
        NamaGame.SetActive(false);
    }
    public void SettingExit()
    {
        Settings.SetActive(false);
        play.SetActive(true);
        Quit.SetActive(true);
        SettingButton.SetActive(true);
        NamaGame.SetActive(true);
    }
    //

    //PauseMenu
    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void GameExit()
    {
        SceneManager.LoadScene("Main-Menu");
        Time.timeScale = 1f;
    }
    //
}
