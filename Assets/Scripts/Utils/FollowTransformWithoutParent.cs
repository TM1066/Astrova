using UnityEngine;

public class FollowTransformWithoutParent : MonoBehaviour
{
    public Transform transformToFollow;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = transformToFollow.position;
        this.transform.rotation = transformToFollow.rotation;
    }
}
