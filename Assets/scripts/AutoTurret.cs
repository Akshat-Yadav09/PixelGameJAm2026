using UnityEngine;

public class AutoTurret : MonoBehaviour
{
    [Header("Turret Room visibility")]
    [SerializeField] private CanvasGroup turretRoomGroup;
    
    [Header("Passive Income")]
    [SerializeField] private float generationRate = 2f; 
    [SerializeField] private int scrapPerCycle = 1;     
    private float nextGenerationTime;

    [Header("Visuals")]
    [SerializeField] private Transform turretSprite; 
    [SerializeField] [Range(1.1f, 2f)] private float pulseSize = 1.3f; 
    [SerializeField] [Range(1f, 10f)] private float recoverySpeed = 5f;

    void Start()
    {
        // Start the timer the moment it spawns using the real-world clock
        nextGenerationTime = Time.unscaledTime + generationRate;
    }

    void Update()
    {
        // 1. Shrink back to normal size using UNSCALED time so it animates while paused
        if (turretSprite != null)
        {
            turretSprite.localScale = Vector3.Lerp(turretSprite.localScale, Vector3.one, Time.unscaledDeltaTime * recoverySpeed);
        }

        // 2. The Passive Timer using UNSCALED time
        if (Time.unscaledTime >= nextGenerationTime)
        {
            GeneratePassiveScrap();
            nextGenerationTime = Time.unscaledTime + generationRate;
        }
    }

    void GeneratePassiveScrap()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScrap(scrapPerCycle);
        }

        if (turretSprite != null)
        {
            turretSprite.localScale = new Vector3(pulseSize, pulseSize, 1f);
        }
    }
}