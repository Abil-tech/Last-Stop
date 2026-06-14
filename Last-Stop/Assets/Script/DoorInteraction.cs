using UnityEngine;
using UnityEngine.UI;

public class DoorInteraction : MonoBehaviour
{
    // A static variable that any script can read to see what the player chose last
    public static bool lastChoseRightDoor = true;

    private bool playerZone = false; //
    
    [Header("Door Configuration")]
    public bool isRightDoor = true; // Check this TRUE for the right door, UNCHECK for left door[cite: 7]

    [Header("Links")]
    public SceneFader sceneFader; //[cite: 7]
    public GameObject interactButtonUI; //[cite: 7]

    [Header("Door Audio Assets")]
    public AudioSource doorAudioSource; // Drag your AudioSource component here
    public AudioClip doorOpenClip;     // Drag your handle click or door slide SFX here

    private Button targetButtonComponent; // Cached component to handle dynamic clicks[cite: 7]

    void Start()
    {
        if (interactButtonUI != null) //[cite: 7]
        {
            interactButtonUI.SetActive(false); //[cite: 7]
        }
    }

    void Update()
    {
        if (playerZone && Input.GetKeyDown(KeyCode.E)) //[cite: 7]
        {
            Interact(); //[cite: 7]
        }
    }

    // Kept for backward compatibility with manual button mapping[cite: 7]
    public void OnInteractButtonPressed()
    {
        if (playerZone) //[cite: 7]
        {
            Interact(); //[cite: 7]
        }
    }

    void Interact()
    {
        if (sceneFader != null) //[cite: 7]
        {
            // Clean up the click listener right before fading so it doesn't double-trigger[cite: 7]
            if (targetButtonComponent != null) //[cite: 7]
            {
                targetButtonComponent.onClick.RemoveListener(Interact); //[cite: 7]
            }

            if (interactButtonUI != null) interactButtonUI.SetActive(false); //[cite: 7]
            
            // --- AUDIO TRIGGER ---
            // Play the physical door click or slide audio feedback instantly
            if (doorAudioSource != null && doorOpenClip != null)
            {
                doorAudioSource.PlayOneShot(doorOpenClip);
            }

            // Lock in the player's choice right before fading out[cite: 7]
            lastChoseRightDoor = isRightDoor; //[cite: 7]
            Debug.Log($"[Choice] Player interacted with the {(isRightDoor ? "RIGHT" : "LEFT")} door."); //[cite: 7]
            
            sceneFader.FadeToNextScene(); //[cite: 7]
        }
        else
        {
            Debug.LogError("[Door] SceneFader link is missing!"); //[cite: 7]
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //[cite: 7]
        {
            playerZone = true; //[cite: 7]
            if (interactButtonUI != null) //[cite: 7]
            {
                interactButtonUI.SetActive(true); //[cite: 7]
                
                // Change button text dynamically based on which door it is[cite: 7]
                Text btnText = interactButtonUI.GetComponentInChildren<Text>(); //[cite: 7]
                if (btnText != null) //[cite: 7]
                {
                    btnText.text = isRightDoor ? "ENTER NEXT" : "TURN BACK"; //[cite: 7]
                }

                // --- DECLARATION CONTEXT EXAMPLE ---
                // Dynamically look up and declare the button component reference in real-time
                targetButtonComponent = interactButtonUI.GetComponent<Button>(); //[cite: 7]
                if (targetButtonComponent != null) //[cite: 7]
                {
                    // Remove first to prevent double-binding loops if anything gets weird[cite: 7]
                    targetButtonComponent.onClick.RemoveListener(Interact); //[cite: 7]
                    targetButtonComponent.onClick.AddListener(Interact); //[cite: 7]
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //[cite: 7]
        {
            playerZone = false; //[cite: 7]
            if (interactButtonUI != null) //[cite: 7]
            {
                // Unbind this door's method when walking away so it doesn't hijack the button[cite: 7]
                if (targetButtonComponent != null) //[cite: 7]
                {
                    targetButtonComponent.onClick.RemoveListener(Interact); //[cite: 7]
                }
                interactButtonUI.SetActive(false); //[cite: 7]
            }
        }
    }
}