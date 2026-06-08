using UnityEngine;
using UnityEngine.UI;
public class sound : MonoBehaviour
{
    [SerializeField] Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.GetFloat("volume", 1);
            load();
        }
        else
        {
            load();
        }
    }

    public void setVolume()
    {
        AudioListener.volume = slider.value;
        save();
    }
    private void load()
    {
        slider.value = PlayerPrefs.GetFloat("volume");
    }
    private void save()
    {
        PlayerPrefs.SetFloat("volume", slider.value);   
    }
}