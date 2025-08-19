using UnityEngine;

//Coming back to this a few months later and h'yucky, no thanks, jesus

public class EndSceneMenuButtonMakeWorker : MonoBehaviour
{
    public void MakeButtonsWorkProperly()
    {
        GetComponentInChildren<Animator>().enabled = true; ;
        //this.GetComponent<Animator>().runtimeAnimatorController = null;

        Destroy(this.GetComponent<Animator>());
    }
}
