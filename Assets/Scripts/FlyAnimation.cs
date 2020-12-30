using UnityEngine;

public class FlyAnimation : MonoBehaviour
{
    // Update is called once per frame
    public GameObject[] fans = new GameObject[4];
    private float speed;
    private float tempSpeed;
    private Animator animator;
    private void Start()
    {
        this.speed = 1000f;
        this.tempSpeed = this.speed;
        this.animator = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //for demo
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("takeOff", true);
            animator.SetBool("flying", true);
            animator.SetBool("landing", false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("takeOff", false);
            animator.SetBool("flying", false);
            animator.SetBool("landing", true);
        }
        

        //code that might be used
        if (animator.GetBool("takeOff")
            || animator.GetBool("flying"))
        {
            tempSpeed = 1000f;
            foreach (GameObject f in fans)
            {
                f.transform.RotateAround(f.transform.position, f.transform.up, Time.deltaTime * speed);
            }
        }
        else if (animator.GetBool("landing"))
        {
            if(gameObject.transform.position.y != 0f)
            {
                foreach (GameObject f in fans)
                {
                    f.transform.RotateAround(f.transform.position, f.transform.up, Time.deltaTime * speed);
                }
            }
        }
    }
}
