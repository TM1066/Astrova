using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    public Transform anchor; // The spaceship or anchor point
    public GameObject ball; // The object at the end of the rope
    public LineRenderer lineRenderer; // The LineRenderer component
    public Rigidbody2D[] ropeSegments; // Array of rope segment Rigidbody2Ds

    void Update()
    {
        // Update the LineRenderer positions
        lineRenderer.positionCount = ropeSegments.Length + 2; // +2 for anchor and ball

        // Set the anchor position
        lineRenderer.SetPosition(0, anchor.position);

        // Set the positions of each rope segment
        for (int i = 0; i < ropeSegments.Length; i++)
        {
            lineRenderer.SetPosition(i + 1, ropeSegments[i].position);
        }

        // Set the ball position
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, ball.transform.position);
    }
}
