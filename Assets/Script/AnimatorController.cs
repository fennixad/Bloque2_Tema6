using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator anim;
    public float speed = 3f;
    public float velGiro = 50f;
    private bool running;

    void Start()
    {
        anim = GetComponent<Animator>();
        running = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement(); 
        AnimatorManager();
        Sprint();
        Attack();

    }

    void AnimatorManager()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0)
        {
            if (running)
            {
                speed = 5f;
                anim.SetBool("RunForward", true);
                anim.SetBool("WalkForward", false);
            }
            else
            {
                speed = 2f;
                anim.SetBool("WalkForward", true);
                anim.SetBool("RunForward", false);
            }
            anim.SetBool("Idle", false);
        }
        else
        {
            anim.SetBool("Idle", true);
            anim.SetBool("WalkForward", false);
            anim.SetBool("RunForward", false);
        }
    }

    void Movement()
    {
        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(0, 0, translation);

        float rotation = Input.GetAxis("Horizontal") * velGiro * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            running = true;
        } else
        {
            running = false;
        }
    }
    void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            print("Attack");
            anim.SetTrigger("AttackTrigger");
        }
    }
}
