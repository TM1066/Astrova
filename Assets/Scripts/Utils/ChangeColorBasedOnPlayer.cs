using UnityEngine;
using UnityEngine.UI;

public class ChangeColorBasedOnPlayer : MonoBehaviour
{

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] UnityEngine.UI.Image image;
    [SerializeField] ParticleSystem particleSystem;

    [SerializeField] bool complimentaryColor = false;
    [SerializeField] bool dependOnColorfulShipsOption = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spriteRenderer != null)
        {
            if (dependOnColorfulShipsOption | complimentaryColor)
            {
                if (GameManager.GetColorfulShipsEnabled() | complimentaryColor)
                {
                    spriteRenderer.color = ScriptUtils.GetComplimentaryColor(GameManager.GetCurrentUserColorFullAlpha());
                }
            }
            else 
            {
                spriteRenderer.color = GameManager.GetCurrentUserColorFullAlpha();
            }
        }
        if (image != null)
        {
            if (dependOnColorfulShipsOption | complimentaryColor)
            {
                if (GameManager.GetColorfulShipsEnabled() | complimentaryColor)
                {
                    image.color = ScriptUtils.GetComplimentaryColor(GameManager.GetCurrentUserColorFullAlpha());
                }
            }
            else 
            {
                image.color = GameManager.GetCurrentUserColorFullAlpha();
            }
        }
        if (particleSystem != null)
        {
            var particleSystemMain = particleSystem.main;
            if (dependOnColorfulShipsOption | complimentaryColor)
            {
                if (GameManager.GetColorfulShipsEnabled() | complimentaryColor)
                {
                    particleSystemMain.startColor = ScriptUtils.GetComplimentaryColor(GameManager.GetCurrentUserColorFullAlpha());
                }
            }
            else 
            {
                particleSystemMain.startColor = GameManager.GetCurrentUserColorFullAlpha();
            }

            

            
        }
    }
}
