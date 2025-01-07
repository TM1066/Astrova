using UnityEngine;

public class DialogueLine : MonoBehaviour
{
    public string speakerName;

    [TextArea(20,15)]
    public string content;

    public float outSpeed;

}
