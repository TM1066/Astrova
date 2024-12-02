using UnityEngine;

public class RepairKit : MonoBehaviour
{
    [SerializeField] float healAmount;

    [SerializeField] AudioClip pickUpSound;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerShip>().IncreaseHealth(healAmount);
            ScriptUtils.PlaySound(null,pickUpSound);
            Destroy(this.gameObject);
        }

        
    }
}
