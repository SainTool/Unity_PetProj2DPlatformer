using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public Door lockDoor;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("On", false);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Box")
        {
            anim.SetBool("On", true);
            lockDoor.lockDoor(false);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Box")
        {
            anim.SetBool("On", false);
            lockDoor.lockDoor(true);
        }
    }
}
