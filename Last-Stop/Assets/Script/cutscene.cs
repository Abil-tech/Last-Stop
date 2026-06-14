using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [Header("Dialog UI")]
    public GameObject bubbleChat;
    public TMP_Text chatText;

    [Header("Fade")]
    public Image fadePanel;
    public float fadeDuration = 1f;

    [Header("Dialog")]
    [TextArea(2, 5)]
    public string[] dialogues;
    public float dialogDuration = 3f;

    [Header("Scene")]
    public string nextSceneName;

    private void Start()
    {
        StartCoroutine(StartCutscene());
    }

    IEnumerator StartCutscene()
    {
        // Mulai dari layar hitam
        Color color = fadePanel.color;
        color.a = 1f;
        fadePanel.color = color;

        yield return FadeIn();
        yield return PlayDialog();
        yield return FadeOut();

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator PlayDialog()
    {
        bubbleChat.SetActive(true);

        foreach (string dialogue in dialogues)
        {
            chatText.text = dialogue;
            yield return new WaitForSeconds(dialogDuration);
        }

        bubbleChat.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            Color color = fadePanel.color;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadePanel.color = color;

            yield return null;
        }

        Color finalColor = fadePanel.color;
        finalColor.a = 0f;
        fadePanel.color = finalColor;
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            Color color = fadePanel.color;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadePanel.color = color;

            yield return null;
        }

        Color finalColor = fadePanel.color;
        finalColor.a = 1f;
        fadePanel.color = finalColor;
    }
}