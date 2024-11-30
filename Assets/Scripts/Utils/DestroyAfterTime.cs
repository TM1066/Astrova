using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float lifeTimeInSeconds = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyAfterSeconds(lifeTimeInSeconds)); 
    }
 
    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        float timePassed = 0f;

        while (timePassed < seconds)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
