using UnityEngine;

public class PowerUpKit : MonoBehaviour
{
    [SerializeField] string powerUpKey;

    [SerializeField] AudioClip pickUpSound;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player") && !other.GetComponent<PlayerShip>().GetIsDead())
        {
            //StartCoroutine(other.gameObject.GetComponent<PlayerShip>().ActivatePowerUp(powerUpKey)); //IT EXISTS HERE AND THEN IT GETS DESTROYED, call on Player script
            ScriptUtils.FindPlayerScript().ActivatePowerUpHandler(powerUpKey);
            ScriptUtils.PlaySound(null,pickUpSound);
            Destroy(this.gameObject);
        }
    }
}
