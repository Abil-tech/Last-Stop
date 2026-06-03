using UnityEngine;
using UnityEngine.UI;

public class DoorInteraction : MonoBehaviour
{
    // A static variable that any script can read to see what the player chose last
    public static bool lastChoseRightDoor = true;

    private bool playerZone = false;
    
    [Header("Door Configuration")]
    public bool isRightDoor = true; // Check this TRUE for the right door, UNCHECK for left door

    [Header("Links")]
    public SceneFader sceneFader; 
    public GameObject interactButtonUI; 

    private Button targetButtonComponent; // Cached component to handle dynamic clicks

    void Start()
    {
        if (interactButtonUI != null)
        {
            interactButtonUI.SetActive(false);
        }
    }

    void Update()
    {
        if (playerZone && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    // You can keep this here, but the dynamic listener below makes inspector mapping obsolete
    public void OnInteractButtonPressed()
    {
        if (playerZone)
        {
            Interact();
        }
    }

    void Interact()
    {
        if (sceneFader != null)
        {
            // Clean up the click listener right before fading so it doesn't double-trigger
            if (targetButtonComponent != null)
            {
                targetButtonComponent.onClick.RemoveListener(Interact);
            }

            if (interactButtonUI != null) interactButtonUI.SetActive(false);
            
            // Lock in the player's choice right before fading out
            lastChoseRightDoor = isRightDoor;
            Debug.Log($"[Choice] Player interacted with the {(isRightDoor ? "RIGHT" : "LEFT")} door.");
            
            sceneFader.FadeToNextScene();
        }
        else
        {
            Debug.LogError("[Door] SceneFader link is missing!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerZone = true;
            if (interactButtonUI != null)
            {
                interactButtonUI.SetActive(true);
                
                // Change button text dynamically based on which door it is
                Text btnText = interactButtonUI.GetComponentInChildren<Text>();
                if (btnText != null)
                {
                    btnText.text = isRightDoor ? "ENTER NEXT" : "TURN BACK";
                }

                // DYNAMICALLY BIND THE CLICK EVENT TO THIS SPECIFIC DOOR IN REAL-TIME
                targetButtonComponent = interactButtonUI.GetComponent<Button>();
                if (targetButtonComponent != null)
                {
                    // Remove first to prevent double-binding loops if anything gets weird
                    targetButtonComponent.onClick.RemoveListener(Interact); 
                    targetButtonComponent.onClick.AddListener(Interact);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerZone = false;
            if (interactButtonUI != null)
            {
                // Unbind this door's method when walking away so it doesn't hijack the button
                if (targetButtonComponent != null)
                {
                    targetButtonComponent.onClick.RemoveListener(Interact);
                }
                interactButtonUI.SetActive(false);
            }
        }
    }
}