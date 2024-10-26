using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    public float speed = 3f;
    public float velGiro = 50f;
    public float jumpForce = 3.5f;
    private bool running;
    private bool canAttack;
    private bool isGrounded;
    private float beetweenAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        running = false;
        canAttack = true;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        Movement(); 
        AnimatorManager();
        Sprint();
        AttackTimer();
        Attack();
        Jump();
        Death();
        TakingDamage();
    }

    void AnimatorManager()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

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
        else if (verticalInput < 0)
        {
            speed = 2f;
            anim.SetBool("WalkBack", true);
        }
        else
        {
            anim.SetBool("Idle", true);
            anim.SetBool("WalkForward", false);
            anim.SetBool("RunForward", false);
            anim.SetBool("WalkBack", false);
        }

        if (horizontalInput < 0)
        {
            anim.SetBool("WalkLeft", true);
            anim.SetBool("WalkRight", false);
        }
        else if (horizontalInput > 0)
        {
            anim.SetBool("WalkRight", true);
            anim.SetBool("WalkLeft", false);
        }
        else
        {
            anim.SetBool("WalkRight", false);
            anim.SetBool("WalkLeft", false);
        }
    }

    void Movement()
    {
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(0, 0, vertical);

        foreach (KeyCode key in new KeyCode[] {KeyCode.A, KeyCode.D, KeyCode.Q, KeyCode.E}) 
        { 
            switch (key)
            {
                case KeyCode.A:
                    if (Input.GetKey(key))
                    {
                        transform.Translate(-speed * Time.deltaTime, 0, 0);
                    }
                    break;
                case KeyCode.D:
                    if (Input.GetKey(key))
                    {
                        transform.Translate(speed * Time.deltaTime, 0, 0); 
                    }
                    break;
                case KeyCode.Q:
                    if (Input.GetKey(key))
                    {
                        transform.Rotate(0, -velGiro * Time.deltaTime, 0); 
                    }
                    break;
                case KeyCode.E:
                    if (Input.GetKey(key))
                    {
                        transform.Rotate(0, velGiro * Time.deltaTime, 0);
                    }
                    break;           
            }
        }
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

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            isGrounded = false;
        }
    }

    void Death()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetTrigger("Death");
        }
    }

    void TakingDamage()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            anim.SetTrigger("TakingDmg");
        }
    }

    void Attack()
    {
        if(Input.GetMouseButtonDown(0) && canAttack)
        {
            print("Attack");
            anim.SetTrigger("AttackTrigger");
        }
    }

    void AttackTimer()
    {
        beetweenAttack += Time.deltaTime;
        if (beetweenAttack >= 0.2f && Input.GetMouseButtonDown(0))
        {
            canAttack = true;
            beetweenAttack = 0;
        } else
        {
            canAttack = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
