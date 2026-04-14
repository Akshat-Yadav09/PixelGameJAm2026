using UnityEngine;

public class ScopeController : MonoBehaviour
{
    [Range(1f, 20f)]
    [SerializeField] public float scopeSpeed = 10f; // Lower number = heavier/slower turret

    void Update()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0f;
        
        // Smoothly glide the scope towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * scopeSpeed);
    }
}