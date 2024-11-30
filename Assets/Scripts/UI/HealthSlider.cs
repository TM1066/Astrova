using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{

    private PlayerShip player;

    [SerializeField] UnityEngine.UI.Slider healthSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("SpaceShip").GetComponent<PlayerShip>();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = player.GetHealth();
    }
}
