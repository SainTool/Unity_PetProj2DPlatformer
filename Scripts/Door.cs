using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool loock;
    public bool right;
    private Animator anim;
    private BoxCollider2D bc;
    private Object AreaDoor;
    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        if (loock)
        {
            anim.SetBool("Open", false);
        }
        else
        {
            anim.SetBool("Open", false);
        }
    }
    public void lockDoor(bool key)
    {
        loock = key;
        if (loock)
        {
            bc.enabled = true;
            anim.SetBool("Open", false);
            anim.SetBool("Left", false);
            anim.SetBool("Right", false);
        }
        else
        {
            bc.enabled = false;
            anim.SetBool("Open", true);
            if (right) anim.SetBool("Right", true);
            else anim.SetBool("Left", true);
        }
    }
}
