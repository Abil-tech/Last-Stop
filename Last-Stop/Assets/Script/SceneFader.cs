using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeSpeed = 1f;

    [Header("Dopamine & Damage (UI/Audio)")]
    public Text feedbackText;          // Drag your Canvas UI Text component here
    public AudioSource audioSource;    // Drag your AudioSource component here
    public AudioClip correctSound;     // SFX for making the right choice
    public AudioClip wrongSound;       // SFX for getting completely jiped
    public float resultDisplayTime = 1.5f; // How many seconds to wait in pitch black

    private bool isFading = false; 

    void Start()
    {
        // Clear out any old text when a fresh loop boots up
        if (feedbackText != null) feedbackText.text = "";
        StartCoroutine(FadeIn());
    }

    public void FadeToNextScene()
    {
        if (isFading) return; 
        
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
        isFading = true; 

        // STEP 1: Fade to absolute pitch black first
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            if (fadeImage != null)
                fadeImage.color = new Color(0f, 0f, 0f, t);
            yield return null;
        }

        // STEP 2: Find the Anomaly Manager to validate the player's choices
        AnomalyManager manager = Object.FindFirstObjectByType<AnomalyManager>(); 

        if (manager != null)
        {
            bool choseRight = DoorInteraction.lastChoseRightDoor;
            bool anomalyPresent = AnomalyManager.isAnomalyActive; 

            // --- DECLARATION CONTEXT EXAMPLE ---
            // We declare local variables to temporarily store our dynamic assets 
            // before committing them to the UI and Audio elements.
            string dynamicMessage = "";
            AudioClip clipToPlay = null;

            if ((choseRight && !anomalyPresent) || (!choseRight && anomalyPresent))
            {
                Debug.Log("[System] Choice Validated: Correct path taken!");
                manager.AdvanceFloor();
                
                // Show their updated progression status (e.g., "FLOOR 3")
                dynamicMessage = $"FLOOR {AnomalyManager.currentFloor}";
                clipToPlay = correctSound;
            }
            else
            {
                Debug.Log("[System] Choice Validated: WRONG path taken!");
                manager.ResetToBeginning();
                
                // Humiliate them with a hard reset to zero
                dynamicMessage = "FLOOR 0";
                clipToPlay = wrongSound;
            }

            // STEP 3: Assign the text data and fire off the audio blast
            if (feedbackText != null) feedbackText.text = dynamicMessage;
            if (audioSource != null && clipToPlay != null)
            {
                audioSource.PlayOneShot(clipToPlay);
            }

            // STEP 4: Pause execution briefly so the audio finishes playing and text can be read
            yield return new WaitForSeconds(resultDisplayTime);

            // Re-roll the corridor assets right before the new scene opens
            manager.GenerateLevelState();
        }
        else
        {
            Debug.LogError("[System Error] SceneFader couldn't find the AnomalyManager in this scene layout!");
        }

        // STEP 5: Reload scene layout
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}