using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] [Range(0.1f, 5f)] private float spawnRate = 1.5f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnAsteroid();
            timer = 0f;
        }
    }

    private void SpawnAsteroid()
    {
        float randomX = Random.Range(-8f, 8f);
        float randomY = Random.Range(-4.5f, 4.5f);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
    }
}
