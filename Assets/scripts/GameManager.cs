using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Turret Room visibility")]
    [SerializeField] private CanvasGroup turretRoomGroup;
    // Singleton
    public static GameManager Instance;

    [SerializeField] private float inc_sl = 0.2f;

    public int currentScrap = 0;
    public TextMeshProUGUI scrapText;
    [SerializeField] private TextMeshProUGUI visionButtonText;
    [SerializeField] private TextMeshProUGUI turretButtonText;

    // Upgrade Variables
    [SerializeField] private Transform spotlightMask;
    [SerializeField] private int upgradeCost = 5;

    // Turret Variables
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private int turretCost = 10;
    private int turretCount = 0;

    // Turret Grid Spawning
    [SerializeField] private Vector3 turretStartPos;
    [SerializeField] private float xSpacing = 2f;
    [SerializeField] private int turretsPerRow = 5;

    // UI Screens
    [SerializeField] private GameObject shopMenuScreen;
    [SerializeField] private GameObject turretScreen;

    // Upgrade limit tracking
    private int upgradeCount = 0;
    private const int MaxUpgrades = 10;

    // Exponential cost growth (e.g. 1.6 = 60% more expensive each time)
    [SerializeField] private float costScaleFactor = 1.6f;

    // Animation settings
    [SerializeField] private float expandDuration = 0.4f;      // Normal upgrade animation time
    [SerializeField] private float finalExpandDuration = 1.5f; // Final reveal animation time
    private bool isAnimating = false;

    void Awake()
    {
        if (Instance == null) Instance = this;

        // Initialize button texts
        if (visionButtonText != null)
            visionButtonText.text = "Upgrade Vision (" + upgradeCost + " Scrap)";
        if (turretButtonText != null)
            turretButtonText.text = "Buy Turret (" + turretCost + " Scrap)";
    }

    // --- UI Menu ---
public void OpenShop()
    {
        if (shopMenuScreen != null) shopMenuScreen.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void CloseShop()
    {
        if (shopMenuScreen != null) shopMenuScreen.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    void Update()
    {
        // Toggle the shop open/closed with Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (shopMenuScreen != null && shopMenuScreen.activeSelf)
                CloseShop();
            else
                OpenShop();
        }
    }

    public void AddScrap(int amount)
    {
        currentScrap += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        scrapText.text = "Scrap: " + currentScrap;
    }

    public void BuyVisionUpgrade()
    {
        Debug.Log($"BuyVisionUpgrade called! Scrap: {currentScrap}, Cost: {upgradeCost}, Upgrades: {upgradeCount}/{MaxUpgrades}");

        // Block if animation is playing or already maxed
        if (isAnimating) return;
        if (upgradeCount >= MaxUpgrades)
        {
            Debug.Log("Vision fully upgraded!");
            return;
        }

        if (currentScrap >= upgradeCost)
        {
            currentScrap -= upgradeCost;
            upgradeCount++;

            if (upgradeCount >= MaxUpgrades)
            {
                // Final upgrade: animate outward to cover full screen (keep active!)
                StartCoroutine(AnimateSpotlight(new Vector3(100f, 100f, 1f), finalExpandDuration));
                Debug.Log("Max upgrades reached! Full screen revealed!");
            }
            else
            {
                // Normal upgrade: animate the spotlight growing
                Vector3 targetScale = spotlightMask.localScale + new Vector3(inc_sl, inc_sl, 0f);
                StartCoroutine(AnimateSpotlight(targetScale, expandDuration));

                // Scale cost exponentially
                upgradeCost = Mathf.RoundToInt(upgradeCost * costScaleFactor);

                // Update vision button text
                if (visionButtonText != null)
                    visionButtonText.text = "Upgrade Vision (" + upgradeCost + " Scrap)";

                Debug.Log($"Upgraded Vision! Next cost: {upgradeCost} | Upgrades: {upgradeCount}/{MaxUpgrades}");
            }

            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough scrap!");
        }
    }

    // Smoothly animates the spotlight to a target scale using an ease-out curve
    private IEnumerator AnimateSpotlight(Vector3 targetScale, float duration)
    {
        isAnimating = true;

        Vector3 startScale = spotlightMask.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Cubic ease-out: snappy start, smooth finish
            t = 1f - Mathf.Pow(1f - t, 3f);

            spotlightMask.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        // Ensure we land exactly on the target
        spotlightMask.localScale = targetScale;

        isAnimating = false;
    }

   public void BuyAutoTurret()
    {
        if (currentScrap >= turretCost)
        {
            currentScrap -= turretCost;
            
            // THE FIX: Add "false" to the end! 
            // This forces the UI element to stay inside the menu properly.
            GameObject newTurret = Instantiate(turretPrefab, turretScreen.transform, false);
            
            // Failsafe: Force the scale back to normal just in case the prefab got warped!
            newTurret.transform.localScale = Vector3.one;
            
            turretCost += 15; 
            
            if (turretButtonText != null)
            {
                turretButtonText.text = "Buy Turret (" + turretCost + " Scrap)";
            }
            
            UpdateUI();
        }
    }
    public void OpenTurretRoom()
    {
        turretRoomGroup.alpha = 1f; // Make it fully visible
        turretRoomGroup.blocksRaycasts = true; // Make buttons clickable
        turretRoomGroup.interactable = true;
    }

    public void CloseTurretRoom()
    {
        turretRoomGroup.alpha = 0f; // Make it completely invisible
        turretRoomGroup.blocksRaycasts = false; // Stop invisible buttons from being clicked
        turretRoomGroup.interactable = false;
    }
}