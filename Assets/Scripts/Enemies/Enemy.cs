using UnityEngine;

public class Enemy : MonoBehaviour // Parent Enemy Template
{
    private GameObject player;
    [SerializeField] float angleOffset = 0f; // For making other sides face the Player 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("SpaceShip");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += angleOffset;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
