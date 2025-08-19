using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    public Transform[] segments; //ship should be first and mace on the other, needs to be ordered!!!
    public GameObject mace;
    public LineRenderer lineRenderer;

    void Update()
    {
        if (mace.activeSelf)
        {
            lineRenderer.positionCount = segments.Length;
            for (int i = 0; i < segments.Length; i++)
            {
                lineRenderer.SetPosition(i, segments[i].position);
            }
        }
    }
    private void Start()
    {
        var maceConnectorGradient = new Gradient();
        maceConnectorGradient.colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(GameManager.GetCurrentUserColorFullAlpha(), 0.75f),
            // kind of wordy but it's easier than writing a new logic bit above
            new GradientColorKey(GameManager.GetColorfulShipsEnabled() ? ScriptUtils.GetComplimentaryColor(GameManager.GetCurrentUserColorFullAlpha()) : Color.white, 0f)
        };
        lineRenderer.colorGradient = maceConnectorGradient;
    }
}
