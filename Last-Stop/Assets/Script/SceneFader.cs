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

        // Find the active anomaly manager component in the current scene layout
        AnomalyManager manager = FindFirstObjectByType<AnomalyManager>();
        if (manager != null)
        {
            bool choseRight = DoorInteraction.lastChoseRightDoor;
            bool anomalyPresent = AnomalyManager.isAnomalyActive;

            // WIN CONDITION MATRIX
            if ((choseRight && !anomalyPresent) || (!choseRight && anomalyPresent))
            {
                Debug.Log("[System] Choice Validated: Correct path taken!");
                manager.AdvanceFloor();
            }
            else
            {
                Debug.Log("[System] Choice Validated: WRONG path taken!");
                manager.ResetToBeginning();
            }
        }

        // Reload the scene. The fresh manager's Start() will run the new randomization seamlessly!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}