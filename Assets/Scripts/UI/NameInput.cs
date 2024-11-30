using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{

    public GameObject[] charInputFields; // should be gameobjects that 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject gameObjectcharInputField in charInputFields)
        {
            gameObjectcharInputField.GetComponent<Button>().onClick.AddListener(MoveToNextInputField); // Should hopefully work with
        }
    }

    void MoveToNextInputField()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
