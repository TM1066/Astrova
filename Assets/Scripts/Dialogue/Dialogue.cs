using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

}
