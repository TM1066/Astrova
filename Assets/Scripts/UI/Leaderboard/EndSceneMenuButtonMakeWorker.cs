using UnityEngine;

public class EndSceneMenuButtonMakeWorker : MonoBehaviour
{
    public void MakeButtonsWorkProperly()
    {
        GetComponentInChildren<Animator>().enabled = true;; 
        //this.GetComponent<Animator>().runtimeAnimatorController = null;

        Destroy(this.GetComponent<Animator>());
    }
}
