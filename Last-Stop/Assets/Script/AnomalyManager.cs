using UnityEngine;
using System.Collections.Generic;

public class AnomalyManager : MonoBehaviour
{
    // Static variables stay alive in system memory across reloads automatically!
    public static int currentFloor = 0;
    public static bool isAnomalyActive = false;

    [Header("Loop Progression")]
    public int escapeFloorTarget = 8;

    [Header("Anomaly Settings")]
    [Range(0f, 1f)] public float anomalyChance = 0.5f; 

    [Header("Corridor Assets")]
    public GameObject normalHallwayAssets; 
    public List<GameObject> anomalyVariants; 

    private int activeAnomalyIndex = -1;

    void Start()
    {
        // Fires fresh right as the scene finishes loading up
        GenerateLevelState();
    }

    public void GenerateLevelState()
    {
        // Reset all anomalies to hidden first
        foreach (GameObject anomaly in anomalyVariants)
        {
            if (anomaly != null) anomaly.SetActive(false);
        }

        // Roll the digital dice
        isAnomalyActive = Random.value < anomalyChance;

        // Floor 0 is always the safe baseline tutorial floor
        if (currentFloor == 0) isAnomalyActive = false;

        if (isAnomalyActive && anomalyVariants.Count > 0)
        {
            // Disable the normal variants (chairs/windows), NOT the whole background!
            if (normalHallwayAssets != null) normalHallwayAssets.SetActive(false);

            activeAnomalyIndex = Random.Range(0, anomalyVariants.Count);
            if (anomalyVariants[activeAnomalyIndex] != null)
            {
                anomalyVariants[activeAnomalyIndex].SetActive(true);
                Debug.Log($"[Spooky] Floor {currentFloor}: Anomaly #{activeAnomalyIndex} manifested!");
            }
        }
        else
        {
            activeAnomalyIndex = -1;
            if (normalHallwayAssets != null) normalHallwayAssets.SetActive(true);
            Debug.Log($"[Normal] Floor {currentFloor}: Hallway is totally safe.");
        }
    }

    public void AdvanceFloor()
    {
        currentFloor++;
        Debug.Log($"[Progress] Correct choice! Moving to Floor {currentFloor}");

        if (currentFloor >= escapeFloorTarget)
        {
            Debug.Log("[Victory] You escaped the train car loop!");
            currentFloor = 0; 
        }
    }

    public void ResetToBeginning()
    {
        Debug.LogWarning("[Caught] Dumb ways to die! Back to Floor 0.");
        currentFloor = 0;
    }
}