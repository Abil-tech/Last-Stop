using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // <--- Critical! Without this, SceneManager crashes the script
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1f;

    void Start()
    {
        // Smoothly fade in when the corridor boots up
        StartCoroutine(FadeIn());
    }

    public void FadeToNextScene()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * fadeSpeed;
            if (fadeImage != null)
                fadeImage.color = new Color(0f, 0f, 0f, t);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            if (fadeImage != null)
                fadeImage.color = new Color(0f, 0f, 0f, t);
            yield return null;
        }

        // Process the Exit 8 logic rules here!
        if (AnomalyManager.Instance != null)
        {
            bool choseRight = DoorInteraction.lastChoseRightDoor;
            bool anomalyPresent = AnomalyManager.Instance.isAnomalyActive;

            // WIN CONDITION MATRIX:
            // 1. Chose Right Door AND No Anomaly Present -> CORRECT
            // 2. Chose Left Door AND Anomaly IS Present -> CORRECT
            if ((choseRight && !anomalyPresent) || (!choseRight && anomalyPresent))
            {
                Debug.Log("[System] Choice Validated: Correct path taken!");
                AnomalyManager.Instance.AdvanceFloor();
            }
            else
            {
                Debug.Log("[System] Choice Validated: WRONG path taken!");
                AnomalyManager.Instance.ResetToBeginning();
            }

            // Reroll the level environment data layout
            AnomalyManager.Instance.GenerateLevelState();
        }

        // Reload the current level index loop
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}