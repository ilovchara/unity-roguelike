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
    //���б��� - ��¼������
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
        //�����ƶ��߼�
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime));
        
     }


    public void Move()
    {
        //��Һ͵��˵�������
        Vector2 offset = player.position - transform.position;
        //�������ֻ��һ�񣬾͹���
        if (offset.magnitude < 1.1f || offset.magnitude + 1f <1.1f || offset.magnitude - 1f < 0)
        {
            //����
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
                //��x���ƶ�
                if (offset.x > 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
            //����Ŀ��λ��ǰ�����
            collider.enabled = false;

            RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + new Vector2(x, y));

            //Debug.Log(hit.transform); //��bug
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
