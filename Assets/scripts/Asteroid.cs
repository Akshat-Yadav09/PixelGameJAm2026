using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] [Range(1f, 10f)] private float speed = 2f;
    private Vector3 driftDirection;
    [Header("Juice")]
    [SerializeField] private GameObject floatingTextPrefab;

    void Start()
    {
        driftDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
    }

    void Update()
    {
        transform.Translate(driftDirection * speed * Time.deltaTime);

        // Despawn once off-screen
        if (Mathf.Abs(transform.position.x) > 12f || Mathf.Abs(transform.position.y) > 8f)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScrap(1);
        }
        if (floatingTextPrefab != null)
        {
            // THE SHORTCUT: Automatically find the Canvas by its exact name!
            GameObject mainCanvas = GameObject.Find("Market");

            if (mainCanvas != null)
            {
                // Translate the 3D rock position into a 2D screen coordinate
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

                // Spawn the text onto the Canvas
                GameObject popUp = Instantiate(floatingTextPrefab, screenPosition, Quaternion.identity, mainCanvas.transform);
                popUp.transform.localScale = Vector3.one;
            }
        }
        
        Destroy(gameObject);
    }

}

