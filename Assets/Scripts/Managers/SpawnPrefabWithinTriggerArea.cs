using UnityEngine;
using System.Collections.Generic;

public class SpawnPrefabWithinTriggerArea : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    [SerializeField] int amountInArea = 15; //Maybe keep in a 'System' Script if ya know what I mean
    public List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField] float minSize;
    [SerializeField] float maxSize;

    List<Vector3> objectPositions = new List<Vector3>();

    private BoxCollider2D objectSpawnArea;
    private float maxSpawnHeight;
    private float maxSpawnWidth;

    [SerializeField] Transform playerTransform;

    // objectt is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectSpawnArea = GetComponent<BoxCollider2D>();

        maxSpawnHeight = objectSpawnArea.size.y / 2;
        maxSpawnWidth = objectSpawnArea.size.x / 2;

        this.transform.position = GameObject.Find("SpaceShip").transform.position;

        SetObjects();

        Debug.Log($"Max Spawn Width: {maxSpawnWidth}, Max Spawn Height: {maxSpawnHeight}");

    }

    private void OnTriggerExit2D (Collider2D other) // reset stars when the Player leaves the starred area
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.transform.position = other.transform.position;

            SetObjects(); // remake objects
        }
    }

    // private void GetRidOfNonVisibleStars()
    // {
    //     foreach (GameObject star in stars)
    //     {
    //         if (star != null)
    //         {
    //             // Check if the asteroid is outside the screen bounds
    //             if (star.GetComponent<SpriteRenderer>().IsVisible != true) // z < 0 means the object is behind the camera
    //             {
    //                 Destroy(star); // Remove the asteroid if it's not visible
    //             }
    //             else
    //             {
    //                 // For debugging: Color the visible asteroids
    //                 //asteroid.GetComponent<SpriteRenderer>().color = Color.blue;
    //             }
    //         }
    //     }
    // }

    private void SpawnObject(Vector2 spawnLocation)
    {
        GameObject objectInstance = Instantiate(objectPrefab);

        // Get camera bounds to avoid spawning inside the camera's view
        float cameraHalfHeight = Camera.main.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

        bool isInsideCamera;
        float padding = 5f;
        int attempts = 0; // Safety counter
        int maxAttempts = 100; // Cap to prevent infinite loops

        do // Generate a valid spawn location
        {
            isInsideCamera = spawnLocation.x > (playerTransform.position.x - cameraHalfWidth - padding) &&
                            spawnLocation.x < (playerTransform.position.x + cameraHalfWidth + padding) &&
                            spawnLocation.y > (playerTransform.position.y - cameraHalfHeight - padding) &&
                            spawnLocation.y < (playerTransform.position.y + cameraHalfHeight + padding);

            if (isInsideCamera)
            {
                // Re-generate spawn location
                spawnLocation = new Vector2(
                    UnityEngine.Random.Range(this.transform.position.x - maxSpawnWidth + 5, this.transform.position.x + maxSpawnWidth - 5),
                    UnityEngine.Random.Range(this.transform.position.y - maxSpawnHeight + 5, this.transform.position.y + maxSpawnHeight - 5)
                );
            }

            attempts++;
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Spawnobject: Could not find a valid spawn position after multiple attempts.");
                return; // Bail out to prevent crashing
            }

        } while (isInsideCamera);

        // Set up the object properties
        float randomScale = UnityEngine.Random.Range(minSize, maxSize);
        objectInstance.transform.localScale = new Vector3(randomScale, randomScale, 1);
        objectInstance.transform.position = new Vector3(spawnLocation.x, spawnLocation.y);

        // Parent the object to this GameObject and add to list
        objectInstance.transform.SetParent(this.transform);
        spawnedObjects.Add(objectInstance);
    }



    private void SetObjects()
    {
        objectPositions.Clear();

        foreach (GameObject gO in spawnedObjects)
        {
            Destroy(gO);
        }
        spawnedObjects.Clear();

        for (int i = 0; i < amountInArea; i++)
        {
            // Generate a new random position for each object
            Vector2 randomSpawnLocation = new Vector2(
                UnityEngine.Random.Range(this.transform.position.x - maxSpawnWidth + 50, this.transform.position.x + maxSpawnWidth - 50),
                UnityEngine.Random.Range(this.transform.position.y - maxSpawnHeight + 50, this.transform.position.y + maxSpawnHeight - 50)
            );

            SpawnObject(randomSpawnLocation);
        }
    }


    void OnLevelWasLoaded(int level)
    {
        foreach (GameObject gO in spawnedObjects)
        {
            GameObject.Destroy(gO.gameObject);
        }
    }
}
