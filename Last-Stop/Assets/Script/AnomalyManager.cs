using UnityEngine;
using System.Collections.Generic;

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager Instance;

    [Header("Loop Progression")]
    public int currentFloor = 0;
    public int escapeFloorTarget = 8;

    [Header("Anomaly Settings")]
    [Range(0f, 1f)] public float anomalyChance = 0.5f; // 50% chance to roll a scare
    public bool isAnomalyActive = false;

    [Header("Corridor Assets")]
    public GameObject normalHallwayAssets; // Parent container for normal decor
    public List<GameObject> anomalyVariants; // List for your creepy containers

    private int activeAnomalyIndex = -1;

    void Awake()
    {
        // Keep this script alive across scene reloads to track floor progress
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        GenerateLevelState();
    }

    // This runs every single time the corridor scene loads up!
    public void GenerateLevelState()
    {
        // Reset everything to normal state first
        if (normalHallwayAssets != null) normalHallwayAssets.SetActive(true);
        
        foreach (GameObject anomaly in anomalyVariants)
        {
            if (anomaly != null) anomaly.SetActive(false);
        }

        // Roll the digital dice to see if this loop is cursed
        isAnomalyActive = Random.value < anomalyChance;

        // Force normal layout on Floor 0 so players have a baseline reference
        if (currentFloor == 0) isAnomalyActive = false;

        if (isAnomalyActive && anomalyVariants.Count > 0)
        {
            // Pick exactly ONE random anomaly folder from your list to activate
            activeAnomalyIndex = Random.Range(0, anomalyVariants.Count);
            if (anomalyVariants[activeAnomalyIndex] != null)
            {
                anomalyVariants[activeAnomalyIndex].SetActive(true);
                Debug.Log($"[Spooky] Anomaly #{activeAnomalyIndex} has manifested!");
            }
        }
        else
        {
            activeAnomalyIndex = -1;
            Debug.Log($"[Normal] The corridor is safe. Floor: {currentFloor}");
        }
    }

    // Called if player picks forward on normal or turns around on an anomaly
    public void AdvanceFloor()
    {
        currentFloor++;
        Debug.Log($"[Progress] Correct choice! Advanced to Floor {currentFloor}");

        if (currentFloor >= escapeFloorTarget)
        {
            Debug.Log("[Victory] You found the exit! Level cleared!");
            currentFloor = 0; // Reset loop for next run
        }
    }

    // Called if they walk forward straight into a glitch (or run back from a normal room)
    public void ResetToBeginning()
    {
        Debug.Log("[Caught] WRONG! The loop resets your mind back to Floor 0.");
        currentFloor = 0;
    }
}