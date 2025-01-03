using UnityEngine;

public class PlayerDeadAnimationSetter : MonoBehaviour
{

    [SerializeField] Animator animator;



    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("SpaceShip").GetComponent<PlayerShip>().GetIsDead())
        {
            animator.SetBool("PlayerDead",true);
            this.transform.SetParent(GameObject.Find("Canvas").transform);
            this.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 16f, -1.25f);
            this.transform.rotation = Quaternion.Euler(0, 0,0);
        }
    }
}
