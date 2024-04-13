using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 targetPosition;
    private Transform player;
    private Rigidbody2D rigidbody;
    
    public float smoothing = 3;
    //共有变量 - 记录攻击力
    public int lossFood = 10;

    public AudioClip attackAudio;

    private BoxCollider2D collider;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
        Game_Manager.Instance.enemyList.Add(this);
    }

     void Update()
     {
        //敌人移动逻辑
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime));
        
     }


    public void Move()
    {
        //玩家和敌人的向量差
        Vector2 offset = player.position - transform.position;
        //如果距离只有一格，就攻击
        if (offset.magnitude < 1.1f || offset.magnitude + 1f <1.1f || offset.magnitude - 1f < 0)
        {
            //攻击
            animator.SetTrigger("Attack");
            Audio_Manager.Instance.RandomPlay(attackAudio);
            player.SendMessage("TakeDamage",lossFood);
        }
        else
        {
            float x = 0, y = 0;
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {
                if (offset.y < 0)
                {
                    y = -1;
                }
                else
                {
                    y = 1;
                }
            }
            else
            {
                //向x轴移动
                if (offset.x > 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
            //设置目标位置前做检查
            collider.enabled = false;

            RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + new Vector2(x, y));

            //Debug.Log(hit.transform); //查bug
            collider.enabled = true;
            
            if (hit.transform == null) targetPosition += new Vector2(x, y);
            else
            {


                if (hit.collider.tag == "Food" || hit.collider.tag == "Soda")
                {
                    targetPosition += new Vector2(x, y);
                }
            }
        }
        
    }




}
