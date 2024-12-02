using UnityEngine;
using System.Collections.Generic;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] GameObject starPrefab;
    //[SerializeField] int starDensity = 15; Maybe keep in a 'System' Script if ya know what I mean
    public List<GameObject> stars = new List<GameObject>();

    private BoxCollider2D starSpawnArea;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starSpawnArea = GetComponent<BoxCollider2D>();

        SetStarBG();
    }

    private void SetStarBG()
    {
        List<Vector3> starPositions = new List<Vector3>();


        for (int i = 0; i < 5000; i++)
        {
            GameObject starInstance = Instantiate(starPrefab);

            float maxWidth = starSpawnArea.size.x / 2;
            float maxHeight = starSpawnArea.size.y / 2;

            starInstance.transform.position = new Vector3(UnityEngine.Random.Range(-maxWidth + 10, maxWidth - 10), UnityEngine.Random.Range(-maxHeight, maxHeight), 100);

            float randomScale = UnityEngine.Random.Range(0.1f, 1f);

            starInstance.transform.localScale = new Vector3(randomScale, randomScale, 1);

            foreach (Vector3 exisitingStarPosition in starPositions) // checks if new star is nearby any current stars
            {
                while (Vector3.Distance( new Vector3 (starInstance.transform.position.x - exisitingStarPosition.x, starInstance.transform.position.y - exisitingStarPosition.y, 100), new Vector3 (2,2,0)) > 2) //Gets distance between the 2 checked stars
                {
                    starInstance.transform.position = new Vector3(UnityEngine.Random.Range(-maxWidth, maxWidth), UnityEngine.Random.Range(maxHeight, maxHeight), 100);
                }
            }

            starInstance.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, UnityEngine.Random.Range(0.1f, 0.8f));

            starInstance.transform.SetParent(this.transform);

            stars.Add(starInstance);
        } 
    }

    private void OnTriggerExit2D(Collider2D other) {
        
    }

}
