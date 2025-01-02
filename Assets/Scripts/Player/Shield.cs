using UnityEngine;

public class Shield : MonoBehaviour
{
    public float health = 1;

    public Collider2D collider;
    public SpriteRenderer spriteRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.color.a <= 0.05f)
        {
            collider.enabled = false;
        }
        else 
        {
            collider.enabled = true;
        }
    }

    void DecreaseHealth(float damage)
    {
        health -= damage;
    }
}
