using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] Transform playerTransform;

    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] List<Sprite> asteroidPossibleSprites = new List<Sprite>();

    private BoxCollider2D triggerArea;

    // private UnityEngine.Vector2[,] possibleAsteroidSpawns; - Not what i'm doing but it should beeeee

    [SerializeField] int asteroidAmount;

    private float maxSpawnHeight;
    private float maxSpawnWidth;

    private List<GameObject> currentAsteroids = new List<GameObject>();


    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("The Player Has left the Asteroid Zone");
            
            this.transform.position = other.transform.position;

            PopulateAsteroids(); // remake asteroids
            this.transform.position = GameObject.Find("SpaceShip").transform.position;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triggerArea = this.GetComponent<BoxCollider2D>();

        maxSpawnHeight = triggerArea.size.y;
        maxSpawnWidth = triggerArea.size.x;

        this.transform.position = GameObject.Find("SpaceShip").transform.position;

        PopulateAsteroids();
    }

    private void SpawnAsteroid (UnityEngine.Vector2 spawnLocation, Transform parent)
    {
        GameObject asteroid = Instantiate(asteroidPrefab,this.transform);

        asteroid.GetComponent<SpriteRenderer>().sprite = asteroidPossibleSprites[UnityEngine.Random.Range(0,asteroidPossibleSprites.Count)]; // Randomise Sprite
        ScriptUtils.RegeneratePolygonCollider2DPoints(asteroid.GetComponent<PolygonCollider2D>(), asteroid.GetComponent<SpriteRenderer>().sprite); // Regenerate Collider Mesh Depending on Sprite

        asteroid.GetComponent<Rigidbody2D>().linearVelocity = new UnityEngine.Vector2(UnityEngine.Random.Range(-5,5), UnityEngine.Random.Range(-5,5)); // Random Direction
        asteroid.GetComponent<Rigidbody2D>().angularVelocity = UnityEngine.Random.Range(-80f,80f); // Random Spin

        asteroid.transform.SetParent(parent);

        asteroid.transform.position = spawnLocation;

        currentAsteroids.Add(asteroid);
    }

    private void GetRidOfNonVisibleAsteroids()
{
    foreach (GameObject asteroid in currentAsteroids)
    {
        if (asteroid != null)
        {
            UnityEngine.Vector3 screenPosition = Camera.main.WorldToScreenPoint(asteroid.transform.position);

            // Check if the asteroid is outside the screen bounds
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height || screenPosition.z < 0) // z < 0 means the object is behind the camera
            {
                Destroy(asteroid); // Remove the asteroid if it's not visible
            }
            else
            {
                // For debugging: Color the visible asteroids
                asteroid.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
    }
}

    private void PopulateAsteroids()
    {

        GetRidOfNonVisibleAsteroids();

        for (int i = 0; i < asteroidAmount; i++)
        {
        if (UnityEngine.Random.Range(1, 3) == 2)
            {
                SpawnAsteroid(new UnityEngine.Vector2(UnityEngine.Random.Range(-maxSpawnWidth + 5, maxSpawnWidth - 5), UnityEngine.Random.Range(-maxSpawnHeight + 5, maxSpawnWidth - 5)),this.transform);
            }
        }
    }
}
