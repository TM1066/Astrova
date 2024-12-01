using System.Net.Http.Headers;
using UnityEditor.VisionOS;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CameraArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<Asteroid>().IsVisible = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<Asteroid>().IsVisible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<Asteroid>().IsVisible = false;
        }
    }
}
