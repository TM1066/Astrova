using UnityEngine;
using System.Collections.Generic;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] GameObject starPrefab;
    [SerializeField] int starAmount = 15; //Maybe keep in a 'System' Script if ya know what I mean
    public List<GameObject> stars = new List<GameObject>();

    List<Vector3> starPositions = new List<Vector3>();

    private BoxCollider2D starSpawnArea;
    private float maxSpawnHeight;
    private float maxSpawnWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starSpawnArea = GetComponent<BoxCollider2D>();

        maxSpawnHeight = starSpawnArea.size.y / 2;
        maxSpawnWidth = starSpawnArea.size.x / 2;

        SetStarBG();
    }

    private void OnTriggerExit2D (Collider2D other) // reset stars when the Player leaves the starred area
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            other.transform.position = new Vector3(0,0,0); // recentre player instead of reset all the stars because that's costly and the physics engine is panicking when the player gets too far away

            //SetStarBG(); // remake stars
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

    private void SpawnStar(Vector2 spawnLocation)
    {
        GameObject starInstance = Instantiate(starPrefab);

        // Get camera bounds to avoid spawning inside the camera's view
        float cameraHalfHeight = Camera.main.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

        // do // Generate a valid spawn location
        // {
        //     isInsideCamera = spawnLocation.x > (playerTransform.position.x - cameraHalfWidth - padding) &&
        //                     spawnLocation.x < (playerTransform.position.x + cameraHalfWidth + padding) &&
        //                     spawnLocation.y > (playerTransform.position.y - cameraHalfHeight - padding) &&
        //                     spawnLocation.y < (playerTransform.position.y + cameraHalfHeight + padding);

        //     if (isInsideCamera)
        //     {
        //         // Re-generate spawn location
        //         spawnLocation = new Vector2(
        //             UnityEngine.Random.Range(this.transform.position.x - maxSpawnWidth + 5, this.transform.position.x + maxSpawnWidth - 5),
        //             UnityEngine.Random.Range(this.transform.position.y - maxSpawnHeight + 5, this.transform.position.y + maxSpawnHeight - 5)
        //         );
        //     }

        //     attempts++;
        //     if (attempts >= maxAttempts)
        //     {
        //         Debug.LogWarning("SpawnStar: Could not find a valid spawn position after multiple attempts.");
        //         return; // Bail out to prevent crashing
        //     }

        // } while (isInsideCamera);

        // Set up the star properties
        float randomScale = UnityEngine.Random.Range(0.1f, 1f);
        starInstance.transform.localScale = new Vector3(randomScale, randomScale, 1);
        starInstance.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, UnityEngine.Random.Range(0.1f, 0.8f));
        starInstance.transform.position = new Vector3(spawnLocation.x, spawnLocation.y, 500);

        // Parent the star to this GameObject and add to list
        starInstance.transform.SetParent(this.transform);
        stars.Add(starInstance);
    }



    private void SetStarBG()
    {
        starPositions.Clear();

        foreach (GameObject star in stars)
        {
            Destroy(star);
        }
        stars.Clear();

        for (int i = 0; i < starAmount; i++)
        {
            // Generate a new random position for each star
            Vector2 randomSpawnLocation = new Vector2(
                UnityEngine.Random.Range(this.transform.position.x - maxSpawnWidth, this.transform.position.x + maxSpawnWidth),
                UnityEngine.Random.Range(this.transform.position.y - maxSpawnHeight, this.transform.position.y + maxSpawnHeight)
            );

            SpawnStar(randomSpawnLocation);
        }
    }


    void OnLevelWasLoaded(int level)
    {
        foreach (GameObject star in stars)
        {
            GameObject.Destroy(star.gameObject);
        }
    }
}
