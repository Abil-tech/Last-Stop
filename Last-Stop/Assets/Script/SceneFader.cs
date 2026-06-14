using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeSpeed = 1f;

    [Header("UI Feedback")]
    public Text feedbackText;          // Drag your Canvas UI Text component here

    [Header("Audio Configurations")]
    public AudioSource audioSource;    // Drag your AudioSource component here
    public AudioClip correctSound;     // Drag your "Ding/Success" SFX here
    public AudioClip wrongSound;       // Drag your "Buzzer/JumpScare" SFX here
    public float resultDisplayTime = 1.5f; // How long to stay in black before reloading

    // --- DECLARATION CONTEXT EXAMPLE ---
    // This boolean acts as an input lock state variable.
    // It prevents chaotic button spamming from running parallel coroutines.
    private bool isFading = false; 

    void Start()
    {
        // Clear out any stale text values when a fresh loop boots up
        if (feedbackText != null) feedbackText.text = "";
        StartCoroutine(FadeIn());
    }

    public void FadeToNextScene()
    {
        // Reject any extra spam clicks if we are already transitioning
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
        // Slam the door shut on any incoming multi-click attempts
        isFading = true; 

        // Smoothly drop the curtain to total pitch black darkness
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            if (fadeImage != null)
                fadeImage.color = new Color(0f, 0f, 0f, t);
            yield return null;
        }

        // Search the active scene hierarchy to discover the current AnomalyManager instance
        AnomalyManager manager = Object.FindFirstObjectByType<AnomalyManager>();

        if (manager != null)
        {
            bool choseRight = DoorInteraction.lastChoseRightDoor;
            bool anomalyPresent = AnomalyManager.isAnomalyActive; 

            // Temporary placeholders to queue up our feedback data
            string displayMessage = "";
            AudioClip clipToPlay = null;

            // Exit 8 Core Logic Rules Matrix
            if ((choseRight && !anomalyPresent) || (!choseRight && anomalyPresent))
            {
                Debug.Log("[System] Choice Validated: Correct path taken!");
                manager.AdvanceFloor();
                
                // Track the new progression value dynamically
                displayMessage = $"FLOOR {AnomalyManager.currentFloor}";
                clipToPlay = correctSound;
            }
            else
            {
                Debug.Log("[System] Choice Validated: WRONG path taken!");
                manager.ResetToBeginning();
                
                displayMessage = "FLOOR 0";
                clipToPlay = wrongSound;
            }

            // Commit changes to UI and blast the audio clip
            if (feedbackText != null) feedbackText.text = displayMessage;
            if (audioSource != null && clipToPlay != null)
            {
                audioSource.PlayOneShot(clipToPlay);
            }

            // Freeze the void momentarily so the user can process their choices and hear the sound fully
            yield return new WaitForSeconds(resultDisplayTime);
        }
        else
        {
            Debug.LogError("[System Error] SceneFader couldn't locate the AnomalyManager script in this scene!");
        }

        // Wipe the scene layout clean and reload it fresh
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}