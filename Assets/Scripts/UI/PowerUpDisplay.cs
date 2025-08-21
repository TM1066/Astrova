using UnityEngine;

public class PowerUpDisplay : MonoBehaviour
{
    [Header("DisplayImages")]
    public SpriteRenderer infiniShieldHighlight;
    public SpriteRenderer infiniShieldBG;
    public SpriteRenderer maceHighlight;
    public SpriteRenderer maceBG;
    public SpriteRenderer tripleShotHighlight;
    public SpriteRenderer tripleShotBG;

    // Will be added to a delegate function invoked when a powerup is activated/deactived
    void Update()
    {
        //reheeheehee

        infiniShieldHighlight.color = ScriptUtils.FindPlayerScript().CheckPowerUp("Infinite Shield") ? GameManager.GetCurrentUserColorFullAlpha() : Color.black;
        //infiniShieldBG.color = ScriptUtils.FindPlayerScript().CheckPowerUp("Infinite Shield") ? Color.white : Color.black;
        maceHighlight.color = ScriptUtils.FindPlayerScript().CheckPowerUp("Mace") ? GameManager.GetCurrentUserColorFullAlpha() : Color.black;
        //maceBG.color = ScriptUtils.FindPlayerScript().CheckPowerUp("Mace") ? Color.white : Color.black;
        tripleShotHighlight.color = ScriptUtils.FindPlayerScript().CheckPowerUp("Triple Shot") ? GameManager.GetCurrentUserColorFullAlpha() : Color.black;
        //tripleShotBG.color = ScriptUtils.FindPlayerScript().CheckPowerUp("Triple Shot") ? Color.white : Color.black;



    }
}
