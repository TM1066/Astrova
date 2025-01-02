using UnityEngine;

public class RepairKit : MonoBehaviour
{
    [SerializeField] float healAmount;

    [SerializeField] AudioClip pickUpSound;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player") && !other.GetComponent<PlayerShip>().GetIsDead())
        {
            other.gameObject.GetComponent<PlayerShip>().IncreaseHealth(healAmount);
            ScriptUtils.PlaySound(null,pickUpSound);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            //Restore Shield Condition
        }

        
    }
}
