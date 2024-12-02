using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorBasedOnPlayer : MonoBehaviour
{

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] UnityEngine.UI.Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = GameManager.GetCurrentUserColorFullAlpha();
        }
        if (image != null)
        {
            image.color = GameManager.GetCurrentUserColorFullAlpha();
        }
    }
}
