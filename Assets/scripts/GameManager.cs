using UnityEngine;
using TMPro; // We need this to talk to UI text!

public class GameManager : MonoBehaviour
{
    // This makes it easy for other scripts to find the GameManager
    public static GameManager Instance; 

    [SerializeField] private float inc_sl = 0.2f;

    public int currentScrap = 0;
    public TextMeshProUGUI scrapText; // The UI text on screen
    
    // Upgrade Variables
    public Transform spotlightMask; // The thing that gets bigger
    public int upgradeCost = 5;

    // Upgrade limit tracking
    private int upgradeCount = 0;
    private const int MaxUpgrades = 10;

    // Scale factor for exponential cost growth (e.g. 1.6 = 60% more expensive each time)
    [SerializeField] private float costScaleFactor = 1.6f;

    void Awake()
    {
        // Set up the Singleton
        if (Instance == null) Instance = this; 
    }

    // Other scripts will call this when an asteroid is clicked
    public void AddScrap(int amount)
    {
        currentScrap += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        scrapText.text = "Scrap: " + currentScrap;
    }

    // The Button will call this function
    public void BuyVisionUpgrade()
    {
        Debug.Log($"BuyVisionUpgrade called! Scrap: {currentScrap}, Cost: {upgradeCost}, Upgrades: {upgradeCount}/{MaxUpgrades}");

        // Already maxed out
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
                // Final upgrade: reveal the entire screen by setting a very large scale
                spotlightMask.localScale = new Vector3(100f, 100f, 1f);
                Debug.Log("Max upgrades reached! Full screen revealed!");
            }
            else
            {
                // Grow the spotlight by the increment
                spotlightMask.localScale = spotlightMask.localScale + new Vector3(inc_sl, inc_sl, 0f);

                // Scale cost exponentially: next cost = current cost * scaleFactor (rounded to int)
                upgradeCost = Mathf.RoundToInt(upgradeCost * costScaleFactor);
                Debug.Log($"Upgraded Vision! Next cost: {upgradeCost} | Upgrades: {upgradeCount}/{MaxUpgrades}");
            }

            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough scrap!");
        }
    }
}