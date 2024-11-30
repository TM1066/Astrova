using UnityEngine;

public class Projectile : MonoBehaviour
{


    private float projectileDamage = 0.33f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectileDamage = GameObject.Find("SpaceShip").GetComponent<PlayerShip>().GetProjectileDamage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetDamage()
    {
        return projectileDamage;
    }
}
