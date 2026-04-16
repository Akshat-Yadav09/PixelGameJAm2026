using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] [Range(1f, 10f)] private float speed = 2f;
    private Vector3 driftDirection;

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
        
        Destroy(gameObject);
    }
}
