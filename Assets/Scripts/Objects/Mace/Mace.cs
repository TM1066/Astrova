using UnityEngine;

public class Mace : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.color = GameManager.GetCurrentUserColorFullAlpha();
    }
}
