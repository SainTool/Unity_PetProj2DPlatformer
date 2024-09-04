using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed;
    public Object FOW;
    private bool facingRight = false;
    private Rigidbody2D rb;
    private float moveInput;
    public Transform feetPos;
    //speed enemy 
    public float EnemyWalk;
    private float bufferWalk;
    //chech aroud time
    public float TimeRotate;
    private float bufferTimeRotate;
    //pos_enemy
    private float LastPositionTarget;
    private bool target =false;
    private bool stopWait = false;

    public enum MovementType
    {
        Moveing,
        lerping
    }

    public MovementType Type = MovementType.Moveing;
    public MovingPath Mypath;
    private IEnumerator<Transform> pointInPath;
    public float MaxDistance = .1f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bufferWalk = EnemyWalk;
        EnemyWalk = 0f;
        bufferTimeRotate = TimeRotate;
        LastPositionTarget = 0;

        pointInPath = Mypath.GetNextPathPoint();
        pointInPath.MoveNext();
        rb.position = pointInPath.Current.position;
     }
    //11
    // Update is called once per frame
    void Update()
    {
        if(pointInPath == null || pointInPath.Current == null)
        {
            return;
        }
        if (!stopWait)
        {
            if (target)
            {
                rb.velocity = new Vector2(EnemyWalk, rb.velocity.y);
            }
            else
            {
                rb.position = Vector2.MoveTowards(rb.position, pointInPath.Current.position, Time.deltaTime * Speed);
            }
            //var distanceSQ = (transform.position - pointInPath.Current.position).sqrMagnitude; // расстояние до точки
            if (pointInPath.Current.position.x >= rb.transform.position.x - MaxDistance && pointInPath.Current.position.x <= rb.transform.position.x + MaxDistance)
            {
                stopWait = true;
                pointInPath.MoveNext();
            }

            
        }
        else
        {
            if (target)
            {
                rb.velocity = new Vector2(EnemyWalk, rb.velocity.y);
            }
            TimeRotate -= Time.deltaTime;
            if (TimeRotate < 0 && EnemyWalk == 0)
            {
                stopWait = false;
                Flip();
                TimeRotate = bufferTimeRotate;
                //что-то сделать по окончанию времени
            }
            if (EnemyWalk > 0)
            {
                TimeRotate = bufferTimeRotate;
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = true;
            LastPositionTarget = GameObject.Find("Player").transform.position.x;
            if (GameObject.FindGameObjectWithTag("Player").transform.position.x > rb.transform.position.x)
                EnemyWalk = bufferWalk;
            else
                EnemyWalk = bufferWalk * -1;
        }
        else{
            if (LastPositionTarget >= rb.transform.position.x - MaxDistance && LastPositionTarget <= rb.transform.position.x + MaxDistance)
            {
                stopWait = true;
                target = false;
                EnemyWalk = 0;
            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Destroy(gameObject);
        }
    }
}
