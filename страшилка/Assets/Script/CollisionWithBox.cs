using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithBox : MonoBehaviour
{
    public float newSpeed;
    public Player moveScript;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            moveScript.CollisionBox(newSpeed);
        }
    }
}

