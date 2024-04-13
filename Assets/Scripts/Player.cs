using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�ƶ��ٶ�
    public float smoothing = 0.5f;
    //��Ϣʱ��
    public float restTime = 1;
    //��ʱ��
    private float restTimer = 0;

    public AudioClip chop1Audio;
    public AudioClip chop2Audio;
    public AudioClip step1Audio;
    public AudioClip step2Audio;

    public AudioClip suda1Audio;
    public AudioClip suda2Audio;
    public AudioClip food1Audio;
    public AudioClip food2Audio;



    //��ʼλ��
    [HideInInspector]public Vector2 targetPos = new Vector2(1,1);
    
    //�����ĸ������ײ��
    private Rigidbody2D rigidbody;
    //�������ײ��
    private BoxCollider2D collider;
    //����
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //��unity��ȡ
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //�����˶��߼�
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPos, smoothing * Time.deltaTime));

        if (Game_Manager.Instance.food <= 0 || Game_Manager.Instance.isEnd == true)
        {
            return;
        }
        
        //��Ϣʱ��������
        restTimer += Time.deltaTime;
        if (restTimer < restTime) return;

        //�����ƶ� - ��ֱ+ˮƽ
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //ˮƽ����
        if (h > 0)
        {
            v = 0;
        }


        if ( h != 0 || v != 0 )
        {
            Game_Manager.Instance.ReduceFood(1);

            //�ֽ��������ײ������
            collider.enabled = false;
            //Ŀ��λ�� - ���������
            RaycastHit2D hit = Physics2D.Linecast(targetPos,targetPos + new Vector2(h,v));
            //��ȡ���֮���ٴ�����
            collider.enabled = true;

            //ͨ������ж���ײ
            if(hit.transform == null)
            {
                targetPos += new Vector2(h, v);
                Audio_Manager.Instance.RandomPlay(step1Audio,step2Audio);
                
            }
            else
            {
                switch (hit.collider.tag)
                {
                    //��ײǽ�Ͳ�������
                    case "OutWall":
                        break;
                    //��ײ�ϰ������Ҫ����
                    case "Wall":
                        //���Ŵ���������
                        animator.SetTrigger("Attack");
                        Audio_Manager.Instance.RandomPlay(chop1Audio,chop2Audio);
                        hit.collider.SendMessage("TakeDamage");
                        break;
                    case "Food":
                        Game_Manager.Instance.AddFood(10);
                        targetPos += new Vector2(h, v);
                        Audio_Manager.Instance.RandomPlay(food1Audio, food2Audio);
                        Destroy(hit.transform.gameObject);
                        break;
                    case "Soda":
                        Game_Manager.Instance.AddFood(10);
                        targetPos += new Vector2(h, v);
                        Audio_Manager.Instance.RandomPlay(suda1Audio, suda2Audio);
                        Destroy(hit.transform.gameObject);
                        break;
                    case "Enemy":
                        break;
                }
            }
            Game_Manager.Instance.OnPlayerMove();

            restTimer = 0; //�������ƶ�����Ҫ��Ϣ
        }

    }

    public void TakeDamage(int lossFood)
    {
        Game_Manager.Instance.ReduceFood(lossFood);
        animator.SetTrigger("Damage");
    }
}
