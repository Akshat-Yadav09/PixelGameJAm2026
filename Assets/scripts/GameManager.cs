using UnityEngine;
using TMPro; // We need this to talk to UI text!

public class GameManager : MonoBehaviour
{
    // This makes it easy for other scripts to find the GameManager
    public static GameManager Instance; 

    public int currentScrap = 0;
    public TextMeshProUGUI scrapText; // The UI text on screen
    
    // Upgrade Variables
    public Transform spotlightMask; // The thing that gets bigger
    public int upgradeCost = 10;

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
        if (currentScrap >= upgradeCost)
        {
            currentScrap -= upgradeCost; // Pay the cost
            UpdateUI(); // Update the text

            // Make the spotlight 50% bigger!
            spotlightMask.localScale += new Vector3(0.5f, 0.5f, 0f); 
            
            // Increase the cost for the next time
            upgradeCost += 15; 
            Debug.Log("Upgraded Vision! Next cost: " + upgradeCost);
        }
        else
        {
            Debug.Log("Not enough scrap!");
        }
    }
}