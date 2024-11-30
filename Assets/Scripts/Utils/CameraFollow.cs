using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform transformToFollow;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(transformToFollow.position.x, transformToFollow.position.y, -10);
    }
}
