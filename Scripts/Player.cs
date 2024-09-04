using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerWalk;
    public float PlayerSprint;
    public float Boost;
    public float JumpStrenght;
    public float StopJump = 8;
    //
    public Light flash;
    //
    private float moveInput;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private bool flashlight = false;
    //
    private float Buffer;
    private int whereYouRun = 1; // 0 - left : 1 - both : 2 - right
    //
    private bool isGround;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    //
    private bool isBox = false;
    public LayerMask whatIsBox;
    
    private Animator anim;

    public AudioClip[] Footsteps;
    public AudioSource PlayerAudio;

    private void Start()
    {
        Buffer = PlayerWalk;
        anim = GetComponent<Animator>();
        anim.SetBool("StayR", true);
        flash = GetComponent<Light>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void CollisionBox(float newSpeed)
    {
        PlayerWalk = newSpeed;
    }
    private void Update()
    {
        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        isBox = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsBox);
        if ((isGround == true || isBox == true) && Input.GetKeyDown(KeyCode.Space) && flashlight == false)
        {
            rb.velocity = Vector2.up * JumpStrenght;
            if (facingRight == false)
            {
                anim.SetTrigger("jumpR");
            }
            else
            {
                anim.SetTrigger("jumpL");

            }
        }
        if (isGround == true || isBox == true)
        {
            anim.SetBool("isJumpingR", false);
            anim.SetBool("isJumpingL", false);
        }
        else
        {
            if (facingRight == false)
            {
                anim.SetBool("isJumpingR", true);
            }
            else
            {
                anim.SetBool("isJumpingL", true);
            }
        }
    }
    void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * PlayerWalk, rb.velocity.y);

        if (Input.GetKeyUp(KeyCode.F))
        {
            PlayerWalk = 0;
            if (!flashlight)//если не активен
            {
                
                flashlight = true;
                if (facingRight == false)
                {
                    anim.SetTrigger("GetFlashR");
                }
                else
                {
                    anim.SetTrigger("GetFlashL");
                }
            }
            else
            {
                flashlight = false;
                if (facingRight == false)
                {
                    anim.SetTrigger("RemovFlashR");
                }
                else
                {
                    anim.SetTrigger("RemoveFlashL");
                }
            }
        }
        if (facingRight == false && moveInput > 0)
        {
            anim.SetBool("StayR", true);
            anim.SetBool("StayL", false);
            
        }
        else if (facingRight == true && moveInput < 0)
        {
            anim.SetBool("StayR", false);
            anim.SetBool("StayL", true);
            
        }
        if (moveInput == 0)
        {
            PlayerWalk = Buffer - 1;
            whereYouRun = 1;
            anim.SetBool("isWalkR", false);
            anim.SetBool("isWalkL", false);
            anim.SetBool("isRuningL", false);
            anim.SetBool("isRuningR", false);

        }
        else
        {
            if (moveInput < 0)
            {
                facingRight = true;
                if (Input.GetKey(KeyCode.LeftShift) && (whereYouRun == 1 || whereYouRun == 0))
                {
                    anim.SetBool("isWalkR", false);
                    anim.SetBool("isWalkL", false);
                    anim.SetBool("StayL", false);
                    anim.SetBool("isRuningL", true);
                    anim.SetBool("isRuningR", false);
                    if (PlayerWalk < PlayerSprint && isGround == true)
                        PlayerWalk = PlayerWalk + Boost;
                }
                else
                {
                    if (PlayerWalk > Buffer)
                    {
                        whereYouRun = 1;
                        PlayerWalk = PlayerWalk - Boost;
                    }
                    else
                    {
                        PlayerWalk = PlayerWalk + Boost;
                    }
                    anim.SetBool("isRuningL", false);
                    anim.SetBool("isWalkR", false);
                    anim.SetBool("StayL", false);
                    anim.SetBool("isWalkL", true);
                    anim.SetBool("StayL", true);
                }

            }
            else
            {
                facingRight = false;
                if (Input.GetKey(KeyCode.LeftShift) && (whereYouRun == 1 || whereYouRun == 2))
                {

                    whereYouRun = 2;
                    anim.SetBool("isWalkR", false);
                    anim.SetBool("isWalkL", false);
                    anim.SetBool("StayL", false);
                    anim.SetBool("isRuningR", true);
                    anim.SetBool("isRuningL", false);
                    if (PlayerWalk < PlayerSprint && isGround == true)
                        PlayerWalk = PlayerWalk + Boost;
                }
                else
                {
                    if (PlayerWalk > Buffer)
                    {
                        whereYouRun = 1;
                        PlayerWalk = PlayerWalk - Boost;
                    }
                    else
                    {
                        PlayerWalk = PlayerWalk + Boost;
                    }
                    anim.SetBool("isRuningR", false);
                    anim.SetBool("isWalkR", true);
                    anim.SetBool("StayR", true);
                    anim.SetBool("isWalkL", false);
                    anim.SetBool("StayL", false);
                }
            }
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        isBox = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsBox);
        if (collision.CompareTag("Box") && flashlight == false && isBox == false)
        {
            PlayerWalk = 1f;
            if (facingRight == false)
            {
                anim.SetTrigger("PushPropR");
                anim.SetBool("PushPropR", true);
            }
            else
            {
                anim.SetTrigger("PushPropL");
                anim.SetBool("PushPropL", true);
            }
        }
        else
        {
            anim.SetBool("PushPropR", false);
            anim.SetBool("PushPropL", false);
        }
    }
    private void Footstep()
    {
        PlayerAudio.pitch = Random.Range(0.95f, 1.05f);
        int randInd = Random.Range(0, Footsteps.Length);
        PlayerAudio.PlayOneShot(Footsteps[0]);
    }
}
