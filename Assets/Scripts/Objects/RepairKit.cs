using UnityEngine;

public class RepairKit : MonoBehaviour
{
    [SerializeField] float healAmount;


    // private void OnCollisionEnter2D(Collision2D other) 
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         other.gameObject.GetComponent<PlayerShip>().IncreaseHealth(healAmount);
    //     }

    //     Destroy(this.gameObject);
    // }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerShip>().IncreaseHealth(healAmount);
        }

        Destroy(this.gameObject);
    }
}
