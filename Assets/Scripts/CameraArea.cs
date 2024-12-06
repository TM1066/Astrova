using UnityEngine;

public class CameraArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<Asteroid>().IsVisible = true;
        }
        else 
        {
            try 
            {
                other.GetComponent<VisibleChecker>().IsVisible = true;
            }
            catch 
            {
                
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<Asteroid>().IsVisible = true;
        }
        else 
        {
            try 
            {
                other.GetComponent<VisibleChecker>().IsVisible = true;
            }
            catch 
            {
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<Asteroid>().IsVisible = false;
        }
        else 
        {
            try 
            {
                other.GetComponent<VisibleChecker>().IsVisible = false;
            }
            catch 
            {

            }
        }
    }
}
