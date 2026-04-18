using UnityEngine;
using TMPro; 

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private TextMeshProUGUI textComponent; 
    
    private Color textColor;
    private float lifetime = 1.5f;

    void Start()
    {
        if (textComponent != null)
        {
            textColor = textComponent.color;
        }
    }

    void Update()
    {
        if (textComponent == null) return;

        // THE FIX: Unscaled time so the timer still ticks while the game is paused!
        lifetime -= Time.unscaledDeltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
            return; 
        }

        // Float upwards
        transform.position += Vector3.up * moveSpeed * Time.unscaledDeltaTime;

        // Fade out smoothly
        textColor.a -= fadeSpeed * Time.unscaledDeltaTime;
        textComponent.color = textColor;
    }
    // Add this right below the Update() function!
    public void SetText(string newText)
    {
        if (textComponent != null)
        {
            textComponent.text = newText;
        }
    }
}