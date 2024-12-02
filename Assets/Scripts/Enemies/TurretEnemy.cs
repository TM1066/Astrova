using UnityEngine;
using System.Collections;

public class TurretEnemy : MonoBehaviour
{
    private GameObject player;

    private SpriteRenderer chassisSpriteRenderer;
    private SpriteRenderer lightsSpriteRenderer;
    private Color thisColor;

    [SerializeField] float angleOffset;

    [SerializeField] GameObject projectilePrefab;

    [SerializeField] float health = 1f;

    [SerializeField] float projectileDamage = 0.22f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Setting up Variables
        player = GameObject.Find("SpaceShip");
        chassisSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        chassisSpriteRenderer.color = ScriptUtils.GetComplimentaryColor(GameManager.GetCurrentUserColorFullAlpha());
        thisColor = chassisSpriteRenderer.color;

        lightsSpriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
        lightsSpriteRenderer.color = thisColor;


        //Starting Coroutines
        StartCoroutine(ShootHandler());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += this.angleOffset;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (health < 0.1f)
        {
            Die();
        }
    }

    private IEnumerator ShootHandler()
    {
        yield return new WaitForSeconds(3f);

        while (gameObject && player.GetComponent<PlayerShip>().GetHealth() > player.GetComponent<PlayerShip>().GetMinHealth())
        {
            //Shootings
            if (Random.Range(0, 2) == 1 && Vector2.Distance(transform.position, player.transform.position) <= 12)
            {
                Vector2 offset = this.transform.up * 1.5f; // 'up' is relative to the ship's rotation

                GameObject projectile = Instantiate(projectilePrefab, (Vector2) this.transform.position + offset, this.transform.localRotation); // Offset so it's not inside the enemy
                projectile.GetComponent<Projectile>().SetDamage(projectileDamage);
                projectile.gameObject.transform.SetParent(transform);

                //Change Projectile Color to match lights
                projectile.GetComponent<SpriteRenderer>().color = new Color (thisColor.r, thisColor.g, thisColor.b, 1f);

                StartCoroutine(ScriptUtils.PositionLerp(projectile.transform, projectile.transform.position, this.transform.up * 1000,  25.5f));
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f,3f)); // Cooldown
        }   
    }

    private void Die()
    {
        GameManager.AddToCurrentScore(5);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            if (projectile != null)
            {
                float damage = projectile.GetDamage();
                Debug.Log($"Projectile hit! Damage: {damage}");

                health -= damage;
                Debug.Log($"Enemy health: {health}");

                Destroy(other.gameObject);
            }
            else
            {
                Debug.LogWarning("Projectile script not found on object!");
            }
        }
        else
        {
            Debug.Log($"Collision ignored with: {other.gameObject.name}");
        }
    }
}
