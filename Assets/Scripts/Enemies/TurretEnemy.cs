using UnityEngine;
using System.Collections;

public class TurretEnemy : MonoBehaviour
{
    private GameObject player;

    private SpriteRenderer spriteRenderer;
    private Color thisColor;

    [SerializeField] float angleOffset;

    [SerializeField] GameObject projectilePrefab;

    [SerializeField] float projectileDamage = 0.22f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("SpaceShip");
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = ScriptUtils.GetComplimentaryColor(GameManager.GetCurrentUserColorFullAlpha());
        thisColor = spriteRenderer.color;

        StartCoroutine(ShootHandler());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += this.angleOffset;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private IEnumerator ShootHandler()
    {
        yield return new WaitForSeconds(3f);

        while (gameObject && player.GetComponent<PlayerShip>().GetHealth() > player.GetComponent<PlayerShip>().GetMinHealth())
        {
            //Shootings
            if (Random.Range(0, 2) == 1)
            {
                Vector2 offset = -this.transform.up * 1.5f; // 'up' is relative to the ship's rotation

                GameObject projectile = Instantiate(projectilePrefab, (Vector2) this.transform.position + offset, this.transform.localRotation); // Offset so it's not inside the enemy
                projectile.GetComponent<Projectile>().SetDamage(projectileDamage);
                projectile.gameObject.transform.SetParent(transform);

                //Change Projectile Color to match lights
                projectile.GetComponent<SpriteRenderer>().color = new Color (thisColor.r, thisColor.g, thisColor.b, 1f);

                StartCoroutine(ScriptUtils.PositionLerp(projectile.transform, projectile.transform.position, -this.transform.up * 1000,  25.5f));
            }
            yield return new WaitForSeconds(1.5f); // Cooldown
        }   
    }
}
