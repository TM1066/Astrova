using UnityEngine;
using UnityEngine.UI;

public class FollowSliderValue : MonoBehaviour
{
    [SerializeField] private Slider slider;       // Reference to the slider
    [SerializeField] private Transform objectToMove; // The object to move
    [SerializeField] private float moveRange = 10f;  // Total distance the object can move on the x-axis

    private float lastSliderValue; // To track the previous slider value

    void Start()
    {
        if (slider != null)
            lastSliderValue = slider.value;
    }

    void Update()
    {
        if (slider == null || objectToMove == null) return;

        float sliderDelta = slider.value - lastSliderValue; // Calculate the change in slider value

        if (sliderDelta < 0) // Only move if the slider value decreases
        {
            Vector3 newPosition = objectToMove.position;
            newPosition.x += sliderDelta * moveRange; // Scale movement by sliderDelta and moveRange
            objectToMove.position = newPosition;
        }

        lastSliderValue = slider.value; // Update the last slider value
    }
}
