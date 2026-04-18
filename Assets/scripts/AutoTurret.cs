using UnityEngine;

public class AutoTurret : MonoBehaviour
{
    [Header("Passive Income")]
    [SerializeField] private float generationRate = 2f; 
    [SerializeField] private int scrapPerCycle = 1;     
    private float nextGenerationTime;

    [Header("Visuals")]
    [SerializeField] private RectTransform turretSprite; 
    [SerializeField] [Range(1.1f, 2f)] private float pulseSize = 1.3f; 
    [SerializeField] [Range(1f, 10f)] private float recoverySpeed = 5f;
    [SerializeField] private GameObject floatingTextPrefab; 

    void Start()
    {
        // Start the timer
        nextGenerationTime = Time.time + generationRate;
    }

    void Update()
    {
        // Smoothly shrink back to normal size
        if (turretSprite != null)
        {
            turretSprite.localScale = Vector3.Lerp(turretSprite.localScale, Vector3.one, Time.deltaTime * recoverySpeed);
        }

        // The Passive Timer (uses scaled time so it pauses when shop is open)
        if (Time.time >= nextGenerationTime)
        {
            GeneratePassiveScrap();
            nextGenerationTime = Time.time + generationRate;
        }
    }

    void GeneratePassiveScrap()
    {
        // 1. Add the Scrap
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScrap(scrapPerCycle);
        }

        // 2. Pulse the visual size
        if (turretSprite != null)
        {
            turretSprite.localScale = new Vector3(pulseSize, pulseSize, 1f);
        }

        // 3. Spawn the floating "+1" text
        if (floatingTextPrefab != null)
        {
            // Parent it to the UI panel so it stays visible!
            // (The Layout Element will stop the Grid from grabbing it)
            GameObject popUp = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform.parent);
            
            // Failsafe scale
            popUp.transform.localScale = Vector3.one;
        }
    }
}