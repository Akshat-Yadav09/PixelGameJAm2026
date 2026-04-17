using UnityEngine;

public class AutoTurret : MonoBehaviour
{
    [Header("Passive Income")]
    [SerializeField] private float generationRate = 2f; // Generates money every 2 seconds
    [SerializeField] private int scrapPerCycle = 1;     // How much it generates
    private float nextGenerationTime;

    [Header("Visuals (Optional)")]
    [SerializeField] private Transform turretSprite; // Drag your child sprite here!
    [SerializeField] [Range(1.1f, 2f)] private float pulseSize = 1.3f; // How big it gets when it generates scrap
    [SerializeField] [Range(1f, 10f)] private float recoverySpeed = 5f;

    void Update()
    {
        // 1. Smoothly shrink the sprite back to its normal size (1, 1, 1) every frame
        if (turretSprite != null)
        {
            turretSprite.localScale = Vector3.Lerp(turretSprite.localScale, Vector3.one, Time.deltaTime * recoverySpeed);
        }

        // 2. The Passive Timer
        if (Time.time >= nextGenerationTime)
        {
            GeneratePassiveScrap();
            
            // Reset the timer
            nextGenerationTime = Time.time + generationRate;
        }
    }

    void GeneratePassiveScrap()
    {
        // Add the money!
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScrap(scrapPerCycle);
        }

        // Create the "Pulse" visual effect by instantly making the sprite bigger
        if (turretSprite != null)
        {
            turretSprite.localScale = new Vector3(pulseSize, pulseSize, 1f);
        }
    }
}